# Get-PnPSubWebs
Returns the subwebs of the current web
## Syntax
```powershell
Get-PnPSubWebs [-Recurse [<SwitchParameter>]]
               [-Web <WebPipeBind>]
               [-Identity <WebPipeBind>]
```


## Returns
>[List<Microsoft.SharePoint.Client.Web>](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|WebPipeBind|False||
|Recurse|SwitchParameter|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
