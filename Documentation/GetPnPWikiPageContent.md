# Get-PnPWikiPageContent
Gets the contents/source of a wiki page
## Syntax
```powershell
Get-PnPWikiPageContent -ServerRelativePageUrl <String>
                       [-Web <WebPipeBind>]
```


## Returns
>System.String

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ServerRelativePageUrl|String|True|The server relative URL for the wiki page|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPWikiPageContent -PageUrl '/sites/demo1/pages/wikipage.aspx'
```
Gets the content of the page '/sites/demo1/pages/wikipage.aspx'
