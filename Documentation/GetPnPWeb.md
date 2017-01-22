#Get-PnPWeb
Returns the current web object
##Syntax
```powershell
Get-PnPWeb [-Includes <String[]>]
           [-Identity <WebPipeBind>]
```


##Returns
>[Microsoft.SharePoint.Client.Web](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|WebPipeBind|False||
|Includes|String[]|False|Specify properties to include when retrieving objects from the server.|
