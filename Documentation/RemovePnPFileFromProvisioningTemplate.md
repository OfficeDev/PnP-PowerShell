#Remove-PnPFileFromProvisioningTemplate
Removes a file from an in-memory PnP Provisioning Template
##Syntax
```powershell
Remove-PnPFileFromProvisioningTemplate -Path <String>
                                       -FilePath <String>
                                       [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|FilePath|String|True|The relative File Path of the file to remove from the in-memory template|
|Path|String|True|Filename to read the template from, optionally including full path.|
|TemplateProviderExtensions|ITemplateProviderExtension[]|False|Allows you to specify ITemplateProviderExtension to execute while saving the template.|
##Examples

###Example 1
```powershell
PS:> Remove-PnPFileFromProvisioningTemplate -Path template.pnp -FilePath filePath
```
Removes a file from an in-memory PnP Provisioning Template
