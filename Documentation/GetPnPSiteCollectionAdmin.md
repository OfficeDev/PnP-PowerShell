# Get-PnPSiteCollectionAdmin
Returns the current site collection administrators of the site colleciton in the current context
## Syntax
```powershell
Get-PnPSiteCollectionAdmin [-Web <WebPipeBind>]
```


## Detailed Description
This command will return all current site collection administrators of the site collection in the current context

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPSiteCollectionAdmin
```
This will return all the current site collection administrators of the site collection in the current context
