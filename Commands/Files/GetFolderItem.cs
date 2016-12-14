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
    [CmdletAlias("Get-SPOFolderItem")]
    [CmdletHelp("List content in folder", Category = CmdletHelpCategory.Files)]
    public class GetFolderItem : SPOWebCmdlet
    {

        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true)]
        public string FolderSiteRelativeUrl;

        [Parameter(Mandatory = false)]
        [ValidateSet("Folder", "File", "All")]
        public string ItemType = "All";

        [Parameter(Mandatory = false)]
        public string ItemName = string.Empty;

        protected override void ExecuteCmdlet()
        {
            string serverRelativeUrl = null;
            if (!string.IsNullOrEmpty(FolderSiteRelativeUrl))
            {
                var webUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
                serverRelativeUrl = UrlUtility.Combine(webUrl, FolderSiteRelativeUrl);
            }

            var targetFolder = (string.IsNullOrEmpty(FolderSiteRelativeUrl)) ? SelectedWeb.RootFolder : SelectedWeb.GetFolderByServerRelativeUrl(serverRelativeUrl);
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
