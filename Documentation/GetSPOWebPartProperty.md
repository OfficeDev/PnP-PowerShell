#Get-SPOWebPartProperty
*Topic automatically generated on: 2015-10-13*

Returns a web part property
##Syntax
```powershell
Get-SPOWebPartProperty -ServerRelativePageUrl <String> -Identity <GuidPipeBind> [-Key <String>] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|GuidPipeBind|True||
|Key|String|False||
|ServerRelativePageUrl|String|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-SPOWebPartProperty -ServerRelativePageUrl /sites/demo/sitepages/home.aspx -Identity ccd2c98a-c9ae-483b-ae72-19992d583914
```
Returns all properties of the webpart.

###Example 2
```powershell
PS:> Get-SPOWebPartProperty -ServerRelativePageUrl /sites/demo/sitepages/home.aspx -Identity ccd2c98a-c9ae-483b-ae72-19992d583914 -Key "Title"
```
Returns the title property of the webpart.
