# Add-PnPPublishingPageLayout
Adds a publishing page layout
## Syntax
```powershell
Add-PnPPublishingPageLayout -SourceFilePath <String>
                            -Title <String>
                            -Description <String>
                            -AssociatedContentTypeID <String>
                            [-DestinationFolderHierarchy <String>]
                            [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AssociatedContentTypeID|String|True|Associated content type ID|
|Description|String|True|Description for the page layout|
|SourceFilePath|String|True|Path to the file which will be uploaded|
|Title|String|True|Title for the page layout|
|DestinationFolderHierarchy|String|False|Folder hierarchy where the html page layouts will be deployed|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPPublishingPageLayout -Title 'Our custom page layout' -SourceFilePath 'customlayout.aspx' -Description 'A custom page layout' -AssociatedContentTypeID 0x01010901
```
Uploads the pagelayout 'customlayout.aspx' to the current site as a 'web part page' pagelayout
