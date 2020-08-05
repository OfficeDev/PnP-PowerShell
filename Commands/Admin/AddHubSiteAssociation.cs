#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;
using OfficeDevPnP.Core.Sites;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Add, "PnPHubSiteAssociation")]
    [Alias("Connect-PnPHubSite")]
    [CmdletHelp("Connects a site to a hubsite.",
        DetailedDescription = @"Connects an existing site to a hubsite",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Add-PnPHubSiteAssociation -Site https://tenant.sharepoint.com/sites/mysite -HubSite https://tenant.sharepoint.com/sites/hubsite",
        Remarks = @"This example adds the specified site to the hubsite.", SortOrder = 1)]
    public class AddHubSiteAssociation : PnPAdminCmdlet
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