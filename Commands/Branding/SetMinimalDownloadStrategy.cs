using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using Resources = OfficeDevPnP.PowerShell.Commands.Properties.Resources;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "SPOMinimalDownloadStrategy")]
    [CmdletHelp("Activates or deactivates the minimal downloading strategy.", 
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(
        Code = @"PS:> Set-SPOMinimalDownloadStrategy -Off",
        Remarks = "Will deactivate minimal download strategy (MDS) for the current web.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-SPOMinimalDownloadStrategy -On",
        Remarks = "Will activate minimal download strategy (MDS) for the current web.",
        SortOrder = 2)]
    public class SetMDS : SPOWebCmdlet
    {
        [Parameter(ParameterSetName = "On", Mandatory = true)]
        public SwitchParameter On;

        [Parameter(ParameterSetName = "Off", Mandatory = true)]
        public SwitchParameter Off;

        [Parameter(Mandatory = false)]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (On)
            {
                SelectedWeb.Features.Add(Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID, Force, FeatureDefinitionScope.None);
            }
            else
            {
                SelectedWeb.Features.Remove(Core.Constants.MINIMALDOWNLOADSTRATEGYFEATUREID, Force);
            }
            ClientContext.ExecuteQueryRetry();
        }
    }

}
