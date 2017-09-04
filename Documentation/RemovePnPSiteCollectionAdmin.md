# Remove-PnPSiteCollectionAdmin
Removes one or more users as site collection administrators from the site collection in the current context
## Syntax
```powershell
Remove-PnPSiteCollectionAdmin -Owners <List`1>
```


## Detailed Description
This command allows removing one to many users as site collection administrators from the site collection in the current context. All existing site collection administrators not included in this command will remain site collection administrator.

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Owners|List`1|True|Specifies owner(s) to remove as site collection adminstrators. Can be both users and groups.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPSiteCollectionAdmin -Owners "user@contoso.onmicrosoft.com"
```
This will remove user@contoso.onmicrosoft.com as a site collection owner from the site collection in the current context

### Example 2
```powershell
PS:> Remove-PnPSiteCollectionAdmin -Owners @("user1@contoso.onmicrosoft.com", "user2@contoso.onmicrosoft.com")
```
This will remove user1@contoso.onmicrosoft.com and user2@contoso.onmicrosoft.com as site collection owners from the site collection in the current context

### Example 3
```powershell
PS:> Get-PnPUser | ? Title -Like "*Doe" | Remove-PnPSiteCollectionAdmin
```
This will remove all users with their title ending with "Doe" as site collection owners from the site collection in the current context

### Example 4
```powershell
PS:> Get-PnPSiteCollectionAdmin | Remove-PnPSiteCollectionAdmin
```
This will remove all existing site collection administrators from the site collection in the current context
