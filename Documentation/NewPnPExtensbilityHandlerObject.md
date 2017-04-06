# New-PnPExtensbilityHandlerObject
Creates an ExtensibilityHandler Object, to be used by the Get-SPOProvisioningTemplate cmdlet
## Syntax
```powershell
New-PnPExtensbilityHandlerObject -Type <String>
                                 -Assembly <String>
                                 [-Configuration <String>]
                                 [-Disabled [<SwitchParameter>]]
```


## Returns
>OfficeDevPnP.Core.Framework.Provisioning.Model.ExtensibilityHandler

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Assembly|String|True|The full assembly name of the handler|
|Type|String|True|The type of the handler|
|Configuration|String|False|Any configuration data you want to send to the handler|
|Disabled|SwitchParameter|False|If set, the handler will be disabled|
## Examples

### Example 1
```powershell

PS:> $handler = New-PnPExtensbilityHandlerObject -Assembly Contoso.Core.Handlers -Type Contoso.Core.Handlers.MyExtensibilityHandler
PS:> Get-PnPProvisioningTemplate -Out NewTemplate.xml -ExtensibilityHandlers $handler
```
This will create a new ExtensibilityHandler object that is run during extraction of the template
