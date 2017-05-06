# New-PnPWeb
Creates a new subweb under the current web
## Syntax
```powershell
New-PnPWeb -Title <String>
           -Url <String>
           -Template <String>
           [-Description <String>]
           [-Locale <Int>]
           [-BreakInheritance [<SwitchParameter>]]
           [-InheritNavigation [<SwitchParameter>]]
           [-Web <WebPipeBind>]
```


## Returns
>[Microsoft.SharePoint.Client.Web](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Template|String|True|The site definition template to use for the new web, e.g. STS#0. Use Get-PnPWebTemplates to fetch a list of available templates|
|Title|String|True|The title of the new web|
|Url|String|True|The URL of the new web|
|BreakInheritance|SwitchParameter|False|By default the subweb will inherit its security from its parent, specify this switch to break this inheritance|
|Description|String|False|The description of the new web|
|InheritNavigation|SwitchParameter|False|Specifies whether the site inherits navigation.|
|Locale|Int|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> New-PnPWeb -Title "Project A Web" -Url projectA -Description "Information about Project A" -Locale 1033 -Template "STS#0"
```
Creates a new subweb under the current web with URL projectA
