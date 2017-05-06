# Add-PnPHtmlPublishingPageLayout
Adds a HTML based publishing page layout
## Syntax
```powershell
Add-PnPHtmlPublishingPageLayout -SourceFilePath <String>
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
|DestinationFolderHierarchy|String|False|Folder hierarchy where the HTML page layouts will be deployed|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPHtmlPublishingPageLayout -Title 'Our custom page layout' -SourceFilePath 'customlayout.aspx' -Description 'A custom page layout' -AssociatedContentTypeID 0x01010901
```
Uploads the pagelayout 'customlayout.aspx' from the current location to the current site as a 'web part page' pagelayout
