# Remove-PnPUnifiedGroup
Removes one Office 365 Group (aka Unified Group) or a list of Office 365 Groups
## Syntax
```powershell
Remove-PnPUnifiedGroup -Identity <UnifiedGroupPipeBind>
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|UnifiedGroupPipeBind|True|The Identity of the Office 365 Group.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPUnifiedGroup -Identity $groupId
```
Removes an Office 365 Groups based on its ID

### Example 2
```powershell
PS:> Remove-PnPUnifiedGroup -Identity $group
```
Removes the provided Office 365 Groups
