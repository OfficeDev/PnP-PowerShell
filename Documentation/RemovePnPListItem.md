#Remove-PnPListItem
Deletes an item from a list
##Syntax
```powershell
Remove-PnPListItem -Identity <ListItemPipeBind>
                   [-Force [<SwitchParameter>]]
                   [-Web <WebPipeBind>]
                   -List <ListPipeBind>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question.|
|Identity|ListItemPipeBind|True|The ID of the listitem, or actual ListItem object|
|List|ListPipeBind|True|The ID, Title or Url of the list.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Remove-PnPListItem -List "Demo List" -Identity "1" -Force
```
Removes the listitem with id "1" from the "Demo List" list.
