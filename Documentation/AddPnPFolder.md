# Add-PnPFolder
Creates a folder within a parent folder
## Syntax
```powershell
Add-PnPFolder -Name <String>
              -Folder <String>
              [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Folder|String|True|The parent folder in the site|
|Name|String|True|The folder name|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPFolder -Name NewFolder -Folder _catalogs/masterpage
```
This will create the folder NewFolder in the masterpage catalog
