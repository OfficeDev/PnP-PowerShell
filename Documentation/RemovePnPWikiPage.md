#Remove-PnPWikiPage
Removes a wiki page
##Syntax
```powershell
Remove-PnPWikiPage [-Web <WebPipeBind>]
                   -ServerRelativePageUrl <String>
```


```powershell
Remove-PnPWikiPage [-Web <WebPipeBind>]
                   -SiteRelativePageUrl <String>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ServerRelativePageUrl|String|True||
|SiteRelativePageUrl|String|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Remove-PnPWikiPage -PageUrl '/pages/wikipage.aspx'
```
Removes the page '/pages/wikipage.aspx'
