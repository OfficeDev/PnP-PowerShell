# Add-PnPFileToProvisioningTemplate
Adds a file to a PnP Provisioning Template
## Syntax
```powershell
Add-PnPFileToProvisioningTemplate -Path <String>
                                  -Source <String>
                                  -Folder <String>
                                  [-Container <String>]
                                  [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Folder|String|True|The target Folder for the file to add to the in-memory template.|
|Path|String|True|Filename of the .PNP Open XML provisioning template to read from, optionally including full path.|
|Source|String|True|The file to add to the in-memory template, optionally including full path.|
|Container|String|False|The target Container for the file to add to the in-memory template, optional argument.|
|TemplateProviderExtensions|ITemplateProviderExtension[]|False|Allows you to specify ITemplateProviderExtension to execute while loading the template.|
## Examples

### Example 1
```powershell
PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source $sourceFilePath -Folder $targetFolder
```
Adds a file to an in-memory PnP Provisioning Template

### Example 2
```powershell
PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source $sourceFilePath -Folder $targetFolder -Container $container
```
Adds a file to an in-memory PnP Provisioning Template with a custom container for the file
