#Add-PnPPublishingImageRendition
Adds an Image Rendition if the Name of the Image Rendition does not already exist. This prevents creating two Image Renditions that share the same name.
##Syntax
```powershell
Add-PnPPublishingImageRendition -Name <String>
                                -Width <Int32>
                                -Height <Int32>
                                [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Height|Int32|True|The height of the Image Rendition.|
|Name|String|True|The display name of the Image Rendition.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
|Width|Int32|True|The width of the Image Rendition.|
##Examples

###Example 1
```powershell
PS:> Add-PnPPublishingImageRendition -Name "MyImageRendition" -Width 800 -Height 600
```

