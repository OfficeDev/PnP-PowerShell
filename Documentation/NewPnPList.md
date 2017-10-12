# New-PnPList
Creates a new list
## Syntax
```powershell
New-PnPList -Title <String>
            -Template <ListTemplateType>
            [-Url <String>]
            [-EnableVersioning [<SwitchParameter>]]
            [-EnableContentTypes [<SwitchParameter>]]
            [-OnQuickLaunch [<SwitchParameter>]]
            [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Template|ListTemplateType|True|The type of list to create.|
|Title|String|True|The Title of the list|
|EnableContentTypes|SwitchParameter|False|Switch parameter if content types should be enabled on this list|
|EnableVersioning|SwitchParameter|False|Switch parameter if versioning should be enabled|
|OnQuickLaunch|SwitchParameter|False|Switch parameter if this list should be visible on the QuickLaunch|
|Url|String|False|If set, will override the url of the list.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> New-PnPList -Title Announcements -Template Announcements
```
Create a new announcements list

### Example 2
```powershell
PS:> New-PnPList -Title "Demo List" -Url "DemoList" -Template Announcements
```
Create a list with a title that is different from the url

## ListTemplateType options
Common options
- Announcements
- DocumentLibrary
- GenericList

Full list available from 
https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.listtemplatetype.aspx
