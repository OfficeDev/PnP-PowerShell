# Get-PnPSite
Returns the current site collection from the context
## Syntax
```powershell
Get-PnPSite [-Includes <String[]>]
```


## Detailed Description
This command returns the site collection of the current context. If you wish to return another site collection, use Connect-PnPOnline -Url <other site collection> to connect to it first.

## Returns
>[Microsoft.SharePoint.Client.Site](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.site.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Includes|String[]|False|Specify properties to include when retrieving objects from the server.|
## Examples

### Example 1
```powershell
PS:> Get-PnPSite
```
Returns the site collection of the current context
