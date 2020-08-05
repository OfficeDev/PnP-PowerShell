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
    [Cmdlet(VerbsCommon.Remove, "PnPSiteCollectionAppCatalog")]
    [CmdletHelp("Removes a Site Collection scoped App Catalog from a site",
        DetailedDescription = "Notice that this will not remove the App Catalog list and its contents from the site.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPSiteCollectionAppCatalog -Site ""https://contoso.sharepoint.com/sites/FinanceTeamsite""",
        Remarks = @"This will remove a SiteCollection app catalog from the specified site", SortOrder = 1)]
    public class RemoveSiteCollectionAppCatalog: PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"Url of the site to remove the app catalog from.")]
        public SitePipeBind Site;

        protected override void ExecuteCmdlet()
        {
            string url = null;
            if(Site.Site != null)
            {
                Site.Site.EnsureProperty(s => s.Url);
                url = Site.Site.Url;
            } else if(!string.IsNullOrEmpty(Site.Url))
            {
                url = Site.Url;
            }
            
            Tenant.GetSiteByUrl(url).RootWeb.TenantAppCatalog.SiteCollectionAppCatalogsSites.Remove(url);
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif