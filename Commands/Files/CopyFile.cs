using System;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Text.RegularExpressions;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;
using OfficeDevPnP.Core.Utilities;
using File = Microsoft.SharePoint.Client.File;

namespace SharePointPnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Copy, "PnPFile", SupportsShouldProcess = true, DefaultParameterSetName = "SOURCEURL")]
    [CmdletHelp("Copies a file or folder to a different location",
        Category = CmdletHelpCategory.Files)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in a document library called Documents in the current site to the site collection otherproject. If a file named company.docx already exists, it won't perform the copy.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl /sites/otherproject/Documents/company.docx",
        SortOrder = 1)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in a document library called Documents to a new document named company2.docx in the same library.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl Documents/company2.docx",
        SortOrder = 2)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in a document library called Documents to a document library called Documents2 in the same site. ",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl Documents2/company.docx",
        SortOrder = 3)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in a document library called Documents to the document library named Document in a subsite named Subsite as a new document named company2.docx.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl Subsite/Documents/company2.docx",
        SortOrder = 4)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in a document library called Documents to the document library named Document in a subsite named Subsite keeping the file name.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl Subsite/Documents",
        SortOrder = 5)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in a document library called Documents in the current site to the site collection otherproject. If a file named company.docx already exists, it will still perform the copy and replace the original company.docx file.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl /sites/otherproject/Documents/company.docx -OverwriteIfAlreadyExists",
        SortOrder = 6)]
    [CmdletExample(
        Remarks = "Copies a folder named MyDocs in the document library called Documents located in the current site to the site collection otherproject. If the MyDocs folder exist it will copy into it, if not it will be created.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/MyDocs -TargetUrl /sites/otherproject/Documents -OverwriteIfAlreadyExists",
        SortOrder = 7)]
    [CmdletExample(
        Remarks = "Copies a folder named MyDocs in the document library called Documents located in the current site to the root folder of the library named Documents in the site collection otherproject.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/MyDocs -TargetUrl /sites/otherproject/Documents -SkipSourceFolderName -OverwriteIfAlreadyExists",
        SortOrder = 8)]
    [CmdletExample(
        Remarks = "Copies a folder named MyDocs in the MyDocs folder of the library named Documents. If the MyDocs folder does not exists, it will be created.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/MyDocs -TargetUrl /sites/otherproject/Documents/MyDocs -SkipSourceFolderName -OverwriteIfAlreadyExists",
        SortOrder = 9)]
    [CmdletExample(
        Remarks = "Copies a folder named MyDocs in the root of the library named Documents. If the MyDocs folder exists in the target, a subfolder also named MyDocs is created.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/MyDocs -TargetUrl /sites/otherproject/Documents/MyDocs -OverwriteIfAlreadyExists",
        SortOrder = 10)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx in the library named Documents in SubSite1 to the library named Documents in SubSite2.",
        Code = @"PS:>Copy-PnPFile -SourceUrl SubSite1/Documents/company.docx -TargetUrl SubSite2/Documents",
        SortOrder = 10)]

    public class CopyFile : PnPWebCmdlet
    {
        private ProgressRecord _progressFolder = new ProgressRecord(0, "Activity", "Status") { Activity = "Copying folder" };
        private ProgressRecord _progressFile = new ProgressRecord(1, "Activity", "Status") { Activity = "Copying file" };
        private ClientContext _sourceContext;
        private ClientContext _targetContext;

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "SERVER", HelpMessage = "Server relative Url specifying the file or folder to copy.")]
        [Obsolete("Use SourceUrl instead.")]
        public string ServerRelativeUrl = string.Empty;

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "SOURCEURL", HelpMessage = "Site relative Url specifying the file or folder to copy.")]
        [Alias("SiteRelativeUrl")]
        public string SourceUrl = string.Empty;

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Server relative Url where to copy the file or folder to.")]
        public string TargetUrl = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "If provided, if a file already exists at the TargetUrl, it will be overwritten. If ommitted, the copy operation will be canceled if the file already exists at the TargetUrl location.")]
        public SwitchParameter OverwriteIfAlreadyExists;

        [Parameter(Mandatory = false, HelpMessage = "If provided, no confirmation will be requested and the action will be performed")]
        public SwitchParameter Force;

        [Parameter(Mandatory = false, HelpMessage = "If the source is a folder, the source folder name will not be created, only the contents within it.")]
        public SwitchParameter SkipSourceFolderName;

        protected override void ExecuteCmdlet()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            SourceUrl = SourceUrl ?? ServerRelativeUrl;
