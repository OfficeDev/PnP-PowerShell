# Add-PnPModernPage
Adds a modern page
## Syntax
```powershell
Add-PnPModernPage -NamePage <String>
                  [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|NamePage|String|True|The name page|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPModernPage -PageUrl 'modernpage.aspx' 
```
Creates a new modern page 'modernpage.aspx'
