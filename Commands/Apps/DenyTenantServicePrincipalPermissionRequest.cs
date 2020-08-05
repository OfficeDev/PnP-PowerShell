#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration.Internal;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsLifecycle.Deny, "PnPTenantServicePrincipalPermissionRequest")]
    [CmdletHelp(@"Denies a permission request for the current tenant's ""SharePoint Online Client"" service principal",
        DetailedDescription = @"Denies a permission request for the current tenant's ""SharePoint Online Client"" service principal

Denying a permission request removes that request from the list of permission requests.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    public class DenyTenantServicePrincipalPermissionRequests : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true)]
        public GuidPipeBind RequestId;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (Force || ShouldContinue($"Deny request {RequestId.Id}?", "Continue"))
            {
                var servicePrincipal = new SPOWebAppServicePrincipal(ClientContext);
                var request = servicePrincipal.PermissionRequests.GetById(RequestId.Id);
                request.Deny();
                ClientContext.ExecuteQueryRetry();
            }
        }

    }
}
#endif