# et-PnPWeb
Sets properties on a web
## Syntax
```powershell
Set-PnPWeb [-SiteLogoUrl <String>]
           [-AlternateCssUrl <String>]
           [-Title <String>]
           [-Description <String>]
           [-MasterUrl <String>]
           [-CustomMasterUrl <String>]
           [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AlternateCssUrl|String|False||
|CustomMasterUrl|String|False||
|Description|String|False||
|MasterUrl|String|False||
|SiteLogoUrl|String|False||
|Title|String|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
