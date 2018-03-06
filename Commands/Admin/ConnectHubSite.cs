#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System.Management.Automation;
using OfficeDevPnP.Core.Sites;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;

namespace SharePointPnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommunications.Connect, "PnPHubSite")]
    [CmdletHelp("Connects a site to a hubsite.",
        DetailedDescription = @"Connects an existing site to a hubsite",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPHubSite -Site https://tenant.sharepoint.com/sites/mysite -HubSite https://tenant.sharepoint.com/sites/hubsite",
        Remarks = @"This example adds the specified site to the hubsite.", SortOrder = 1)]
    public class ConnectHubSite : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"The site to connect to the hubsite")]
        public SitePipeBind Site;

        [Parameter(Mandatory = true, HelpMessage = "The hubsite to connect the site to")]
        public SitePipeBind HubSite;

        protected override void ExecuteCmdlet()
        {
            Tenant.ConnectSiteToHubSite(Site.Url, HubSite.Url);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif