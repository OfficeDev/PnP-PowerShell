using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Utilities;

namespace SharePointPnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Add, "PnPFolder")]
    [CmdletHelp("Creates a folder within a parent folder",
        Category = CmdletHelpCategory.Files)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFolder -Name NewFolder -Folder _catalogs/masterpage",
		Remarks = "This will create the folder NewFolder in the masterpage catalog",
        SortOrder = 1)]
    public class AddFolder : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The folder name")]
        public string Name = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The parent folder in the site")]
        public string Folder = string.Empty;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
                        
            Folder folder = SelectedWeb.GetFolderByServerRelativeUrl(UrlUtility.Combine(SelectedWeb.ServerRelativeUrl, Folder));
            ClientContext.Load(folder, f => f.ServerRelativeUrl);
            ClientContext.ExecuteQueryRetry();

            var result = folder.CreateFolder(Name);

            WriteObject(result);
        }
    }
}
