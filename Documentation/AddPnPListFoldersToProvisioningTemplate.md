# Add-PnPListFoldersToProvisioningTemplate
Adds folders to a list in a PnP Provisioning Template
## Syntax
```powershell
Add-PnPListFoldersToProvisioningTemplate -Path <String>
                                         -List <ListPipeBind>
                                         [-Web <WebPipeBind>]
                                         [-Recursive [<SwitchParameter>]]
                                         [-IncludeSecurity [<SwitchParameter>]]
                                         [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|List|ListPipeBind|True|The list to query|
|Path|String|True|Filename of the .PNP Open XML provisioning template to read from, optionally including full path.|
|IncludeSecurity|SwitchParameter|False|A switch to include ObjectSecurity information.|
|Recursive|SwitchParameter|False|A switch parameter to include all folders in the list, or just top level folders.|
|TemplateProviderExtensions|ITemplateProviderExtension[]|False|Allows you to specify ITemplateProviderExtension to execute while loading the template.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPListFoldersToProvisioningTemplate -Path template.pnp -List 'PnPTestList'
```
Adds top level folders from a list to an existing template and returns an in-memory PnP Provisioning Template

### Example 2
```powershell
PS:> Add-PnPListFoldersToProvisioningTemplate -Path template.pnp -List 'PnPTestList' -Recursive
```
Adds all folders from a list to an existing template and returns an in-memory PnP Provisioning Template

### Example 3
```powershell
PS:> Add-PnPListFoldersToProvisioningTemplate -Path template.pnp -List 'PnPTestList' -Recursive -IncludeSecurity
```
Adds all folders from a list with unique permissions to an in-memory PnP Provisioning Template
