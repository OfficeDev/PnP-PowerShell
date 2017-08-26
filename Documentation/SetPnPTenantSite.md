# Set-PnPTenantSite
Uses the tenant API to set site information.
>*Only available for SharePoint Online*
## Syntax
```powershell
Set-PnPTenantSite -Url <String>
                  [-Title <String>]
                  [-Sharing <Nullable`1>]
                  [-StorageMaximumLevel <Nullable`1>]
                  [-StorageWarningLevel <Nullable`1>]
                  [-UserCodeMaximumLevel <Nullable`1>]
                  [-UserCodeWarningLevel <Nullable`1>]
                  [-AllowSelfServiceUpgrade <Nullable`1>]
                  [-Owners <List`1>]
                  [-LockState <SiteLockState>]
                  [-NoScriptSite [<SwitchParameter>]]
                  [-Wait [<SwitchParameter>]]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Url|String|True|Specifies the URL of the site|
|AllowSelfServiceUpgrade|Nullable`1|False|Specifies if the site administrator can upgrade the site collection|
|LockState|SiteLockState|False|Sets the lockstate of a site|
|NoScriptSite|SwitchParameter|False|Specifies if a site allows custom script or not. See https://support.office.com/en-us/article/Turn-scripting-capabilities-on-or-off-1f2c515f-5d7e-448a-9fd7-835da935584f for more information.|
|Owners|List`1|False|Specifies owners to add as site collection adminstrators. Can be both users and groups.|
|Sharing|Nullable`1|False|Specifies what the sharing capablilites are for the site. Possible values: Disabled, ExternalUserSharingOnly, ExternalUserAndGuestSharing, ExistingExternalUserSharingOnly|
|StorageMaximumLevel|Nullable`1|False|Specifies the storage quota for this site collection in megabytes. This value must not exceed the company's available quota.|
|StorageWarningLevel|Nullable`1|False|Specifies the warning level for the storage quota in megabytes. This value must not exceed the values set for the StorageMaximumLevel parameter|
|Title|String|False|Specifies the title of the site|
|UserCodeMaximumLevel|Nullable`1|False|Specifies the quota for this site collection in Sandboxed Solutions units. This value must not exceed the company's aggregate available Sandboxed Solutions quota. The default value is 0. For more information, see Resource Usage Limits on Sandboxed Solutions in SharePoint 2010 : http://msdn.microsoft.com/en-us/library/gg615462.aspx.|
|UserCodeWarningLevel|Nullable`1|False|Specifies the warning level for the resource quota. This value must not exceed the value set for the UserCodeMaximumLevel parameter|
|Wait|SwitchParameter|False|Wait for the operation to complete|
## Examples

### Example 1
```powershell
PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com -Title 'Contoso Website' -Sharing Disabled
```
This will set the title of the site collection with the URL 'https://contoso.sharepoint.com' to 'Contoso Website' and disable sharing on this site collection.

### Example 2
```powershell
PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com -Title 'Contoso Website' -StorageWarningLevel 8000 -StorageMaximumLevel 10000
```
This will set the title of the site collection with the URL 'https://contoso.sharepoint.com' to 'Contoso Website', set the storage warning level to 8GB and set the storage maximum level to 10GB.

### Example 3
```powershell
PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com/sites/sales -Owners 'user@contoso.onmicrosoft.com'
```
This will set user@contoso.onmicrosoft.com as a site collection owner at 'https://contoso.sharepoint.com/sites/sales'.

### Example 4
```powershell
PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com/sites/sales -NoScriptSite:$false
```
This will enable script support for the site 'https://contoso.sharepoint.com/sites/sales' if disabled.
