using System;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Text.RegularExpressions;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using Resources = PnP.PowerShell.Commands.Properties.Resources;
using OfficeDevPnP.Core.Utilities;
using File = Microsoft.SharePoint.Client.File;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Copy, "PnPFile", SupportsShouldProcess = true)]
    [CmdletHelp("Copies a file or folder to a different location. This location can be within the same document library, same site, same site collection or even to another site collection on the same tenant. Currently there is a 200MB file size limit for the file or folder to be copied.",
        Category = CmdletHelpCategory.Files)]
#if !ONPREMISES
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in a document library called Shared Documents in the site collection project to the Shared Documents library in the site collection otherproject. If a file named company.docx already exists, it won't perform the copy.",
        Code = @"PS:>Copy-PnPFile -ServerRelativeUrl ""/sites/project/Shared Documents/company.docx"" -TargetServerRelativeLibrary ""/sites/otherproject/Shared Documents""",
        SortOrder = 1)]
    [CmdletExample(
        Remarks = "Copies a folder named Archive located in a document library called Shared Documents in the site collection project to the Shared Documents library in the site collection otherproject. If a folder named Archive already exists, it will overwrite it.",
        Code = @"PS:>Copy-PnPFile -ServerRelativeUrl ""/sites/project/Shared Documents/Archive"" -TargetServerRelativeLibrary ""/sites/otherproject/Shared Documents"" -OverwriteIfAlreadyExists",
        SortOrder = 2)]
#endif
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in a document library called Documents to a new document named company2.docx in the same library.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl Documents/company2.docx",
        SortOrder = 3)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in a document library called Documents to a document library called Documents2 in the same site. ",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl Documents2/company.docx",
        SortOrder = 4)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in a document library called Documents to the document library named Document in a subsite named Subsite as a new document named company2.docx.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl Subsite/Documents/company2.docx",
        SortOrder = 5)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in a document library called Documents to the document library named Document in a subsite named Subsite keeping the file name.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl Subsite/Documents",
        SortOrder = 6)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx located in a document library called Documents in the current site to the site collection otherproject. If a file named company.docx already exists, it will still perform the copy and replace the original company.docx file.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl /sites/otherproject/Documents/company.docx -OverwriteIfAlreadyExists",
        SortOrder = 7)]
    [CmdletExample(
        Remarks = "Copies a folder named MyDocs in the document library called Documents located in the current site to the site collection otherproject. If the MyDocs folder exist it will copy into it, if not it will be created.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/MyDocs -TargetUrl /sites/otherproject/Documents -OverwriteIfAlreadyExists",
        SortOrder = 8)]
    [CmdletExample(
        Remarks = "Copies a folder named MyDocs in the document library called Documents located in the current site to the root folder of the library named Documents in the site collection otherproject.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/MyDocs -TargetUrl /sites/otherproject/Documents -SkipSourceFolderName -OverwriteIfAlreadyExists",
        SortOrder = 0)]
    [CmdletExample(
        Remarks = "Copies a folder named MyDocs in the MyDocs folder of the library named Documents. If the MyDocs folder does not exists, it will be created.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/MyDocs -TargetUrl /sites/otherproject/Documents/MyDocs -SkipSourceFolderName -OverwriteIfAlreadyExists",
        SortOrder = 10)]
    [CmdletExample(
        Remarks = "Copies a folder named MyDocs in the root of the library named Documents. If the MyDocs folder exists in the target, a subfolder also named MyDocs is created.",
        Code = @"PS:>Copy-PnPFile -SourceUrl Documents/MyDocs -TargetUrl /sites/otherproject/Documents/MyDocs -OverwriteIfAlreadyExists",
        SortOrder = 11)]
    [CmdletExample(
        Remarks = "Copies a file named company.docx in the library named Documents in SubSite1 to the library named Documents in SubSite2.",
        Code = @"PS:>Copy-PnPFile -SourceUrl SubSite1/Documents/company.docx -TargetUrl SubSite2/Documents",
        SortOrder = 12)]

    public class CopyFile : PnPWebCmdlet
    {
        private ProgressRecord _progressFolder = new ProgressRecord(0, "Activity", "Status") { Activity = "Copying folder" };
        private ProgressRecord _progressFile = new ProgressRecord(1, "Activity", "Status") { Activity = "Copying file" };
        private ClientContext _sourceContext;
        private ClientContext _targetContext;

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Site or server relative Url specifying the file or folder to copy. Must include the file name if it's a file or the entire path to the folder if it's a folder.")]
        [Alias("SiteRelativeUrl", "ServerRelativeUrl")] // Aliases are present to allow for switching between Move-PnPFile and Copy-PnPFile keeping the same parameters.
        public string SourceUrl = string.Empty;

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Server relative Url where to copy the file or folder to. Must not include the file name.")]
#if !ONPREMISES
        [Alias(nameof(MoveFile.TargetServerRelativeLibrary))] // Aliases is present to allow for switching between Move-PnPFile and Copy-PnPFile keeping the same parameters.
#endif
        public string TargetUrl = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "If provided, if a file already exists at the TargetUrl, it will be overwritten. If omitted, the copy operation will be canceled if the file already exists at the TargetUrl location.")]
        public SwitchParameter OverwriteIfAlreadyExists;

        [Parameter(Mandatory = false, HelpMessage = "If provided, no confirmation will be requested and the action will be performed")]
        public SwitchParameter Force;

        [Parameter(Mandatory = false, HelpMessage = "If the source is a folder, the source folder name will not be created, only the contents within it")]
        public SwitchParameter SkipSourceFolderName;

