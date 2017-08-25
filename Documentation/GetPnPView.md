# Get-PnPView
Returns one or all views from a list
## Syntax
```powershell
Get-PnPView -List <ListPipeBind>
            [-Identity <ViewPipeBind>]
            [-Web <WebPipeBind>]
            [-Includes <String[]>]
```


## Returns
>[Microsoft.SharePoint.Client.View](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.view.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|List|ListPipeBind|True|The Id, Title or Url of the list|
|Identity|ViewPipeBind|False|The Id, Title or instance of the view|
|Includes|String[]|False|Specify properties to include when retrieving objects from the server.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
Get-PnPView -List "Demo List"
```
Returns all views associated from the list titled "Demo List"

### Example 2
```powershell
Get-PnPView -List "Demo List" -Identity "Demo View"
```
Returns the view called "Demo View" from the list titled "Demo List"

### Example 3
```powershell
Get-PnPView -List "Demo List" -Identity "5275148a-6c6c-43d8-999a-d2186989a661"
```
Returns the view with the Id "5275148a-6c6c-43d8-999a-d2186989a661" from the list titled "Demo List"

### Example 4
```powershell
Get-PnPList -Identity "Demo List" | Get-PnPView -Identity "Demo View"
```
Returns the view called "Demo View" from the list titled "Demo List"
