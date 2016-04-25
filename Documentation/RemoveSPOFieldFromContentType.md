#Remove-SPOFieldFromContentType
Removes a site column from a content type
##Syntax
```powershell
Remove-SPOFieldFromContentType -Field <FieldPipeBind> -ContentType <ContentTypePipeBind> [-DoNotUpdateChildren [<SwitchParameter>]] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ContentType|ContentTypePipeBind|True|The content type where the field is to be removed from.|
|DoNotUpdateChildren|SwitchParameter|False|If specified, inherited content types will not be updated.|
|Field|FieldPipeBind|True|The field to remove.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Remove-SPOFieldFromContentType -Field "Project_Name" -ContentType "Project Document"
```
This will remove the site column with an internal name of "Project_Name" from a content type called "Project Document"

###Example 2
```powershell
PS:> Remove-SPOFieldFromContentType -Field "Project_Name" -ContentType "Project Document" -DoNotUpdateChildren
```
This will remove the site column with an internal name of "Project_Name" from a content type called "Project Document". It will not update content types that inherit from the "Project Document" content type.
