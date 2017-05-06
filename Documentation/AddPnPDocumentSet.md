# Add-PnPDocumentSet
Creates a new document set in a library.
## Syntax
```powershell
Add-PnPDocumentSet -List <ListPipeBind>
                   -Name <String>
                   -ContentType <ContentTypePipeBind>
                   [-Web <WebPipeBind>]
```


## Returns
>System.String

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ContentType|ContentTypePipeBind|True|The name of the content type, its ID or an actual content object referencing to the document set.|
|List|ListPipeBind|True|The name of the list, its ID or an actual list object from where the document set needs to be added|
|Name|String|True|The name of the document set|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPDocumentSet -List "Documents" -ContentType "Test Document Set" -Name "Test"
```
This will add a new document set based upon the 'Test Document Set' content type to a list called 'Documents'. The document set will be named 'Test'
