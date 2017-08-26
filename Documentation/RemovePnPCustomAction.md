# Remove-PnPCustomAction
Removes a custom action
## Syntax
```powershell
Remove-PnPCustomAction [-Scope <CustomActionScope>]
                       [-Force [<SwitchParameter>]]
                       [-Web <WebPipeBind>]
                       [-Identity <UserCustomActionPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Use the -Force flag to bypass the confirmation question|
|Identity|UserCustomActionPipeBind|False|The id or name of the CustomAction that needs to be removed or a CustomAction instance itself|
|Scope|CustomActionScope|False|Define if the CustomAction is to be found at the web or site collection scope. Specify All to allow deletion from either web or site collection.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPCustomAction -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2 -Scope All
```
Removes the custom action with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2' from the web and site

### Example 2
```powershell
PS:> Remove-PnPCustomAction -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2 -Scope Web
```
Removes the custom action with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2' from the current web

### Example 3
```powershell
PS:> Remove-PnPCustomAction -Identity jQuery -Scope Site -Confirm:$false
```
Removes the custom action with the name 'jQuery' from the current site without asking for confirmation

### Example 4
```powershell
PS:> Get-PnPCustomAction -Scope All | ? Location -eq ScriptLink | Remove-PnPCustomAction
```
Removes all custom actions that are ScriptLinks
