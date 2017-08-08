# Set-PnPSiteLogo
Sets the Site Logo of the current web.
## Syntax
```powershell
Set-PnPSiteLogo -RootFolderRelativeUrl <String>
                [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|RootFolderRelativeUrl|String|True|The root folder relative url of the homepage, e.g. 'sitepages/home.aspx'|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Set-PnPSiteLogo -RootFolderRelativeUrl SitePages/Home.aspx
```
Sets the home page to the home.aspx file which resides in the SitePages library
