# Get-PnPPublishingImageRendition
Returns all image renditions or if Identity is specified a specific one
## Syntax
```powershell
Get-PnPPublishingImageRendition [-Web <WebPipeBind>]
                                [-Identity <ImageRenditionPipeBind>]
```


## Returns
>[Microsoft.SharePoint.Client.Publishing.ImageRendition](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.publishing.imagerendition.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ImageRenditionPipeBind|False|Id or name of an existing image rendition|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPPublishingImageRendition
```
Returns all Image Renditions

### Example 2
```powershell
PS:> Get-PnPPublishingImageRendition -Identity "Test"
```
Returns the image rendition named "Test"

### Example 3
```powershell
PS:> Get-PnPPublishingImageRendition -Identity 2
```
Returns the image rendition where its id equals 2
