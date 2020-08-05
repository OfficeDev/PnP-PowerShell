#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Enums;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Remove, "PnPStorageEntity", SupportsShouldProcess = true)]
    [CmdletHelp(@"Remove Storage Entities / Farm Properties from either the tenant scoped app catalog or the current site collection if the site has a site collection scoped app catalog",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(Code = @"PS:> Remove-PnPStorageEntity -Key MyKey ", Remarks = "Removes an existing storage entity / farm property", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Remove-PnPStorageEntity -Key MyKey -Scope Site", Remarks = "Removes an existing storage entity from the current site collection", SortOrder = 1)]
    public class RemovePnPStorageEntity : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The key of the value to remove.")]
        public string Key;

        [Parameter(Mandatory = false, HelpMessage = "Defines the scope of the storage entity. Defaults to Tenant.")]
        public StorageEntityScope Scope = StorageEntityScope.Tenant;

        protected override void ExecuteCmdlet()
        {
            if (Scope == StorageEntityScope.Tenant)
            {
                var appCatalogUri = ClientContext.Web.GetAppCatalog();
                if(appCatalogUri != null)
                {
                    using (var clonedContext = ClientContext.Clone(appCatalogUri))
                    {
                        clonedContext.Web.RemoveStorageEntity(Key);
                        clonedContext.ExecuteQueryRetry();
                    }
                }
                else
                {
                    WriteWarning("Tenant app catalog is not available on this tenant.");
                }                
            }
            else
            {
                var appcatalog = ClientContext.Site.RootWeb.SiteCollectionAppCatalog;
                ClientContext.Load(appcatalog);
                ClientContext.ExecuteQueryRetry();
                if (appcatalog.ServerObjectIsNull == false)
                {
                    ClientContext.Site.RootWeb.RemoveStorageEntity(Key);
                    ClientContext.ExecuteQueryRetry();
                }
                else
                {
                    WriteWarning("Site Collection App Catalog is not available on this site.");
                }
            }
        }
    }
}
#endif