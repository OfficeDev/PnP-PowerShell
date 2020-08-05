using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Set, "PnPAppSideLoading")]
    [CmdletHelp("Enables the App SideLoading Feature on a site",
        Category = CmdletHelpCategory.Sites)]
    [CmdletExample(
        Code = @"PS:> Set-PnPAppSideLoading -On",
        Remarks = @"This will turn on App side loading",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPAppSideLoading -Off",
        Remarks = @"This will turn off App side loading",
        SortOrder = 1)]
    public class SetAppSideLoading : PnPSharePointCmdlet
    {
        [Parameter(ParameterSetName = "On", Mandatory = true)]
        public SwitchParameter On;

        [Parameter(ParameterSetName = "Off", Mandatory = true)]
        public SwitchParameter Off;
        protected override void ExecuteCmdlet()
        {
            if (On)
            {
                ClientContext.Site.ActivateFeature(Constants.FeatureId_Site_AppSideLoading);
            }
            else
            {
                ClientContext.Site.DeactivateFeature(Constants.FeatureId_Site_AppSideLoading);
            }
        }

    }
}
