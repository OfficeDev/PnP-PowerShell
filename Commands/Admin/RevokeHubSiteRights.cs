#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsSecurity.Revoke, "PnPHubSiteRights")]
    [CmdletHelp(@"Revoke permissions to the permissions already in place to associate sites to Hub Sites for one or more specific users",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(Code = @"PS:> Revoke-PnPHubSiteRights -Identity https://contoso.sharepoint.com/sites/hubsite -Principals ""myuser@mydomain.com"",""myotheruser@mydomain.com""", Remarks = "This example shows how to revoke the rights of myuser and myotheruser to associate their sites with the provided Hub Site", SortOrder = 1)]
    public class RevokeHubSiteRights : PnPAdminCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true, HelpMessage = "The Hub Site to revoke the permissions on to associate another site with this Hub Site")]
        [Alias("HubSite")]
        public HubSitePipeBind Identity { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "One or more usernames that will be revoked the permission to associate a site with this Hub Site.")]
        public string[] Principals { get; set; }

        protected override void ExecuteCmdlet()
        {
            base.Tenant.RevokeHubSiteRights(Identity.Url ?? Identity.GetHubSite(Tenant).SiteUrl, Principals);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif
