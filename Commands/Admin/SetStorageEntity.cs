#if !ONPREMISES
using System.Linq;
using System.Management.Automation;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Enums;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "PnPStorageEntity", SupportsShouldProcess = true)]
    [CmdletHelp(@"Set Storage Entities / Farm Properties in either the tenant scoped app catalog or the site collection app catalog.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(Code = @"PS:> Set-PnPStorageEntity -Key MyKey -Value ""MyValue"" -Comment ""My Comment"" -Description ""My Description""", Remarks = "Sets an existing or adds a new storage entity / farm property at tenant level.", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Set-PnPStorageEntity -Scope Site -Key MyKey -Value ""MyValue"" -Comment ""My Comment"" -Description ""My Description""", Remarks = "Sets an existing or adds a new storage entity site collection level.", SortOrder = 2)]
    public class SetPnPStorageEntity : PnPCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The key of the value to set.")]
        public string Key;

        [Parameter(Mandatory = true, HelpMessage = "The value to set.")]
        public string Value;

        [Parameter(Mandatory = false, HelpMessage = "The comment to set.")]
        [AllowNull]
        public string Comment;

        [Parameter(Mandatory = false, HelpMessage = "The description to set.")]
        [AllowNull]
        public string Description;

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
                        clonedContext.Web.SetStorageEntity(Key, Value, Description, Comment);
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
                    ClientContext.Site.RootWeb.SetStorageEntity(Key, Value, Description, Comment);
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