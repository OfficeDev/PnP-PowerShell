# Move-PnPListItemToRecycleBin
Moves an item from a list to the Recycle Bin
## Syntax
```powershell
Move-PnPListItemToRecycleBin -Identity <ListItemPipeBind>
                             -List <ListPipeBind>
                             [-Force [<SwitchParameter>]]
                             [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ListItemPipeBind|True|The ID of the listitem, or actual ListItem object|
|List|ListPipeBind|True|The ID, Title or Url of the list.|
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Move-PnPListItemToRecycleBin -List "Demo List" -Identity "1" -Force
```
Moves the listitem with id "1" from the "Demo List" list to the Recycle Bin.
