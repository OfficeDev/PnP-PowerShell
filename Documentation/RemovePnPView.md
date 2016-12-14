#Remove-PnPView
Deletes a view from a list
##Syntax
```powershell
Remove-PnPView [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
               -Identity <ViewPipeBind>
               -List <ListPipeBind>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question.|
|Identity|ViewPipeBind|True|The ID or Title of the view.|
|List|ListPipeBind|True|The ID or Url of the list.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Remove-PnPView -List "Demo List" -Identity "All Items"
```
Removes the view with title "All Items" from the "Demo List" list.
