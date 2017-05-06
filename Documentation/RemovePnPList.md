# Remove-PnPList
Deletes a list
## Syntax
```powershell
Remove-PnPList -Identity <ListPipeBind>
               [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ListPipeBind|True|The ID or Title of the list.|
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPList -Title Announcements
```
Removes the list named 'Announcements'. Asks for confirmation.

### Example 2
```powershell
PS:> Remove-PnPList -Title Announcements -Force
```
Removes the list named 'Announcements' without asking for confirmation.