#pragma warning restore CS0618 // Type or member is obsolete
            var webServerRelativeUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

            if (!SourceUrl.StartsWith("/"))
            {
                SourceUrl = UrlUtility.Combine(webServerRelativeUrl, SourceUrl);
            }
            if (!TargetUrl.StartsWith("/"))
            {
                TargetUrl = UrlUtility.Combine(webServerRelativeUrl, TargetUrl);
            }

            Uri currentContextUri = new Uri(ClientContext.Url);
            Uri sourceUri = new Uri(currentContextUri, SourceUrl);
            Uri sourceWebUri = Microsoft.SharePoint.Client.Web.WebUrlFromFolderUrlDirect(ClientContext, sourceUri);
            Uri targetUri = new Uri(currentContextUri, TargetUrl);
            Uri targetWebUri = Microsoft.SharePoint.Client.Web.WebUrlFromFolderUrlDirect(ClientContext, targetUri);

            _sourceContext = ClientContext;
            if (!currentContextUri.AbsoluteUri.Equals(sourceWebUri.AbsoluteUri, StringComparison.InvariantCultureIgnoreCase))
            {
                _sourceContext = ClientContext.Clone(sourceWebUri);
            }

            File file = _sourceContext.Web.GetFileByServerRelativeUrl(SourceUrl);
            Folder folder = _sourceContext.Web.GetFolderByServerRelativeUrl(SourceUrl);
            file.EnsureProperties(f => f.Name, f => f.Exists);
#if !SP2013
            folder.EnsureProperties(f => f.Name, f => f.Exists);
            bool srcIsFolder = folder.Exists;
#else
            folder.EnsureProperties(f => f.Name);
            bool srcIsFolder;
            try
            {
                folder.EnsureProperties(f => f.ItemCount); //Using ItemCount as marker if this is a file or folder
                srcIsFolder = true;
            }
            catch
            {
                srcIsFolder = false;
            }

