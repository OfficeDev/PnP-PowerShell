# Remove-PnPFolder
Deletes a folder within a parent folder
## Syntax
```powershell
Remove-PnPFolder -Name <String>
                 -Folder <String>
                 [-Force [<SwitchParameter>]]
                 [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Folder|String|True|The parent folder in the site|
|Name|String|True|The folder name|
|Force|SwitchParameter|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPFolder -Name NewFolder -Folder _catalogs/masterpage
```
Removes the folder 'NewFolder' from '_catalogsmasterpage'
