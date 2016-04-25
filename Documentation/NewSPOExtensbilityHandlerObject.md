#New-SPOExtensbilityHandlerObject
Creates a ExtensibilityHandler Object, to be used by the Get-SPOProvisioningTemplate cmdlet
##Syntax
```powershell
New-SPOExtensbilityHandlerObject -Type <String> [-Configuration <String>] [-Disabled [<SwitchParameter>]] -Assembly <String>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Assembly|String|True|The full assembly name of the handler|
|Configuration|String|False|Any configuration data you want to send to the handler|
|Disabled|SwitchParameter|False|If set, the handler will be disabled|
|Type|String|True|The type of the handler|
##Examples

###Example 1
```powershell

PS:> $handler = New-SPOExtensibilityHandlerObject -Assembly Contoso.Core.Handlers -Type Contoso.Core.Handlers.MyExtensibilityHandler
PS:> Get-SPOProvisioningTemplate -Out NewTemplate.xml -ExtensibilityHandlers $handler
```
This will create a new ExtensibilityHandler object that is run during extraction of the template
