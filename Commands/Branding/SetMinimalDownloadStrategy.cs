using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Set, "PnPMinimalDownloadStrategy")]
    [CmdletHelp("Activates or deactivates the minimal downloading strategy.",
        "Activates or deactivates the minimal download strategy feature of a site",
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(
        Code = @"PS:> Set-PnPMinimalDownloadStrategy -Off",
        Remarks = "Will deactivate minimal download strategy (MDS) for the current web.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPMinimalDownloadStrategy -On",
        Remarks = "Will activate minimal download strategy (MDS) for the current web.",
        SortOrder = 2)]
    public class SetMinimalDownloadStrategy : PnPWebCmdlet
    {
        [Parameter(ParameterSetName = "On", Mandatory = true, HelpMessage = "Turn minimal download strategy on")]
        public SwitchParameter On;

        [Parameter(ParameterSetName = "Off", Mandatory = true, HelpMessage = "Turn minimal download strategy off")]
        public SwitchParameter Off;
         
        [Parameter(Mandatory = false, HelpMessage = "Specifies whether to overwrite (when activating) or continue (when deactivating) an existing feature with the same feature identifier. This parameter is ignored if there are no errors.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (On)
            {
                SelectedWeb.Features.Add(OfficeDevPnP.Core.Constants.FeatureId_Web_MinimalDownloadStrategy, Force, FeatureDefinitionScope.None);
            }
            else
            {
                SelectedWeb.Features.Remove(OfficeDevPnP.Core.Constants.FeatureId_Web_MinimalDownloadStrategy, Force);
            }
            ClientContext.ExecuteQueryRetry();
        }
    }

}
