# Add-PnPPublishingImageRendition
Adds an Image Rendition if the Name of the Image Rendition does not already exist. This prevents creating two Image Renditions that share the same name.
## Syntax
```powershell
Add-PnPPublishingImageRendition -Name <String>
                                -Width <Int>
                                -Height <Int>
                                [-Web <WebPipeBind>]
```

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Height|Int|True|The height of the Image Rendition.|
|Name|String|True|The display name of the Image Rendition.|
|Width|Int|True|The width of the Image Rendition.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPPublishingImageRendition -Name "MyImageRendition" -Width 800 -Height 600
```

