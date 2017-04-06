# Set-PnPListPermission
Sets list permissions
## Syntax
```powershell
Set-PnPListPermission -Group <GroupPipeBind>
                      -Identity <ListPipeBind>
                      [-AddRole <String>]
                      [-RemoveRole <String>]
                      [-Web <WebPipeBind>]
```


```powershell
Set-PnPListPermission -User <String>
                      -Identity <ListPipeBind>
                      [-AddRole <String>]
                      [-RemoveRole <String>]
                      [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Group|GroupPipeBind|True||
|Identity|ListPipeBind|True|The ID or Title of the list.|
|User|String|True||
|AddRole|String|False|The role that must be assigned to the group or user|
|RemoveRole|String|False|The role that must be removed from the group or user|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Set-PnPListPermission -Identity 'Documents' -User 'user@contoso.com' -AddRole 'Contribute'
```
Adds the 'Contribute' permission to the user 'user@contoso.com' for the list 'Documents'

### Example 2
```powershell
PS:> Set-PnPListPermission -Identity 'Documents' -User 'user@contoso.com' -RemoveRole 'Contribute'
```
Removes the 'Contribute' permission to the user 'user@contoso.com' for the list 'Documents'
