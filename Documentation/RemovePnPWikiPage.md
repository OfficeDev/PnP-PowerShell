# Remove-PnPWikiPage
Removes a wiki page
## Syntax
```powershell
Remove-PnPWikiPage -ServerRelativePageUrl <String>
                   [-Web <WebPipeBind>]
```


```powershell
Remove-PnPWikiPage -SiteRelativePageUrl <String>
                   [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ServerRelativePageUrl|String|True||
|SiteRelativePageUrl|String|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPWikiPage -PageUrl '/pages/wikipage.aspx'
```
Removes the page '/pages/wikipage.aspx'
