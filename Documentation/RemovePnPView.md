# Remove-PnPView
Deletes a view from a list
## Syntax
```powershell
Remove-PnPView -Identity <ViewPipeBind>
               -List <ListPipeBind>
               [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ViewPipeBind|True|The ID or Title of the view.|
|List|ListPipeBind|True|The ID or Url of the list.|
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPView -List "Demo List" -Identity "All Items"
```
Removes the view with title "All Items" from the "Demo List" list.
