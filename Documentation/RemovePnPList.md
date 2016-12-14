#Remove-PnPList
Deletes a list
##Syntax
```powershell
Remove-PnPList [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
               -Identity <ListPipeBind>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question.|
|Identity|ListPipeBind|True|The ID or Title of the list.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Remove-PnPList -Title Announcements
```
Removes the list named 'Announcements'. Asks for confirmation.

###Example 2
```powershell
PS:> Remove-PnPList -Title Announcements -Force
```
Removes the list named 'Announcements' without asking for confirmation.
