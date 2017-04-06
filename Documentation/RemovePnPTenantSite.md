# Remove-PnPTenantSite
Office365 only: Removes a site collection from the current tenant
## Syntax
```powershell
Remove-PnPTenantSite -Url <String>
                     [-SkipRecycleBin [<SwitchParameter>]]
                     [-Force [<SwitchParameter>]]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Url|String|True|Specifies the full URL of the site collection that needs to be deleted|
|Force|SwitchParameter|False|Do not ask for confirmation.|
|SkipRecycleBin|SwitchParameter|False|Do not add to the tenant scoped recycle bin when selected.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPTenantSite -Url https://tenant.sharepoint.com/sites/contoso
```
This will remove the site collection with the url 'https://tenant.sharepoint.com/sites/contoso'  and put it in the recycle bin.

### Example 2
```powershell
PS:> Remove-PnPTenantSite -Url https://tenant.sharepoint.com/sites/contoso -Force -SkipRecycleBin
```
This will remove the site collection with the url 'https://tenant.sharepoint.com/sites/contoso' with force and it will skip the recycle bin.

### Example 3
```powershell
PS:> Remove-PnPTenantSite -Url https://tenant.sharepoint.com/sites/contoso -FromRecycleBin
```
This will remove the site collection with the url 'https://tenant.sharepoint.com/sites/contoso' from the recycle bin.
