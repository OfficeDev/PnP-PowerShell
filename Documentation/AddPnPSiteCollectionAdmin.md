# Add-PnPSiteCollectionAdmin
Adds one or more users as site collection administrators to the site collection in the current context
## Syntax
```powershell
Add-PnPSiteCollectionAdmin -Owners <List`1>
```


## Detailed Description
This command allows adding one to many users as site collection administrators to the site collection in the current context. It does not replace or remove exisitng site collection administrators.

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Owners|List`1|True|Specifies owner(s) to add as site collection adminstrators. They will be added as additional site collection administrators to the site in the current context. Existing administrators will stay. Can be both users and groups.|
## Examples

### Example 1
```powershell
PS:> Add-PnPSiteCollectionAdmin -Owners "user@contoso.onmicrosoft.com"
```
This will add user@contoso.onmicrosoft.com as an additional site collection owner to the site collection in the current context

### Example 2
```powershell
PS:> Add-PnPSiteCollectionAdmin -Owners @("user1@contoso.onmicrosoft.com", "user2@contoso.onmicrosoft.com")
```
This will add user1@contoso.onmicrosoft.com and user2@contoso.onmicrosoft.com as additional site collection owners to the site collection in the current context

### Example 3
```powershell
PS:> Get-PnPUser | ? Title -Like "*Doe" | Add-PnPSiteCollectionAdmin
```
This will add all users with their title ending with "Doe" as additional site collection owners to the site collection in the current context
