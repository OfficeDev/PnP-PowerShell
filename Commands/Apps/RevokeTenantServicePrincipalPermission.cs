#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration.Internal;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsSecurity.Revoke, "PnPTenantServicePrincipalPermission")]
    [CmdletHelp(@"Revokes a permission that was previously granted to the ""SharePoint Online Client"" service principal.",
        DetailedDescription = @"Revokes a permission that was previously granted to the ""SharePoint Online Client"" service principal.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    public class RevokeTenantServicePrincipal : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true)]
        public string ObjectId;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (Force || ShouldContinue("Revoke permission?","Continue"))
            {
                var servicePrincipal = new SPOWebAppServicePrincipal(ClientContext);
                var grant = servicePrincipal.PermissionGrants.GetByObjectId(ObjectId);
                grant.DeleteObject();
                ClientContext.ExecuteQuery();
            }
        }
    }
}
#endif