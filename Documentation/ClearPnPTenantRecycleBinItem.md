# Clear-PnPTenantRecycleBinItem
Permanently deletes a site collection from the tenant scoped recycle bin
## Syntax
```powershell
Clear-PnPTenantRecycleBinItem -Url <String>
                              [-Wait [<SwitchParameter>]]
                              [-Force [<SwitchParameter>]]
```


## Detailed Description
The Clear-PnPTenantRecycleBinItem cmdlet allows a site collection that has been deleted and still exists in the tenant recycle bin to be permanently deleted from the recycle bin as well.

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Url|String|True|Url of the site collection to permanently delete from the tenant recycle bin|
|Force|SwitchParameter|False|If provided, no confirmation will be asked to permanently delete the site collection from the tenant recycle bin|
|Wait|SwitchParameter|False|If provided, the PowerShell execution will halt until the operation has completed|
## Examples

### Example 1
```powershell
PS:> Clear-PnPTenantRecycleBinItem -Url https://tenant.sharepoint.com/sites/contoso
```
This will permanently delete site collection with the url 'https://tenant.sharepoint.com/sites/contoso' from the tenant recycle bin

### Example 2
```powershell
PS:> Clear-PnPTenantRecycleBinItem -Url https://tenant.sharepoint.com/sites/contoso -Wait
```
This will permanently delete site collection with the url 'https://tenant.sharepoint.com/sites/contoso' from the tenant recycle bin and will wait with executing further PowerShell commands until the operation has completed
