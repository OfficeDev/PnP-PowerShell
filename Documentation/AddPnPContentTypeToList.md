#Add-PnPContentTypeToList
Adds a new content type to a list
##Syntax
```powershell
Add-PnPContentTypeToList -List <ListPipeBind>
                         -ContentType <ContentTypePipeBind>
                         [-DefaultContentType [<SwitchParameter>]]
                         [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ContentType|ContentTypePipeBind|True|Specifies the content type that needs to be added to the list|
|DefaultContentType|SwitchParameter|False|Specify if the content type needs to be the default content type or not|
|List|ListPipeBind|True|Specifies the list the content type needs to be added to|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Add-PnPContentTypeToList -List "Documents" -ContentType "Project Document" -DefaultContentType
```
This will add an existing content type to a list and sets it as the default content type
