# Add-PnPWebPartToModernPage
Adds a webpart to a Modern Page in a specified zone
## Syntax
```powershell
Add-PnPWebPartToModernPage -ServerRelativePageUrl <String>
                           -NameWebPart <String>
                           -ZoneIndex <Int>
                           [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|NameWebPart|String|True|A name for the webpart.|
|ServerRelativePageUrl|String|True|Server Relative Url of the page to add the webpart to.|
|ZoneIndex|Int|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPWebPartToModernPage -ServerRelativePageUrl "/sites/demo/sitepages/home.aspx" -NameWebPart "HelloWorld"  -ZoneIndex 1 
```

