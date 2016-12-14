#Set-PnPDefaultContentTypeToList
Sets the default content type for a list
##Syntax
```powershell
Set-PnPDefaultContentTypeToList -List <ListPipeBind>
                                -ContentType <ContentTypePipeBind>
                                [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ContentType|ContentTypePipeBind|True|The content type object that needs to be added to the list|
|List|ListPipeBind|True|The name of a content type, its ID or an actual content type object that needs to be removed from the specified list.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Set-PnPDefaultContentTypeToList -List "Project Documents" -ContentType "Project"
```
This will set the Project content type (which has already been added to a list) as the default content type
