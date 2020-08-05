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
    [Cmdlet(VerbsSecurity.Grant, "PnPSiteDesignRights", SupportsShouldProcess = true)]
    [CmdletHelp(@"Grants the specified principals rights to use the site design.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Grant-PnPSiteDesignRights -Identity 5c73382d-9643-4aa0-9160-d0cba35e40fd -Principals ""myuser@mydomain.com"",""myotheruser@mydomain.com""",
        Remarks = "Grants the specified principals View rights on the site design specified",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteDesign -Title ""MySiteDesign"" -SiteScriptIds 438548fd-60dd-42cf-b843-2db506c8e259 -WebTemplate TeamSite | Grant-PnPSiteDesignRights -Principals ""myuser@mydomain.com"",""myotheruser@mydomain.com""",
        Remarks = "Grants the specified principals View rights on the site design specified",
        SortOrder = 1)]
    public class GrantSiteDesignRights : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline =true, HelpMessage = "The site design to use.")]
        public TenantSiteDesignPipeBind Identity;

        [Parameter(Mandatory = true, HelpMessage = "One or more principals to grant rights to.")]
        public string[] Principals;

        [Parameter(Mandatory = false, HelpMessage = "The rights to set. Defaults to 'View'")]
        public TenantSiteDesignPrincipalRights Rights = TenantSiteDesignPrincipalRights.View;

        protected override void ExecuteCmdlet()
        {
            Tenant.GrantSiteDesignRights(Identity.Id, Principals, Rights);
            ClientContext.ExecuteQueryRetry();
        }
    }
}         
#endif