using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Set, "PnPAppSideLoading")]
    [CmdletAlias("Set-SPOAppSideLoading")]
    [CmdletHelp("Enables the App SideLoading Feature on a site",
        Category = CmdletHelpCategory.Sites)]
    public class SetAppSideLoading : SPOCmdlet
    {
        [Parameter(ParameterSetName = "On", Mandatory = true)]
        public SwitchParameter On;

        [Parameter(ParameterSetName = "Off", Mandatory = true)]
        public SwitchParameter Off;
        protected override void ExecuteCmdlet()
        {
            if (On)
            {
                ClientContext.Site.ActivateFeature(Constants.APPSIDELOADINGFEATUREID);
            }
            else
            {
                ClientContext.Site.DeactivateFeature(Constants.APPSIDELOADINGFEATUREID);
            }
        }

    }
}
