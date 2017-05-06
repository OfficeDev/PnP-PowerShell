# Get-PnPFolder
Return a folder object
## Syntax
```powershell
Get-PnPFolder -Url <String>
              [-Web <WebPipeBind>]
              [-Includes <String[]>]
```


## Detailed Description
Retrieves a folder if it exists. Use Ensure-PnPFolder to create the folder if it does not exist.

## Returns
>[Microsoft.SharePoint.Client.Folder](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.file.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Url|String|True|Site or server relative URL of the folder to retrieve. In the case of a server relative url, make sure that the url starts with the managed path as the current web.|
|Includes|String[]|False|Specify properties to include when retrieving objects from the server.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPFolder -RelativeUrl "Shared Documents"
```
Returns the folder called 'Shared Documents' which is located in the root of the current web

### Example 2
```powershell
PS:> Get-PnPFolder -RelativeUrl "/sites/demo/Shared Documents"
```
Returns the folder called 'Shared Documents' which is located in the root of the current web
