#Remove-PnPFile
Removes a file.
##Syntax
```powershell
Remove-PnPFile [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
               -ServerRelativeUrl <String>
```


```powershell
Remove-PnPFile [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
               -SiteRelativeUrl <String>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False||
|ServerRelativeUrl|String|True|Server relative URL to the file|
|SiteRelativeUrl|String|True|Site relative URL to the file|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:>Remove-PnPFile -ServerRelativeUrl /sites/project/_catalogs/themes/15/company.spcolor
```
Removes the file company.spcolor

###Example 2
```powershell
PS:>Remove-PnPFile -SiteRelativeUrl _catalogs/themes/15/company.spcolor
```
Removes the file company.spcolor
