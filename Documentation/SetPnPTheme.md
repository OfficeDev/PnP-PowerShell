#Set-PnPTheme
Sets the theme of the current web.
##Syntax
```powershell
Set-PnPTheme [-ColorPaletteUrl <String>]
             [-FontSchemeUrl <String>]
             [-BackgroundImageUrl <String>]
             [-ResetSubwebsToInherit [<SwitchParameter>]]
             [-UpdateRootWebOnly [<SwitchParameter>]]
             [-Web <WebPipeBind>]
```


##Detailed Description
 Sets the theme of the current web, if any of the attributes is not set, that value will be set to null

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|BackgroundImageUrl|String|False|Specifies the Background Image Url based on the server relative url|
|ColorPaletteUrl|String|False|Specifies the Color Palette Url based on the site relative url|
|FontSchemeUrl|String|False|Specifies the Font Scheme Url based on the server relative url|
|ResetSubwebsToInherit|SwitchParameter|False|Resets subwebs to inherit the theme from the rootweb|
|UpdateRootWebOnly|SwitchParameter|False|Updates only the rootweb, even if subwebs are set to inherit the theme.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Set-PnPTheme
```
Removes the current theme and resets it to the default.

###Example 2
```powershell
PS:> Set-PnPTheme -ColorPaletteUrl /_catalogs/theme/15/company.spcolor
```


###Example 3
```powershell
PS:> Set-PnPTheme -ColorPaletteUrl /_catalogs/theme/15/company.spcolor -BackgroundImageUrl '/sites/teamsite/style library/background.png'
```


###Example 4
```powershell
PS:> Set-PnPTheme -ColorPaletteUrl /_catalogs/theme/15/company.spcolor -BackgroundImageUrl '/sites/teamsite/style library/background.png' -ResetSubwebsToInherit
```
Sets the theme to the web, and updates all subwebs to inherit the theme from this web.
