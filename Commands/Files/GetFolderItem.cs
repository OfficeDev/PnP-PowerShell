using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using File = Microsoft.SharePoint.Client.File;

namespace SharePointPnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Get, "PnPFolderItem")]
    [CmdletHelp("List content in folder", Category = CmdletHelpCategory.Files)]
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
    public class GetFolderItem : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage ="The site relative folder to retrieve")]
        public string FolderSiteRelativeUrl;

        [Parameter(Mandatory = false, HelpMessage = "The type of contents to retrieve, either File, Folder or All (default)")]
        [ValidateSet("Folder", "File", "All")]
        public string ItemType = "All";

        [Parameter(Mandatory = false, HelpMessage = "Optional name of the item to retrieve")]
        public string ItemName = string.Empty;

        protected override void ExecuteCmdlet()
        {
            string serverRelativeUrl = null;
            if (!string.IsNullOrEmpty(FolderSiteRelativeUrl))
            {
                var webUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
                serverRelativeUrl = UrlUtility.Combine(webUrl, FolderSiteRelativeUrl);
            }

#if ONPREMISES
            var targetFolder = (string.IsNullOrEmpty(FolderSiteRelativeUrl)) ? SelectedWeb.RootFolder : SelectedWeb.GetFolderByServerRelativeUrl(serverRelativeUrl);
#else
            var targetFolder = (string.IsNullOrEmpty(FolderSiteRelativeUrl)) ? SelectedWeb.RootFolder : SelectedWeb.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(serverRelativeUrl));
#endif
            IEnumerable<File> files = null;
            IEnumerable<Folder> folders = null;

            if (ItemType == "File" || ItemType == "All")
            {
                files = ClientContext.LoadQuery(targetFolder.Files).OrderBy(f => f.Name);
                if (!string.IsNullOrEmpty(ItemName))
                {
                    files = files.Where(f=>f.Name.Equals(ItemName, StringComparison.InvariantCultureIgnoreCase));
                }
            }
            if (ItemType == "Folder" || ItemType == "All")
            {
                folders = ClientContext.LoadQuery(targetFolder.Folders).OrderBy(f => f.Name);
                if (!string.IsNullOrEmpty(ItemName))
                {
                    folders = folders.Where(f => f.Name.Equals(ItemName, StringComparison.InvariantCultureIgnoreCase));
                }
            }
            ClientContext.ExecuteQueryRetry();

            switch (ItemType)
            {
                case "All":
                    var foldersAndFiles = folders.Concat<object>(files);
                    WriteObject(foldersAndFiles, true);
                    break;
                case "Folder":
                    WriteObject(folders, true);
                    break;
                default:
                    WriteObject(files, true);
                    break;
            }
        }
    }
}
