using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using Resources = PnP.PowerShell.Commands.Properties.Resources;
using OfficeDevPnP.Core.Utilities;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Remove, "PnPFolder")]
    [CmdletHelp("Deletes a folder within a parent folder",
        Category = CmdletHelpCategory.Files)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPFolder -Name NewFolder -Folder _catalogs/masterpage",
        SortOrder = 1,
        Remarks = @"Removes the folder 'NewFolder' from '_catalogsmasterpage'")]
    [CmdletExample(
        Code = @"PS:> Remove-PnPFolder -Name NewFolder -Folder _catalogs/masterpage -Recycle",
        SortOrder = 2,
        Remarks = @"Removes the folder 'NewFolder' from '_catalogsmasterpage' and is saved in the Recycle Bin")]
    public class RemoveFolder : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The folder name")]
        public string Name = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The parent folder in the site")]
        public string Folder = string.Empty;

        [Parameter(Mandatory = false)]
        public SwitchParameter Recycle;

        [Parameter(Mandatory = false)]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

#if ONPREMISES
            Folder folder = SelectedWeb.GetFolderByServerRelativeUrl(UrlUtility.Combine(SelectedWeb.ServerRelativeUrl, Folder, Name));
#else
            var folderUrl = UrlUtility.Combine(SelectedWeb.ServerRelativeUrl, Folder, Name);
            Folder folder = SelectedWeb.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(folderUrl));
#endif
            folder.EnsureProperty(f => f.Name);

            if (Force || ShouldContinue(string.Format(Resources.Delete0, folder.Name), Resources.Confirm))
            {
                if (Recycle)
                {
                    folder.Recycle();
                }
                else
                {
                    folder.DeleteObject();
                }

                ClientContext.ExecuteQueryRetry();
            }
        }
    }
}

