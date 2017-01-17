#Move-PnPFolder
Move a folder to another location in the current web
##Syntax
```powershell
Move-PnPFolder -Folder <String>
               -TargetFolder <String>
               [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Folder|String|True|The folder to move|
|TargetFolder|String|True|The new parent location to which the folder should be moved to|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Move-PnPFolder -Folder Documents/Reports -TargetLocation 'Archived Reports'
```
This will move the folder Reports in the Documents library to the 'Archived Reports' library

###Example 2
```powershell
PS:> Move-PnPFolder -Folder 'Shared Documents/Reports/2016/Templates' -TargetLocation 'Shared Documents/Reports'
```
This will move the folder Templates to the new location in 'Shared Documents/Reports'
