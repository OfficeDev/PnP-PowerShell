# Get-PnPWebPartXml
Returns the webpart XML of a webpart registered on a site
## Syntax
```powershell
Get-PnPWebPartXml -ServerRelativePageUrl <String>
                  -Identity <WebPartPipeBind>
                  [-Web <WebPipeBind>]
```


## Returns
>System.String

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|WebPartPipeBind|True|Id or title of the webpart. Use Get-PnPWebPart to retrieve all webpart Ids|
|ServerRelativePageUrl|String|True|Full server relative url of the webpart page, e.g. /sites/mysite/sitepages/home.aspx|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPWebPartXml -ServerRelativePageUrl "/sites/demo/sitepages/home.aspx" -Identity a2875399-d6ff-43a0-96da-be6ae5875f82
```
Returns the webpart XML for a given webpart on a page.
