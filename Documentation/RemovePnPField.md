#Remove-PnPField
Removes a field from a list or a site
##Syntax
```powershell
Remove-PnPField [-Force [<SwitchParameter>]]
                [-Web <WebPipeBind>]
                -Identity <FieldPipeBind>
                [-List <ListPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question.|
|Identity|FieldPipeBind|True|The field object or name to remove|
|List|ListPipeBind|False|The list object or name where to remove the field from|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Remove-PnPField -Identity "Speakers"
```
Removes the speakers field from the site columns

###Example 2
```powershell
PS:> Remove-PnPField -List "Demo list" -Identity "Speakers"
```
Removes the speakers field from the list Demo list
