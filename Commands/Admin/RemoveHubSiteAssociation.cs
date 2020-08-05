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
    [Cmdlet(VerbsCommon.Remove, "PnPHubSiteAssociation")]
    [Alias("Disconnect-PnPHubSite")]
    [CmdletHelp("Disconnects a site from a hubsite.",
        DetailedDescription = @"Disconnects an site from a hubsite",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPHubSiteAssociation -Site https://tenant.sharepoint.com/sites/mysite",
        Remarks = @"This example adds the specified site to the hubsite.", SortOrder = 1)]
    public class RemoveHubSiteAssociation : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"The site to disconnect from its hubsite")]
        public SitePipeBind Site;

        protected override void ExecuteCmdlet()
        {
            Tenant.DisconnectSiteFromHubSite(Site.Url);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif