# Set-PnPSitePolicy
Sets a site policy
## Syntax
```powershell
Set-PnPSitePolicy -Name <String>
                  [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Name|String|True|The name of the site policy to apply|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Set-PnPSitePolicy -Name "Contoso HBI"
```
This applies a site policy with the name "Contoso HBI" to the current site. The policy needs to be available in the site.
