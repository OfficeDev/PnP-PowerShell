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
            Tenant.UnregisterHubSite(Site.Url);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif