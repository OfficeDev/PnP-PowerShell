# Add-PnPDataRowsToProvisioningTemplate
Adds datarows to a list inside a PnP Provisioning Template
## Syntax
```powershell
Add-PnPDataRowsToProvisioningTemplate -List <ListPipeBind>
                                      -Query <String>
                                      -Path <String>
                                      [-Fields <String[]>]
                                      [-Web <WebPipeBind>]
                                      [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
                                      [-IncludeSecurity [<SwitchParameter>]]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|List|ListPipeBind|True|The list to query|
|Path|String|True|Filename of the .PNP Open XML provisioning template to read from, optionally including full path.|
|Query|String|True|The CAML query to execute against the list|
|Fields|String[]|False|The fields to retrieve. If not specified all fields will be loaded in the returned list object.|
|IncludeSecurity|SwitchParameter|False|A switch to include ObjectSecurity information.|
|TemplateProviderExtensions|ITemplateProviderExtension[]|False|Allows you to specify ITemplateProviderExtension to execute while loading the template.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPDataRowsToProvisioningTemplate -Path template.pnp -List 'PnPTestList' -Query '<View></View>' -Fields 'Title','Choice'
```
Adds datarows to a list in an in-memory PnP Provisioning Template

### Example 2
```powershell
PS:> Add-PnPDataRowsToProvisioningTemplate -Path template.pnp -List 'PnPTestList' -Query '<View></View>' -Fields 'Title','Choice' -IncludeSecurity
```
Adds datarows to a list in an in-memory PnP Provisioning Template
