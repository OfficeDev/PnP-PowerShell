#Disable-PnPResponsiveUI
Disables the PnP Responsive UI implementation on a classic SharePoint Site
##Syntax
```powershell
Disable-PnPResponsiveUI [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Disable-PnPResponsiveUI
```
If enabled previously, this will remove the PnP Responsive UI from a site.
