# Remove-PnPPublishingImageRendition
Removes an existing image rendition
## Syntax
```powershell
Remove-PnPPublishingImageRendition -Identity <ImageRenditionPipeBind>
                                   [-Force [<SwitchParameter>]]
                                   [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ImageRenditionPipeBind|True|The display name or id of the Image Rendition.|
|Force|SwitchParameter|False|If provided, no confirmation will be asked to remove the Image Rendition.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPPublishingImageRendition -Name "MyImageRendition" -Width 800 -Height 600
```

