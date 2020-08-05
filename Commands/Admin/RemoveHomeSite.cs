#if !ONPREMISES
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Remove, "PnPHomeSite")]
    [CmdletHelp("Removes the currently set site as the home site",
     SupportedPlatform = CmdletSupportedPlatform.Online,
     Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
     Code = @"PS:> Remove-PnPHomeSite",
     Remarks = @"Removes the currently set site as the home site", SortOrder = 1)]
    public class RemoveHomeSite : PnPAdminCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            var homesiteUrl = Tenant.GetSPHSiteUrl();
            ClientContext.ExecuteQueryRetry();
            if (!string.IsNullOrEmpty(homesiteUrl.Value))
            {
                if (Force || ShouldContinue($"Remove {homesiteUrl.Value} as the home site?", Properties.Resources.Confirm))
                {
                    Tenant.RemoveSPHSite();
                    ClientContext.ExecuteQueryRetry();
                }
            }
            else
            {
                WriteWarning("There is currently not site collection set as a home site in your tenant.");
            }
        }
    }
}
#endif