#endif

            if (Force || ShouldContinue(string.Format(Resources.CopyFile0To1, SourceUrl, TargetUrl), Resources.Confirm))
            {
                var srcWeb = _sourceContext.Web;
                srcWeb.EnsureProperty(s => s.Url);

                _targetContext = ClientContext.Clone(targetWebUri.AbsoluteUri);
                var dstWeb = _targetContext.Web;
                dstWeb.EnsureProperty(s => s.Url);
                if (srcWeb.Url == dstWeb.Url)
                {
                    try
                    {
                        var targetFile = UrlUtility.Combine(TargetUrl, file.Name);
                        // If src/dst are on the same Web, then try using CopyTo - backwards compability
                        file.CopyTo(targetFile, OverwriteIfAlreadyExists);
                        _sourceContext.ExecuteQueryRetry();
                        return;
                    }
                    catch
                    {
                        SkipSourceFolderName = true; // target folder exist
                        //swallow exception, in case target was a lib/folder which exists
                    }
                }

                //different site/site collection
                Folder targetFolder = null;
                string fileOrFolderName = null;
                bool targetFolderExists = false;
                try
                {
                    targetFolder = _targetContext.Web.GetFolderByServerRelativeUrl(TargetUrl);
#if !SP2013
                    targetFolder.EnsureProperties(f => f.Name, f => f.Exists);
                    if (!targetFolder.Exists) throw new Exception("TargetUrl is an existing file, not folder");
                     targetFolderExists = true;
#else
                    targetFolder.EnsureProperties(f => f.Name);
                    try
                    {
                        targetFolder.EnsureProperties(f => f.ItemCount); //Using ItemCount as marker if this is a file or folder
                        targetFolderExists = true;
                    }
                    catch
                    {
                        targetFolderExists = false;
                    }
                    if (!targetFolderExists) throw new Exception("TargetUrl is an existing file, not folder");
#endif
                }
                catch (Exception)
                {
                    targetFolder = null;
                    Expression<Func<List, object>> expressionRelativeUrl = l => l.RootFolder.ServerRelativeUrl;
                    var query = _targetContext.Web.Lists.IncludeWithDefaultProperties(expressionRelativeUrl);
                    var lists = _targetContext.LoadQuery(query);
                    _targetContext.ExecuteQueryRetry();
                    lists = lists.OrderByDescending(l => l.RootFolder.ServerRelativeUrl); // order descending in case more lists start with the same
                    foreach (List targetList in lists)
                    {
                        if (!TargetUrl.StartsWith(targetList.RootFolder.ServerRelativeUrl, StringComparison.InvariantCultureIgnoreCase)) continue;
                        fileOrFolderName = Regex.Replace(TargetUrl, targetList.RootFolder.ServerRelativeUrl, "", RegexOptions.IgnoreCase).Trim('/');
                        targetFolder = srcIsFolder ? targetList.RootFolder.EnsureFolder(fileOrFolderName) : targetList.RootFolder;
                        break;
                    }
                }
                if (targetFolder == null) throw new Exception("Target does not exist");
                if (srcIsFolder)
                {
                    if (!SkipSourceFolderName && targetFolderExists)
                    {
                        targetFolder = targetFolder.EnsureFolder(folder.Name);
                    }
                    CopyFolder(folder, targetFolder);
                }
                else
                {
                    UploadFile(file, targetFolder, fileOrFolderName);
                }
            }
        }

        private void CopyFolder(Folder sourceFolder, Folder targetFolder)
        {
            sourceFolder.EnsureProperties(f => f.ServerRelativeUrl, f => f.Files, f => f.Folders, folder => folder.Files.Include(f => f.ServerRelativeUrl));
            targetFolder.EnsureProperty(f => f.ServerRelativeUrl);

            _progressFolder.RecordType = ProgressRecordType.Processing;
            _progressFolder.StatusDescription = $"{sourceFolder.ServerRelativeUrl} to {targetFolder.ServerRelativeUrl}";
            _progressFolder.PercentComplete = 0;
            WriteProgress(_progressFolder);

            _progressFile.RecordType = ProgressRecordType.Processing;
            foreach (File file in sourceFolder.Files)
            {
                _progressFile.StatusDescription = $"{file.ServerRelativeUrl}";
                _progressFile.PercentComplete = 0;
                WriteProgress(_progressFile);
                UploadFile(file, targetFolder);
                _progressFile.PercentComplete = 100;
                WriteProgress(_progressFile);
            }
            _progressFile.RecordType = ProgressRecordType.Completed;
            WriteProgress(_progressFile);

            _progressFolder.RecordType = ProgressRecordType.Processing;
            _progressFolder.PercentComplete = 100;
            WriteProgress(_progressFolder);

            foreach (Folder folder in sourceFolder.Folders)
            {
                var childFolder = targetFolder.EnsureFolder(folder.Name);
                CopyFolder(folder, childFolder);
            }
        }

        private void UploadFile(File srcFile, Folder targetFolder, string filename = "")
        {
            var binaryStream = srcFile.OpenBinaryStream();
            _sourceContext.ExecuteQueryRetry();
            if (string.IsNullOrWhiteSpace(filename)) filename = srcFile.Name;
            targetFolder.UploadFile(filename, binaryStream.Value, OverwriteIfAlreadyExists);
            _targetContext.ExecuteQueryRetry();
        }
    }
}
