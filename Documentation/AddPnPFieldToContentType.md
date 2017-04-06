# Add-PnPFieldToContentType
Adds an existing site column to a content type
## Syntax
```powershell
Add-PnPFieldToContentType -Field <FieldPipeBind>
                          -ContentType <ContentTypePipeBind>
                          [-Required [<SwitchParameter>]]
                          [-Hidden [<SwitchParameter>]]
                          [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ContentType|ContentTypePipeBind|True|Specifies which content type a field needs to be added to|
|Field|FieldPipeBind|True|Specifies the field that needs to be added to the content type|
|Hidden|SwitchParameter|False|Specifies whether the field should be hidden or not|
|Required|SwitchParameter|False|Specifies whether the field is required or not|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPFieldToContentType -Field "Project_Name" -ContentType "Project Document"
```
This will add an existing site column with an internal name of "Project_Name" to a content type called "Project Document"
