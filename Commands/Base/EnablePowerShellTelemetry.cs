using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsLifecycle.Enable, "PnPPowerShellTelemetry")]
    [CmdletHelp("Enables PnP PowerShell telemetry tracking.",
        "In order to help to make PnP PowerShell better, we can track anonymous telemetry. We track the version of the cmdlets you are using, which cmdlet you are executing and which version of SharePoint you are connecting to. Use Disable-PnPPowerShellTelemetry to turn this off, alternative, use the -NoTelemetry switch on Connect-PnPOnline to turn it off for that session.",
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
       Code = "PS:> Enable-PnPPowerShellTelemetry",
       Remarks = "Will prompt you to confirm to enable telemetry tracking.",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Enable-PnPPowerShellTelemetry -Force",
       Remarks = "Will enable telemetry tracking without prompting.",
       SortOrder = 2)]
    public class EnablePowerShellTelemetry : PSCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ProcessRecord()
        {
            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var telemetryFile = System.IO.Path.Combine(userFolder, ".pnppowershelltelemetry");
            if (Force || ShouldContinue("Do you want to enable telemetry for PnP PowerShell?", "Confirm"))
            {
                SPOnlineConnection.CurrentConnection?.InitializeTelemetry(SPOnlineConnection.CurrentConnection.Context, Host);
                System.IO.File.WriteAllText(telemetryFile, "allow");
                WriteObject("Telemetry enabled");
            }
            else
            {
                var enabled = false;
                if (System.IO.File.Exists(telemetryFile))
                {
                    enabled = System.IO.File.ReadAllText(telemetryFile).ToLower() == "allow";
                }
                WriteObject($"Telemetry setting unchanged: currently {(enabled ? "enabled" : "disabled")}");
            }
        }
    }
}