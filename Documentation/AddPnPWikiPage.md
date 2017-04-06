# Add-PnPWikiPage
Adds a wiki page
## Syntax
```powershell
Add-PnPWikiPage -Content <String>
                -ServerRelativePageUrl <String>
                [-Web <WebPipeBind>]
```


```powershell
Add-PnPWikiPage -Layout <WikiPageLayout>
                -ServerRelativePageUrl <String>
                [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Content|String|True||
|Layout|WikiPageLayout|True||
|ServerRelativePageUrl|String|True|The server relative page URL|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPWikiPage -PageUrl '/sites/demo1/pages/wikipage.aspx' -Content 'New WikiPage'
```
Creates a new wiki page '/sites/demo1/pages/wikipage.aspx' and sets the content to 'New WikiPage'
