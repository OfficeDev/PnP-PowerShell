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
    [Cmdlet(VerbsCommon.Get, "PnPStorageEntity", SupportsShouldProcess = true)]
    [CmdletHelp(@"Retrieve Storage Entities / Farm Properties.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(Code = @"PS:> Get-PnPStorageEntity", Remarks = "Returns all site storage entities/farm properties", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPStorageEntity -Key MyKey", Remarks = "Returns the storage entity/farm property with the given key.", SortOrder = 2)]
    public class GetPnPStorageEntity : PnPCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The key of the value to retrieve.")]
        public string Key;

        protected override void ExecuteCmdlet()
        {          
            var appCatalogUri = ClientContext.Web.GetAppCatalog();
            using (var clonedContext = ClientContext.Clone(appCatalogUri))
            {             
                var storageEntitiesIndex = clonedContext.Web.GetPropertyBagValueString("storageentitiesindex", "");

                if (storageEntitiesIndex != "")
                {
                    var storageEntitiesDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(storageEntitiesIndex);

                    var storageEntities = new List<StorageEntity>();
                    foreach (var key in storageEntitiesDict.Keys)
                    {
                        var storageEntity = new StorageEntity {
                            Key = key,
                            Value = storageEntitiesDict[key]["Value"],
                            Comment = storageEntitiesDict[key]["Comment"],
                            Description = storageEntitiesDict[key]["Description"]
                        };
                        storageEntities.Add(storageEntity);
                    }
                    if (MyInvocation.BoundParameters.ContainsKey("Key"))
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