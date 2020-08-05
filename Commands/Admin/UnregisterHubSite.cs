#if !ONPREMISES
using System;
using System.Linq;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsLifecycle.Unregister, "PnPHubSite")]
    [CmdletHelp("Unregisters a site as a hubsite",
        DetailedDescription = @"Registers a site as a hubsite",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Unregister-PnPHubSite -Site https://tenant.sharepoint.com/sites/myhubsite",
        Remarks = @"This example unregisters the specified site as a hubsite", SortOrder = 1)]
    public class UnregisterHubSite : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"The site to unregister as a hubsite")]
        public SitePipeBind Site;

        protected override void ExecuteCmdlet()
        {
            var hubSitesProperties = Tenant.GetHubSitesProperties();
            ClientContext.Load(hubSitesProperties);
            ClientContext.ExecuteQueryRetry();
            HubSiteProperties props = null;
            if (Site.Id != Guid.Empty)
            {
                props = hubSitesProperties.Single(h => h.SiteId == Site.Id);
            }
            else
            {
                props = hubSitesProperties.Single(h => h.SiteUrl.Equals(Site.Url, StringComparison.OrdinalIgnoreCase));
            }
            Tenant.UnregisterHubSiteById(props.ID);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif