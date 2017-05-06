# Remove-PnPWeb
Removes a subweb in the current web
## Syntax
```powershell
Remove-PnPWeb -Url <String>
              [-Force [<SwitchParameter>]]
              [-Web <WebPipeBind>]
```


```powershell
Remove-PnPWeb -Identity <WebPipeBind>
              [-Force [<SwitchParameter>]]
              [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|WebPipeBind|True|Identity/Id/Web object to delete|
|Url|String|True|The site relative url of the web, e.g. 'Subweb1'|
|Force|SwitchParameter|False|Do not ask for confirmation to delete the subweb|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPWeb -Url projectA
```
Remove a web

### Example 2
```powershell
PS:> Remove-PnPWeb -Identity 5fecaf67-6b9e-4691-a0ff-518fc9839aa0
```
Remove a web specified by its ID

### Example 3
```powershell
PS:> Get-PnPSubWebs | Remove-PnPWeb -Force
```
Remove all subwebs and do not ask for confirmation
