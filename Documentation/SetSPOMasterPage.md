#Set-SPOMasterPage
Sets the default master page of the current web.
##Syntax
```powershell
Set-SPOMasterPage [-MasterPageServerRelativeUrl <String>] [-CustomMasterPageServerRelativeUrl <String>] [-Web <WebPipeBind>]
```


```powershell
Set-SPOMasterPage [-MasterPageSiteRelativeUrl <String>] [-CustomMasterPageSiteRelativeUrl <String>] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|CustomMasterPageServerRelativeUrl|String|False||
|CustomMasterPageSiteRelativeUrl|String|False||
|MasterPageServerRelativeUrl|String|False||
|MasterPageSiteRelativeUrl|String|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Set-SPOMasterPage -MasterPageServerRelativeUrl /sites/projects/_catalogs/masterpage/oslo.master
```
Sets the master page based on a server relative URL

###Example 2
```powershell
PS:> Set-SPOMasterPage -MasterPageSiteRelativeUrl _catalogs/masterpage/oslo.master
```
Sets the master page based on a site relative URL
