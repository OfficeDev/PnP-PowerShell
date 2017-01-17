#Restore-PnPTenantRecycleBinItem
Restores a site collection from the tenant scoped recycle bin
##Syntax
```powershell
Restore-PnPTenantRecycleBinItem -Url <String>
                                [-Wait [<SwitchParameter>]]
                                [-Force [<SwitchParameter>]]
```


##Detailed Description
The Reset-PnPTenantRecycleBinItem cmdlet allows a site collection that has been deleted and still exists in the tenant recycle bin to be restored to its original location.

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|If provided, no confirmation will be asked to restore the site collection from the tenant recycle bin|
|Url|String|True|Url of the site collection to restore from the tenant recycle bin|
|Wait|SwitchParameter|False|If provided, the PowerShell execution will halt until the site restore process has completed|
##Examples

###Example 1
```powershell
PS:> Reset-PnPTenantRecycleBinItem -Url https://tenant.sharepoint.com/sites/contoso
```
This will restore the deleted site collection with the url 'https://tenant.sharepoint.com/sites/contoso' to its original location

###Example 2
```powershell
PS:> Reset-PnPTenantRecycleBinItem -Url https://tenant.sharepoint.com/sites/contoso -Wait
```
This will restore the deleted site collection with the url 'https://tenant.sharepoint.com/sites/contoso' to its original location and will wait with executing further PowerShell commands until the site collection restore has completed
