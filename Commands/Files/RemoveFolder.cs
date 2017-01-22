using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;
using OfficeDevPnP.Core.Utilities;

namespace SharePointPnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Remove, "PnPFolder")]
    [CmdletAlias("Remove-SPOFolder")]
    [CmdletHelp("Deletes a folder within a parent folder",
        Category = CmdletHelpCategory.Files)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPFolder -Name NewFolder -Folder _catalogs/masterpage",
        SortOrder = 1,
        Remarks = @"Removes the folder 'NewFolder' from '_catalogsmasterpage'")]
    public class RemoveFolder : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The folder name")]
        public string Name = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The parent folder in the site")]
        public string Folder = string.Empty;

        [Parameter(Mandatory = false)]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

            Folder folder = SelectedWeb.GetFolderByServerRelativeUrl(UrlUtility.Combine(SelectedWeb.ServerRelativeUrl, Folder, Name));

            folder.EnsureProperty(f => f.Name);

            if (Force || ShouldContinue(string.Format(Resources.Delete0, folder.Name), Resources.Confirm))
            {
                folder.DeleteObject();

                ClientContext.ExecuteQueryRetry();
            }
        }
    }
}

