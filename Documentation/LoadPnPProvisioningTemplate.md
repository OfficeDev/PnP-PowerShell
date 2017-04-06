# Load-PnPProvisioningTemplate
Loads a PnP file from the file systems
## Syntax
```powershell
Load-PnPProvisioningTemplate -Path <String>
                             [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Path|String|True|Filename to read from, optionally including full path.|
|TemplateProviderExtensions|ITemplateProviderExtension[]|False|Allows you to specify ITemplateProviderExtension to execute while loading the template.|
## Examples

### Example 1
```powershell
PS:> Load-PnPProvisioningTemplate -Path template.pnp
```
Loads a PnP file from the file systems

### Example 2
```powershell
PS:> Load-PnPProvisioningTemplate -Path template.pnp -TemplateProviderExtensions $extensions
```
Loads a PnP file from the file systems using some custom template provider extenions while loading the file.
