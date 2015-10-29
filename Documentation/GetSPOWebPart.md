#Get-SPOWebPart
*Topic automatically generated on: 2015-10-13*

Returns a webpart definition object
##Syntax
```powershell
Get-SPOWebPart -ServerRelativePageUrl <String> [-Identity <WebPartPipeBind>] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|WebPartPipeBind|False||
|ServerRelativePageUrl|String|True|Full server relative url of the webpart page, e.g. /sites/mysite/sitepages/home.aspx|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-SPOWebPart -ServerRelativePageUrl "/sites/demo/sitepages/home.aspx"
```
Returns all webparts defined on the given page.

###Example 2
```powershell
PS:> Get-SPOWebPart -ServerRelativePageUrl "/sites/demo/sitepages/home.aspx" -Identity a2875399-d6ff-43a0-96da-be6ae5875f82
```
Returns a specific webpart defined on the given page.
