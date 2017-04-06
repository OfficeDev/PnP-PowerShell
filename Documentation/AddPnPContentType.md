# Add-PnPContentType
Adds a new content type
## Syntax
```powershell
Add-PnPContentType -Name <String>
                   [-ContentTypeId <String>]
                   [-Description <String>]
                   [-Group <String>]
                   [-ParentContentType <ContentType>]
                   [-Web <WebPipeBind>]
```


## Returns
>[Microsoft.SharePoint.Client.ContentType](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.contenttype.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Name|String|True|Specify the name of the new content type|
|ContentTypeId|String|False|If specified, in the format of 0x0100233af432334r434343f32f3, will create a content type with the specific ID|
|Description|String|False|Specifies the description of the new content type|
|Group|String|False|Specifies the group of the new content type|
|ParentContentType|ContentType|False|Specifies the parent of the new content type|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPContentType -Name "Project Document" -Description "Use for Contoso projects" -Group "Contoso Content Types" -ParentContentType $ct
```
This will add a new content type based on the parent content type stored in the $ct variable.
