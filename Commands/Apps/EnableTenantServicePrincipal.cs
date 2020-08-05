#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration.Internal;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsLifecycle.Enable, "PnPTenantServicePrincipal", ConfirmImpact = ConfirmImpact.High)]
    [CmdletHelp(@"Enables the current tenant's ""SharePoint Online Client"" service principal.",
        DetailedDescription = @"Enables the current tenant's ""SharePoint Online Client"" service principal.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    public class EnableTenantServicePrincipal : PnPAdminCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (ShouldContinue("Do you want to enable the Tenant Service Principal?", "Continue?"))
            {
                var servicePrincipal = new SPOWebAppServicePrincipal(ClientContext);
                servicePrincipal.AccountEnabled = true;
                servicePrincipal.Update();
                ClientContext.Load(servicePrincipal);
                ClientContext.ExecuteQueryRetry();
                WriteObject(servicePrincipal);
            }
        }
    }
}
#endif