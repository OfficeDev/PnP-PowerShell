# Get-PnPUser
Returns site users of current web
## Syntax
```powershell
Get-PnPUser [-Web <WebPipeBind>]
            [-Identity <UserPipeBind>]
```


## Detailed Description
This command will return all the users that exist in the current site collection its User Information List

## Returns
>[Microsoft.SharePoint.Client.User](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.user.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|UserPipeBind|False|User ID or login name|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPUser
```
Returns all users from the User Information List of the current site collection

### Example 2
```powershell
PS:> Get-PnPUser -Identity 23
```
Returns the user with Id 23 from the User Information List of the current site collection

### Example 3
```powershell
PS:> Get-PnPUser -Identity i:0#.f|membership|user@tenant.onmicrosoft.com
```
Returns the user with LoginName i:0#.f|membership|user@tenant.onmicrosoft.com from the User Information List of the current site collection

### Example 4
```powershell
PS:> Get-PnPUser | ? Email -eq "user@tenant.onmicrosoft.com"
```
Returns the user with e-mail address user@tenant.onmicrosoft.com from the User Information List of the current site collection
