# Get-PnPWeb
Returns a web object
## Syntax
```powershell
Get-PnPWeb [-Includes <String[]>]
           [-Identity <WebPipeBind>]
```


## Detailed Description
This allows returning a web object representing either the current context its web or one of the webs located underneath it

## Returns
>[Microsoft.SharePoint.Client.Web](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|WebPipeBind|False||
|Includes|String[]|False|Specify properties to include when retrieving objects from the server.|
## Examples

### Example 1
```powershell
PS:> Get-PnPWeb
```
Returns the web of the current context

### Example 2
```powershell
PS:> Get-PnPWeb -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2
```
Returns the current web or a subweb with the Id 'aa66f67e-46c0-4474-8a82-42bf467d07f2'

### Example 3
```powershell
PS:> Get-PnPWeb -Identity "HR"
```
Returns the subsite located at /HR
