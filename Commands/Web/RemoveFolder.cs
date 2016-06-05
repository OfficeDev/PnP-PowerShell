using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;
using OfficeDevPnP.Core.Utilities;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Remove, "SPOFolder")]
    [CmdletHelp("Deletes a folder within a parent folder",
        Category = CmdletHelpCategory.Webs)]
    [CmdletExample(
        Code = @"PS:> Remove-SPOFolder -Name NewFolder -Folder _catalogs/masterpage/newfolder",
        SortOrder = 1)]
    public class RemoveFolder : SPOWebCmdlet
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

            Folder folder = SelectedWeb.GetFolderByServerRelativeUrl(UrlUtility.Combine(SelectedWeb.ServerRelativeUrl, Folder,Name));

            ClientContext.Load(folder, f => f.Name);
 //           ClientContext.Load(folder, f => f.ServerRelativeUrl);
            ClientContext.ExecuteQueryRetry();
 //           WriteObject("Folder" + folder.Name + folder.ServerRelativeUrl);

            if (Force || ShouldContinue(string.Format(Resources.Delete0, folder.Name), Resources.Confirm))
            {
                folder.DeleteObject();

                ClientContext.ExecuteQueryRetry();
            }
        }
    }
}

