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
    [Cmdlet(VerbsLifecycle.Register, "PnPHubSite")]
    [CmdletHelp("Registers a site as a hubsite",
        DetailedDescription = @"Registers a site as a hubsite",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Register-PnPHubSite -Site https://tenant.sharepoint.com/sites/myhubsite",
        Remarks = @"This example registers the specified site as a hubsite", SortOrder = 1)]
    public class RegisterHubSite : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"The site to register as a hubsite")]
        public SitePipeBind Site;

        protected override void ExecuteCmdlet()
        {
            Tenant.RegisterHubSite(Site.Url);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif