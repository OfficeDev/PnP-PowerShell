using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;
using OfficeDevPnP.Core.Utilities;

namespace SharePointPnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Remove, "PnPFile", SupportsShouldProcess = true)]
    [CmdletHelp("Removes a file.",
        Category = CmdletHelpCategory.Files)]
    [CmdletExample(
        Code = @"PS:>Remove-PnPFile -ServerRelativeUrl /sites/project/_catalogs/themes/15/company.spcolor",
        SortOrder = 1,
        Remarks = @"Removes the file company.spcolor")]
    [CmdletExample(
        Code = @"PS:>Remove-PnPFile -SiteRelativeUrl _catalogs/themes/15/company.spcolor",
        SortOrder = 2,
        Remarks = @"Removes the file company.spcolor")]
    [CmdletExample(
        Code = @"PS:>Remove-PnPFile -SiteRelativeUrl _catalogs/themes/15/company.spcolor -Recycle",
        SortOrder = 3,
        Remarks = @"Removes the file company.spcolor and saves it to the Recycle Bin")]

    public class RemoveFile : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "SERVER", HelpMessage = "Server relative URL to the file")]
        public string ServerRelativeUrl = string.Empty;

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "SITE", HelpMessage = "Site relative URL to the file")]
        public string SiteRelativeUrl = string.Empty;

        [Parameter(Mandatory = false)]
        public SwitchParameter Recycle;

        [Parameter(Mandatory = false)]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == "SITE")
            {
                var webUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
                ServerRelativeUrl = UrlUtility.Combine(webUrl, SiteRelativeUrl);
            }

            var file = SelectedWeb.GetFileByServerRelativeUrl(ServerRelativeUrl);

            ClientContext.Load(file, f => f.Name);
            ClientContext.ExecuteQueryRetry();

            if (Force || ShouldContinue(string.Format(Resources.Delete0, file.Name), Resources.Confirm))
            {
                if (Recycle)
                {
                    file.Recycle();
                }
                else
                {
                    file.DeleteObject();
                }

                ClientContext.ExecuteQueryRetry();
            }

        }
    }
}
