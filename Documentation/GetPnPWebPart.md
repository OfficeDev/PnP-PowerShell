#Get-PnPWebPart
Returns a webpart definition object
##Syntax
```powershell
Get-PnPWebPart -ServerRelativePageUrl <String>
               [-Identity <WebPartPipeBind>]
               [-Web <WebPipeBind>]
```


##Returns
>[System.Collections.Generic.IEnumerable`1[Microsoft.SharePoint.Client.WebParts.WebPartDefinition]](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.webparts.webpartdefinition.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|WebPartPipeBind|False||
|ServerRelativePageUrl|String|True|Full server relative URL of the webpart page, e.g. /sites/mysite/sitepages/home.aspx|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-PnPWebPart -ServerRelativePageUrl "/sites/demo/sitepages/home.aspx"
```
Returns all webparts defined on the given page.

###Example 2
```powershell
PS:> Get-PnPWebPart -ServerRelativePageUrl "/sites/demo/sitepages/home.aspx" -Identity a2875399-d6ff-43a0-96da-be6ae5875f82
```
Returns a specific webpart defined on the given page.
