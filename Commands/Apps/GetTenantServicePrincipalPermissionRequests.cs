#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration.Internal;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsCommon.Get, "PnPTenantServicePrincipalPermissionRequests")]
    [CmdletHelp(@"Gets the collection of permission requests for the ""SharePoint Online Client"" service principal",
        DetailedDescription = @"Gets the collection of permission requests for the ""SharePoint Online Client"" service principal.

Permission request object

A permission request contains the following properties:

* Id: The identifier of the request.
* Resource: The resource that the application requires access to.
* Scope: The value of the scope claim that the resource application should expect in the OAuth 2.0 access token.
",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    public class GetTenantServicePrincipalPermissionRequests : PnPAdminCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var servicePrincipal = new SPOWebAppServicePrincipal(ClientContext);
            var requests = servicePrincipal.PermissionRequests;
            ClientContext.Load(requests);
            ClientContext.ExecuteQueryRetry();
            WriteObject(requests, true);
        }

    }
}
#endif