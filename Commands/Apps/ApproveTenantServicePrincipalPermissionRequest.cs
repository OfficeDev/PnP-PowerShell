#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration.Internal;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsLifecycle.Approve, "PnPTenantServicePrincipalPermissionRequest")]
    [CmdletHelp(@"Approves a permission request for the current tenant's ""SharePoint Online Client"" service principal",
        DetailedDescription = @"Approves a permission request for the current tenant's ""SharePoint Online Client"" service principal

The return value of a successful call is a permission grant object.

To get the collection of permission grants for the ""SharePoint Online Client"" service principal, use the Get-PnPTenantServicePrincipalPermissionGrants command.

Approving a permission request also removes that request from the list of permission requests.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    public class ApproveTenantServicePrincipalPermissionRequests : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true)]
        public GuidPipeBind RequestId;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (Force || ShouldContinue($"Approve request {RequestId.Id}?", "Continue"))
            {
                var servicePrincipal = new SPOWebAppServicePrincipal(ClientContext);
                var request = servicePrincipal.PermissionRequests.GetById(RequestId.Id);
                var grant = request.Approve();
                ClientContext.Load(grant);
                ClientContext.ExecuteQueryRetry();
                WriteObject(new TenantServicePrincipalPermissionGrant(grant));
            }
        }

    }
}
#endif