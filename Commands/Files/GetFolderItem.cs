using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using File = Microsoft.SharePoint.Client.File;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Get, "PnPFolderItem")]
    [CmdletHelp("List content in folder", Category = CmdletHelpCategory.Files, SupportedPlatform = CmdletSupportedPlatform.All)]
    [CmdletExample(
        Code = @"PS:> Get-PnPFolderItem -FolderSiteRelativeUrl ""SitePages""",
        Remarks = "Returns the contents of the folder SitePages which is located in the root of the current web",
        SortOrder = 1
        )]
    [CmdletExample(
        Code = @"PS:> Get-PnPFolderItem -FolderSiteRelativeUrl ""SitePages"" -ItemName ""Default.aspx""",
        Remarks = "Returns the file 'Default.aspx' which is located in the folder SitePages which is located in the root of the current web",
        SortOrder = 2
        )]
    [CmdletExample(
        Code = @"PS:> Get-PnPFolderItem -FolderSiteRelativeUrl ""SitePages"" -ItemType Folder",
        Remarks = "Returns all folders in the folder SitePages which is located in the root of the current web",
        SortOrder = 3
        )]
    [CmdletExample(
        Code = @"PS:> Get-PnPFolderItem -FolderSiteRelativeUrl ""SitePages"" -ItemType File",
        Remarks = "Returns all files in the folder SitePages which is located in the root of the current web",
        SortOrder = 4
        )]
    [CmdletExample(
        Code = @"PS:> Get-PnPFolderItem -FolderSiteRelativeUrl ""SitePages"" -Recursive",
        Remarks = "Returns all files and folders, including contents of any subfolders, in the folder SitePages which is located in the root of the current web",
        SortOrder = 5
        )]
    public class GetFolderItem : PnPWebCmdlet
    {
        private const string ParameterSet_FOLDERSBYPIPE = "Folder via pipebind";
        private const string ParameterSet_FOLDERBYURL = "Folder via url";

        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "The site relative URL of the folder to retrieve", ParameterSetName = ParameterSet_FOLDERBYURL)]
        public string FolderSiteRelativeUrl;

        [Parameter(Mandatory = false, Position = 0, HelpMessage = "A folder instance to the folder to retrieve", ParameterSetName = ParameterSet_FOLDERSBYPIPE)]
        public FolderPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "The type of contents to retrieve, either File, Folder or All (default)")]
        [ValidateSet("Folder", "File", "All")]
        public string ItemType = "All";

        [Parameter(Mandatory = false, HelpMessage = "Optional name of the item to retrieve")]
        public string ItemName = string.Empty;

        [Parameter(Mandatory = false, Position = 4, HelpMessage = "A switch parameter to include contents of all subfolders in the specified folder")]
        public SwitchParameter Recursive;

        private IEnumerable<object> GetContents(string FolderSiteRelativeUrl)
        {
            Folder targetFolder = null;
            if (ParameterSetName == ParameterSet_FOLDERSBYPIPE && Identity != null)
            {
                targetFolder = Identity.GetFolder(SelectedWeb);
            }
            else
            {
                string serverRelativeUrl = null;
                if (!string.IsNullOrEmpty(FolderSiteRelativeUrl))
                {
                    var webUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
                    serverRelativeUrl = UrlUtility.Combine(webUrl, FolderSiteRelativeUrl);
                }

#if ONPREMISES
                targetFolder = (string.IsNullOrEmpty(FolderSiteRelativeUrl)) ? SelectedWeb.RootFolder : SelectedWeb.GetFolderByServerRelativeUrl(serverRelativeUrl);
#else
                targetFolder = (string.IsNullOrEmpty(FolderSiteRelativeUrl)) ? SelectedWeb.RootFolder : SelectedWeb.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(serverRelativeUrl));
#endif
            }

            IEnumerable<File> files = null;
            IEnumerable<Folder> folders = null;

            if (ItemType == "File" || ItemType == "All")
            {
                files = ClientContext.LoadQuery(targetFolder.Files).OrderBy(f => f.Name);
                if (!string.IsNullOrEmpty(ItemName))
                {
                    files = files.Where(f => f.Name.Equals(ItemName, StringComparison.InvariantCultureIgnoreCase));
                }
            }
            if (ItemType == "Folder" || ItemType == "All" || Recursive)
            {
                folders = ClientContext.LoadQuery(targetFolder.Folders).OrderBy(f => f.Name);
                if (!string.IsNullOrEmpty(ItemName))
                {
                    folders = folders.Where(f => f.Name.Equals(ItemName, StringComparison.InvariantCultureIgnoreCase));
                }
            }
            ClientContext.ExecuteQueryRetry();
            
            IEnumerable<object> folderContent = null;
            switch (ItemType)
            {
                case "All":
                    folderContent = folders.Concat<object>(files);                    
                    break;
                case "Folder":
                    folderContent = folders;
                    break;
                default:
                    folderContent = files;
                    break;
            }
            
            if(Recursive && folders.Count() > 0)
            {
                foreach(var folder in folders)
                {
                    var relativeUrl = folder.ServerRelativeUrl.Replace(SelectedWeb.ServerRelativeUrl, "");
                    var subFolderContents = GetContents(relativeUrl);
                    folderContent = folderContent.Concat<object>(subFolderContents);
                }                
            }

            return folderContent;
        }

        protected override void ExecuteCmdlet()
        {
            var contents = GetContents(FolderSiteRelativeUrl);
            WriteObject(contents, true);
        }
    }
}
