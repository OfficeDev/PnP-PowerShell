# Remove-PnPGroup
Removes a group from a web.
## Syntax
```powershell
Remove-PnPGroup [-Force [<SwitchParameter>]]
                [-Web <WebPipeBind>]
                [-Identity <GroupPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question.|
|Identity|GroupPipeBind|False|A group object, an ID or a name of a group to remove|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPGroup -Identity "My Users"
```
Removes the group "My Users"