#if !ONPREMISES
        [Parameter(Mandatory = false, HelpMessage = "If provided, only the latest version of the document will be copied and its history will be discared. If not provided, all historical versions will be copied along.")]
        public SwitchParameter IgnoreVersionHistory;
#endif

        protected override void ExecuteCmdlet()
        {
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

            if (Force || ShouldContinue(string.Format(Resources.CopyFile0To1, SourceUrl, TargetUrl), Resources.Confirm))
            {
#if !ONPREMISES
                if (sourceWebUri != targetWebUri)
                {
                    CopyToOtherSiteCollection(sourceUri, targetUri);
                }
                else
                {
#endif
                    CopyWithinSameSiteCollection(currentContextUri, sourceWebUri, targetWebUri);
#if !ONPREMISES
                }
#endif
            }
        }

        /// <summary>
        /// Allows copying to within the same site collection
        /// </summary>
        private void CopyWithinSameSiteCollection(Uri currentContextUri, Uri sourceWebUri, Uri targetWebUri)
        {
            _sourceContext = ClientContext;
            if (!currentContextUri.AbsoluteUri.Equals(sourceWebUri.AbsoluteUri, StringComparison.InvariantCultureIgnoreCase))
            {
                _sourceContext = ClientContext.Clone(sourceWebUri);
            }

            bool isFile = true;
            bool srcIsFolder = false;

            File file = null;
            Folder folder = null;

            try
            {
#if ONPREMISES
                file = _sourceContext.Web.GetFileByServerRelativeUrl(SourceUrl);
#else
                file = _sourceContext.Web.GetFileByServerRelativePath(ResourcePath.FromDecodedUrl(SourceUrl));
#endif
                file.EnsureProperties(f => f.Name, f => f.Exists);
                isFile = file.Exists;
            }
            catch
            {
                isFile = false;
            }

            if (!isFile)
            {
#if ONPREMISES
                folder = _sourceContext.Web.GetFolderByServerRelativeUrl(SourceUrl);
#else
                folder = _sourceContext.Web.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(SourceUrl));
#endif

#if !SP2013
                folder.EnsureProperties(f => f.Name, f => f.Exists);
                srcIsFolder = folder.Exists;
#else
                folder.EnsureProperties(f => f.Name);

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
            }

            var srcWeb = _sourceContext.Web;
            srcWeb.EnsureProperty(s => s.Url);

            _targetContext = ClientContext.Clone(targetWebUri.AbsoluteUri);
            var dstWeb = _targetContext.Web;
            dstWeb.EnsureProperties(s => s.Url, s => s.ServerRelativeUrl);
            if (srcWeb.Url == dstWeb.Url)
            {
                try
                {
                    var targetFile = UrlUtility.Combine(TargetUrl, file?.Name);
                    // If src/dst are on the same Web, then try using CopyTo - backwards compability
#if ONPREMISES
                        file?.CopyTo(targetFile, OverwriteIfAlreadyExists);
#else
                    file?.CopyToUsingPath(ResourcePath.FromDecodedUrl(targetFile), OverwriteIfAlreadyExists);
#endif
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
#if ONPREMISES
                    targetFolder = _targetContext.Web.GetFolderByServerRelativeUrl(TargetUrl);
#else
                targetFolder = _targetContext.Web.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(TargetUrl));
#endif
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
                    fileOrFolderName = Regex.Replace(TargetUrl, _targetContext.Web.ServerRelativeUrl, "", RegexOptions.IgnoreCase).Trim('/');
                    targetFolder = srcIsFolder
                        ? _targetContext.Web.EnsureFolderPath(fileOrFolderName)
                        : targetList.RootFolder;
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

#if !ONPREMISES
        /// <summary>
        /// Allows copying to another site collection
        /// </summary>
        private void CopyToOtherSiteCollection(Uri source, Uri destination)
        {
            ClientContext.Site.CreateCopyJobs(new[] { source.ToString() }, destination.ToString(), new CopyMigrationOptions
            {
                IsMoveMode = false,
                IgnoreVersionHistory = IgnoreVersionHistory.ToBool(),
                NameConflictBehavior = OverwriteIfAlreadyExists.ToBool() ? MigrationNameConflictBehavior.Replace : MigrationNameConflictBehavior.Fail
            });
            ClientContext.ExecuteQueryRetry();
        }
#endif

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
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = srcFile.Name;
            }
            this.UploadFileWithSpecialCharacters(targetFolder, filename, binaryStream.Value, OverwriteIfAlreadyExists);
            _targetContext.ExecuteQueryRetry();
        }

        private File UploadFileWithSpecialCharacters(Folder folder, string fileName, System.IO.Stream stream, bool overwriteIfExists)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Filename is required");
            }

            // Create the file
            var newFileInfo = new FileCreationInformation()
            {
                ContentStream = stream,
                Url = fileName,
                Overwrite = overwriteIfExists
            };

            var file = folder.Files.Add(newFileInfo);
            folder.Context.Load(file);
            folder.Context.ExecuteQueryRetry();

            return file;
        }
    }
}
