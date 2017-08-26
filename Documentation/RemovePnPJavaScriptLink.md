# Remove-PnPJavaScriptLink
Removes a JavaScript link or block from a web or sitecollection
## Syntax
```powershell
Remove-PnPJavaScriptLink [-Force [<SwitchParameter>]]
                         [-Scope <CustomActionScope>]
                         [-Web <WebPipeBind>]
                         [-Identity <UserCustomActionPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Use the -Force flag to bypass the confirmation question|
|Identity|UserCustomActionPipeBind|False|Name or id of the JavaScriptLink to remove. Omit if you want to remove all JavaScript Links.|
|Scope|CustomActionScope|False|Define if the JavaScriptLink is to be found at the web or site collection scope. Specify All to allow deletion from either web or site collection.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPJavaScriptLink -Identity jQuery
```
Removes the injected JavaScript file with the name jQuery from the current web after confirmation

### Example 2
```powershell
PS:> Remove-PnPJavaScriptLink -Identity jQuery -Scope Site
```
Removes the injected JavaScript file with the name jQuery from the current site collection after confirmation

### Example 3
```powershell
PS:> Remove-PnPJavaScriptLink -Identity jQuery -Scope Site -Confirm:$false
```
Removes the injected JavaScript file with the name jQuery from the current site collection and will not ask for confirmation

### Example 4
```powershell
PS:> Remove-PnPJavaScriptLink -Scope Site
```
Removes all the injected JavaScript files from the current site collection after confirmation for each of them

### Example 5
```powershell
PS:> Remove-PnPJavaScriptLink -Identity faea0ce2-f0c2-4d45-a4dc-73898f3c2f2e -Scope All
```
Removes the injected JavaScript file with id faea0ce2-f0c2-4d45-a4dc-73898f3c2f2e from both the Web and Site scopes

### Example 6
```powershell
PS:> Get-PnPJavaScriptLink -Scope All | ? Sequence -gt 1000 | Remove-PnPJavaScriptLink
```
Removes all the injected JavaScript files from both the Web and Site scope that have a sequence number higher than 1000
