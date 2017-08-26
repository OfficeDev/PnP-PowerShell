# Remove-PnPUser
Removes a specific user from the site collection User Information List
## Syntax
```powershell
Remove-PnPUser -Identity <UserPipeBind>
               [-Force [<SwitchParameter>]]
               [-Confirm [<SwitchParameter>]]
               [-Web <WebPipeBind>]
```


## Detailed Description
This command will allow the removal of a specific user from the User Information List

## Returns
>[Microsoft.SharePoint.Client.User](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.user.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|UserPipeBind|True|User ID or login name|
|Confirm|SwitchParameter|False|Specifying the Confirm parameter will allow the confirmation question to be skipped|
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPUser -Identity 23
```
Remove the user with Id 23 from the User Information List of the current site collection

### Example 2
```powershell
PS:> Remove-PnPUser -Identity i:0#.f|membership|user@tenant.onmicrosoft.com
```
Remove the user with LoginName i:0#.f|membership|user@tenant.onmicrosoft.com from the User Information List of the current site collection

### Example 3
```powershell
PS:> Get-PnPUser | ? Email -eq "user@tenant.onmicrosoft.com" | Remove-PnPUser
```
Remove the user with e-mail address user@tenant.onmicrosoft.com from the User Information List of the current site collection

### Example 4
```powershell
PS:> Remove-PnPUser -Identity i:0#.f|membership|user@tenant.onmicrosoft.com -Confirm:$false
```
Remove the user with LoginName i:0#.f|membership|user@tenant.onmicrosoft.com from the User Information List of the current site collection without asking to confirm the removal first
