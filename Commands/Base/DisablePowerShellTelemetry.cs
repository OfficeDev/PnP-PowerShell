using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
#if NETSTANDARD2_0
using System.IdentityModel.Tokens.Jwt;
#else
using System.IdentityModel.Tokens;
#endif
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsLifecycle.Disable, "PnPPowerShellTelemetry")]
    [CmdletHelp("Disables PnP PowerShell telemetry tracking",
        "Disables PnP PowerShell telemetry tracking",
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
       Code = "PS:> Disable-PnPPowerShellTelemetry",
       Remarks = "Will prompt you to confirm to disable telemetry tracking.",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Disable-PnPPowerShellTelemetry -Force",
       Remarks = "Will disable telemetry tracking without prompting.",
       SortOrder = 2)]
    public class DisablePowerShellTelemetry : PSCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ProcessRecord()
        {
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var telemetryFile = System.IO.Path.Combine(userProfile, ".pnppowershelltelemetry");
            if (Force || ShouldContinue("Do you want to disable telemetry for PnP PowerShell?", "Confirm"))
            {
                System.IO.File.WriteAllText(telemetryFile, "disallow");
                if (SPOnlineConnection.CurrentConnection != null)
                {
                    SPOnlineConnection.CurrentConnection.TelemetryClient = null;
                }
                WriteObject("Telemetry disabled");
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