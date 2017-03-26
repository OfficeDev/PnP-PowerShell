# Add-PnPUserToGroup
Adds a user to a group
## Syntax
```powershell
Add-PnPUserToGroup -LoginName <String>
                   -Identity <GroupPipeBind>
                   [-Web <WebPipeBind>]
```


```powershell
Add-PnPUserToGroup -Identity <GroupPipeBind>
                   -EmailAddress <String>
                   [-SendEmail [<SwitchParameter>]]
                   [-EmailBody <String>]
                   [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|EmailAddress|String|True|The email address of the user|
|Identity|GroupPipeBind|True|The group id, group name or group object to add the user to.|
|LoginName|String|True|The login name of the user|
|EmailBody|String|False||
|SendEmail|SwitchParameter|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPUserToGroup -LoginName user@company.com -Identity 'Marketing Site Members'
```
Add the specified user to the group "Marketing Site Members"

### Example 2
```powershell
PS:> Add-PnPUserToGroup -LoginName user@company.com -Identity 5
```
Add the specified user to the group with Id 5
