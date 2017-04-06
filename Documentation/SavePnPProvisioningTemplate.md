# Save-PnPProvisioningTemplate
Saves a PnP file to the file systems
## Syntax
```powershell
Save-PnPProvisioningTemplate -InputInstance <ProvisioningTemplate>
                             -Out <String>
                             [-Force [<SwitchParameter>]]
                             [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|InputInstance|ProvisioningTemplate|True|Allows you to provide an in-memory instance of the ProvisioningTemplate type of the PnP Core Component. When using this parameter, the -Out parameter refers to the path for saving the template and storing any supporting file for the template.|
|Out|String|True|Filename to write to, optionally including full path.|
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question.|
|TemplateProviderExtensions|ITemplateProviderExtension[]|False|Allows you to specify the ITemplateProviderExtension to execute while saving a template.|
## Examples

### Example 1
```powershell
PS:> Save-PnPProvisioningTemplate -InputInstance $template -Out .\template.pnp
```
Saves a PnP file to the file systems
