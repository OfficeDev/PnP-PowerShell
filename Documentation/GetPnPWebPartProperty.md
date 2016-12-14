#Get-PnPWebPartProperty
Returns a web part property
##Syntax
```powershell
Get-PnPWebPartProperty -ServerRelativePageUrl <String>
                       -Identity <GuidPipeBind>
                       [-Key <String>]
                       [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|GuidPipeBind|True|The id of the webpart|
|Key|String|False|Name of a single property to be returned|
|ServerRelativePageUrl|String|True|Full server relative URL of the webpart page, e.g. /sites/mysite/sitepages/home.aspx|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-PnPWebPartProperty -ServerRelativePageUrl /sites/demo/sitepages/home.aspx -Identity ccd2c98a-c9ae-483b-ae72-19992d583914
```
Returns all properties of the webpart.

###Example 2
```powershell
PS:> Get-PnPWebPartProperty -ServerRelativePageUrl /sites/demo/sitepages/home.aspx -Identity ccd2c98a-c9ae-483b-ae72-19992d583914 -Key "Title"
```
Returns the title property of the webpart.
