#Set-PnPProvisioningTemplateMetadata
Sets metadata of a provisioning template
##Syntax
```powershell
Set-PnPProvisioningTemplateMetadata [-TemplateDisplayName <String>]
                                    [-TemplateImagePreviewUrl <String>]
                                    [-TemplateProperties <Hashtable>]
                                    [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
                                    [-Web <WebPipeBind>]
                                    -Path <String>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Path|String|True|Path to the xml or pnp file containing the provisioning template.|
|TemplateDisplayName|String|False|It can be used to specify the DisplayName of the template file that will be updated.|
|TemplateImagePreviewUrl|String|False|It can be used to specify the ImagePreviewUrl of the template file that will be updated.|
|TemplateProperties|Hashtable|False|It can be used to specify custom Properties for the template file that will be updated.|
|TemplateProviderExtensions|ITemplateProviderExtension[]|False|Allows you to specify ITemplateProviderExtension to execute while extracting a template.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Set-PnPProvisioningTemplateMetadata -Path template.xml -TemplateDisplayName "DisplayNameValue"
```
Sets the DisplayName property of a provisioning template in XML format.

###Example 2
```powershell
PS:> Set-PnPProvisioningTemplateMetadata -Path template.pnp -TemplateDisplayName "DisplayNameValue"
```
Sets the DisplayName property of a provisioning template in Office Open XML format.

###Example 3
```powershell
PS:> Set-PnPProvisioningTemplateMetadata -Path template.xml -TemplateImagePreviewUrl "Full URL of the Image Preview"
```
Sets the Url to the preview image of a provisioning template in XML format.

###Example 4
```powershell
PS:> Set-PnPProvisioningTemplateMetadata -Path template.pnp -TemplateImagePreviewUrl "Full URL of the Image Preview"
```
Sets the to the preview image of a provisioning template in Office Open XML format.

###Example 5
```powershell
PS:> Set-PnPProvisioningTemplateMetadata -Path template.xml -TemplateProperties @{"Property1" = "Test Value 1"; "Property2"="Test Value 2"}
```
Sets the property 'Property1' to the value 'Test Value 1' of a provisioning template in XML format.

###Example 6
```powershell
PS:> Set-PnPProvisioningTemplateMetadata -Path template.pnp -TemplateProperties @{"Property1" = "Test Value 1"; "Property2"="Test Value 2"}
```
Sets the property 'Property1' to the value 'Test Value 1' of a provisioning template in Office Open XML format.
