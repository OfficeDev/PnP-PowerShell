#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsSecurity.Revoke, "PnPSiteDesignRights", SupportsShouldProcess = true)]
    [CmdletHelp(@"Revokes the specified principals rights to use the site design.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Revoke-PnPSiteDesignRights -Identity 5c73382d-9643-4aa0-9160-d0cba35e40fd -Principals ""myuser@mydomain.com"",""myotheruser@mydomain.com""",
        Remarks = "Revokes rights to the specified principals on the site design specified",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteDesign -Identity 5c73382d-9643-4aa0-9160-d0cba35e40fd | Revoke-PnPSiteDesignRights -Principals ""myuser@mydomain.com"",""myotheruser@mydomain.com""",
        Remarks = "Revokes rights to the specified principals on the site design specified",
        SortOrder = 1)]
    public class RevokeSiteDesignRights : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline =true, HelpMessage = "The site design to use.")]
        public TenantSiteDesignPipeBind Identity;

        [Parameter(Mandatory = true, HelpMessage = "One or more principals to revoke.")]
        public string[] Principals;

        protected override void ExecuteCmdlet()
        {
            Tenant.RevokeSiteDesignRights(ClientContext, Identity.Id, Principals);
            ClientContext.ExecuteQueryRetry();
        }
    }
}         
#endif