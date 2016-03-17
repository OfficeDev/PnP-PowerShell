#Remove-SPOWikiPage
Removes a wiki page
##Syntax
```powershell
Remove-SPOWikiPage [-Web <WebPipeBind>] -ServerRelativePageUrl <String>
```


```powershell
Remove-SPOWikiPage [-Web <WebPipeBind>] -SiteRelativePageUrl <String>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ServerRelativePageUrl|String|True||
|SiteRelativePageUrl|String|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
