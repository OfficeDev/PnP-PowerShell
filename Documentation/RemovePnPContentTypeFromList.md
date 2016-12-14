#Remove-PnPContentTypeFromList
Removes a content type from a list
##Syntax
```powershell
Remove-PnPContentTypeFromList -List <ListPipeBind>
                              -ContentType <ContentTypePipeBind>
                              [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ContentType|ContentTypePipeBind|True|The name of a content type, its ID or an actual content type object that needs to be removed from the specified list.|
|List|ListPipeBind|True|The name of the list, its ID or an actual list object from where the content type needs to be removed from|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Remove-PnPContentTypeFromList -List "Documents" -ContentType "Project Document"
```
This will remove a content type called "Project Document" from the "Documents" list
