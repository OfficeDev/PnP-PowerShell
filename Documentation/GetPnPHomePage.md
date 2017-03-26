# Get-PnPHomePage
Returns the URL to the home page
## Syntax
```powershell
Get-PnPHomePage [-Web <WebPipeBind>]
```


## Returns
>System.String

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPHomePage
```
Will return the URL of the home page of the web.
