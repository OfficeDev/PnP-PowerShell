#Remove-PnPTenantSite
Office365 only: Removes a site collection from the current tenant
##Syntax
```powershell
Remove-PnPTenantSite [-SkipRecycleBin [<SwitchParameter>]]
                     [-FromRecycleBin [<SwitchParameter>]]
                     [-Force [<SwitchParameter>]]
                     -Url <String>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Do not ask for confirmation.|
|FromRecycleBin|SwitchParameter|False|If specified, will search for the site in the Recycle Bin and remove it from there.|
|SkipRecycleBin|SwitchParameter|False|Do not add to the trashcan when selected.|
|Url|String|True|Specifies the full URL of the site collection that needs to be deleted|
##Examples

###Example 1
```powershell
PS:> Remove-PnPTenantSite -Url https://tenant.sharepoint.com/sites/contoso
```
This will remove the site collection with the url 'https://tenant.sharepoint.com/sites/contoso'  and put it in the recycle bin.

###Example 2
```powershell
PS:> Remove-PnPTenantSite -Url https://tenant.sharepoint.com/sites/contoso -Force -SkipRecycleBin
```
This will remove the site collection with the url 'https://tenant.sharepoint.com/sites/contoso' with force and it will skip the recycle bin.

###Example 3
```powershell
PS:> Remove-PnPTenantSite -Url https://tenant.sharepoint.com/sites/contoso -FromRecycleBin
```
This will remove the site collection with the url 'https://tenant.sharepoint.com/sites/contoso' from the recycle bin.
