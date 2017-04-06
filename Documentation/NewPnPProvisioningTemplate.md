# New-PnPProvisioningTemplate
Creates a new provisioning template object
## Syntax
```powershell
New-PnPProvisioningTemplate [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> $template = New-PnPProvisioningTemplate
```
Creates a new instance of a provisioning template object.
