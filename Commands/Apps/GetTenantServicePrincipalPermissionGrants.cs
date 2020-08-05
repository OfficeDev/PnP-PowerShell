#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration.Internal;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Model;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsCommon.Get, "PnPTenantServicePrincipalPermissionGrants")]
    [CmdletHelp(@"Gets the collection of permission grants for the ""SharePoint Online Client"" service principal",
        DetailedDescription = @"Gets the collection of permission grants for the ""SharePoint Online Client"" service principal.

A permission grant contains the following properties:

* ClientId: The objectId of the service principal granted consent to impersonate the user when accessing the resource(represented by the resourceId).
* ConsentType: Whether consent was provided by the administrator on behalf of the organization or whether consent was provided by an individual.The possible values are ""AllPrincipals"" or ""Principal"".
* ObjectId: The unique identifier for the permission grant.
* Resource: The resource to which access has been granted (Coming soon)
* ResourceId: The objectId of the resource service principal to which access has been granted.
* Scope: The value of the scope claim that the resource application should expect in the OAuth 2.0 access token.
",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    public class GetTenantServicePrincipalPermissionGrants : PnPAdminCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var servicePrincipal = new SPOWebAppServicePrincipal(ClientContext);
            var permissionGrants = servicePrincipal.PermissionGrants;
            ClientContext.Load(permissionGrants);
            ClientContext.ExecuteQueryRetry();
            WriteObject(permissionGrants.Select(g => new TenantServicePrincipalPermissionGrant(g)), true);
        }

    }
}
#endif