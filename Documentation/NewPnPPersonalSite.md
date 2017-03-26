# New-PnPPersonalSite
Office365 only: Creates a personal / OneDrive For Business site
## Syntax
```powershell
New-PnPPersonalSite -Email <String[]>
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Email|String[]|True|The UserPrincipalName (UPN) of the users|
## Examples

### Example 1
```powershell
PS:> $users = ('katiej@contoso.onmicrosoft.com','garth@contoso.onmicrosoft.com')
                 PS:> New-PnPPersonalSite -Email $users
```
Creates a personal / OneDrive For Business site for the 2 users in the variable $users
