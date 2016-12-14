#Remove-PnPContentType
Removes a content type from a web
##Syntax
```powershell
Remove-PnPContentType [-Force [<SwitchParameter>]]
                      [-Web <WebPipeBind>]
                      -Identity <ContentTypePipeBind>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question.|
|Identity|ContentTypePipeBind|True|The name or ID of the content type to remove|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Remove-PnPContentType -Identity "Project Document"
```
This will remove a content type called "Project Document" from the current web

###Example 2
```powershell
PS:> Remove-PnPContentType -Identity "Project Document" -Force
```
This will remove a content type called "Project Document" from the current web with force
