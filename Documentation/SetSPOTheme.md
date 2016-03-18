#Set-SPOTheme
Sets the theme of the current web.
##Syntax
```powershell
Set-SPOTheme [-ColorPaletteUrl <String>] [-FontSchemeUrl <String>] [-BackgroundImageUrl <String>] [-ShareGenerated [<SwitchParameter>]] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|BackgroundImageUrl|String|False||
|ColorPaletteUrl|String|False||
|FontSchemeUrl|String|False||
|ShareGenerated|SwitchParameter|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Set-SPOTheme -ColorPaletteUrl /_catalogs/theme/15/company.spcolor
```

