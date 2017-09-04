# Remove-PnPFile
Removes a file.
## Syntax
```powershell
Remove-PnPFile -ServerRelativeUrl <String>
               [-Recycle [<SwitchParameter>]]
               [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
```


```powershell
Remove-PnPFile -SiteRelativeUrl <String>
               [-Recycle [<SwitchParameter>]]
               [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ServerRelativeUrl|String|True|Server relative URL to the file|
|SiteRelativeUrl|String|True|Site relative URL to the file|
|Force|SwitchParameter|False||
|Recycle|SwitchParameter|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:>Remove-PnPFile -ServerRelativeUrl /sites/project/_catalogs/themes/15/company.spcolor
```
Removes the file company.spcolor

### Example 2
```powershell
PS:>Remove-PnPFile -SiteRelativeUrl _catalogs/themes/15/company.spcolor
```
Removes the file company.spcolor

### Example 3
```powershell
PS:>Remove-PnPFile -SiteRelativeUrl _catalogs/themes/15/company.spcolor -Recycle
```
Removes the file company.spcolor and saves it to the Recycle Bin
