# Enable-PnPResponsiveUI
Enables the PnP Responsive UI implementation on a classic SharePoint Site
## Syntax
```powershell
Enable-PnPResponsiveUI [-InfrastructureSiteUrl <String>]
                       [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|InfrastructureSiteUrl|String|False|A full URL pointing to an infrastructure site. If specified, it will add a custom action pointing to the responsive UI JS code in that site.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Enable-PnPResponsiveUI
```
Will upload a CSS file, a JavaScript file and adds a custom action to the root web of the current site collection, enabling the responsive UI on the site collection. The CSS and JavaScript files are located in the style library, in a folder called SP.Responsive.UI.
