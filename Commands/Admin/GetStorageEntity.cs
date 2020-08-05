#if !ONPREMISES
using System.Linq;
using System.Management.Automation;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Enums;
using System.Collections.Generic;
using System.Text.Json;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPStorageEntity", SupportsShouldProcess = true)]
    [CmdletHelp(@"Retrieve Storage Entities / Farm Properties from either the Tenant App Catalog or from the current site if it has a site scope app catalog.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(Code = @"PS:> Get-PnPStorageEntity", Remarks = "Returns all site storage entities/farm properties", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPStorageEntity -Key MyKey", Remarks = "Returns the storage entity/farm property with the given key.", SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Get-PnPStorageEntity -Scope Site", Remarks = "Returns all site collection scoped storage entities", SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Get-PnPStorageEntity -Key MyKey -Scope Site", Remarks = "Returns the storage entity from the site collection with the given key", SortOrder = 3)]
    public class GetPnPStorageEntity : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The key of the value to retrieve.")]
        public string Key;

        [Parameter(Mandatory = false, HelpMessage = "Defines the scope of the storage entity. Defaults to Tenant.")]
        public StorageEntityScope Scope = StorageEntityScope.Tenant;

        protected override void ExecuteCmdlet()
        {
            string storageEntitiesIndex = string.Empty;
            if (Scope == StorageEntityScope.Tenant)
            {
                var appCatalogUri = ClientContext.Web.GetAppCatalog();
                if(appCatalogUri != null)
                {
                    using (var clonedContext = ClientContext.Clone(appCatalogUri))
                    {
                        storageEntitiesIndex = clonedContext.Web.GetPropertyBagValueString("storageentitiesindex", "");
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
                    storageEntitiesIndex = ClientContext.Site.RootWeb.GetPropertyBagValueString("storageentitiesindex", "");
                } else
                {
                    WriteWarning("Site Collection App Catalog is not available on this site.");
                }
            }

            if (!string.IsNullOrEmpty(storageEntitiesIndex))
            {
                var storageEntitiesDict = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(storageEntitiesIndex);

                var storageEntities = new List<StorageEntity>();
                foreach (var key in storageEntitiesDict.Keys)
                {
                    var storageEntity = new StorageEntity
                    {
                        Key = key,
                        Value = storageEntitiesDict[key]["Value"],
                        Comment = storageEntitiesDict[key]["Comment"],
                        Description = storageEntitiesDict[key]["Description"]
                    };
                    storageEntities.Add(storageEntity);
                }
                if (ParameterSpecified(nameof(Key)))
                {
                    WriteObject(storageEntities.Where(k => k.Key == Key));
                }
                else
                {
                    WriteObject(storageEntities, true);
                }
            }
        }
    }

    public class StorageEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
    }
}
#endif