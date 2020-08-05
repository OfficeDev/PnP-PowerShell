using PnP.PowerShell.CmdletHelpAttributes;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPPowerShellTelemetryEnabled")]
    [CmdletHelp("Returns true if the PnP PowerShell Telemetry has been enabled.",
        "In order to help to make PnP PowerShell better, we can track anonymous telemetry. We track the version of the cmdlets you are using, which cmdlet you are executing and which version of SharePoint you are connecting to. Use Disable-PnPPowerShellTelemetry to turn this off, alternative, use the -NoTelemetry switch on Connect-PnPOnline to turn it off for that session.",
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
       Code = "PS:> Get-PnPPowerShellTelemetryEnabled",
       Remarks = "Will return true of false.",
       SortOrder = 1)]
    public class GetPowerShellTelemetryEnabled : PnPSharePointCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(PnPConnection.CurrentConnection.TelemetryClient != null);
        }
    }
}