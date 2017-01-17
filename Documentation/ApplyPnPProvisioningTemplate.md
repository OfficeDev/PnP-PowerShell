#Apply-PnPProvisioningTemplate
Applies a provisioning template to a web
##Syntax
```powershell
Apply-PnPProvisioningTemplate [-GalleryTemplateId <Guid>]
                              [-ResourceFolder <String>]
                              [-OverwriteSystemPropertyBagValues [<SwitchParameter>]]
                              [-ClearNavigation [<SwitchParameter>]]
                              [-Parameters <Hashtable>]
                              [-Handlers <Handlers>]
                              [-ExcludeHandlers <Handlers>]
                              [-ExtensibilityHandlers <ExtensibilityHandler[]>]
                              [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
                              [-Web <WebPipeBind>]
```


```powershell
Apply-PnPProvisioningTemplate [-InputInstance <ProvisioningTemplate>]
                              [-ResourceFolder <String>]
                              [-OverwriteSystemPropertyBagValues [<SwitchParameter>]]
                              [-ClearNavigation [<SwitchParameter>]]
                              [-Parameters <Hashtable>]
                              [-Handlers <Handlers>]
                              [-ExcludeHandlers <Handlers>]
                              [-ExtensibilityHandlers <ExtensibilityHandler[]>]
                              [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
                              [-Web <WebPipeBind>]
```


```powershell
Apply-PnPProvisioningTemplate [-ResourceFolder <String>]
                              [-OverwriteSystemPropertyBagValues [<SwitchParameter>]]
                              [-ClearNavigation [<SwitchParameter>]]
                              [-Parameters <Hashtable>]
                              [-Handlers <Handlers>]
                              [-ExcludeHandlers <Handlers>]
                              [-ExtensibilityHandlers <ExtensibilityHandler[]>]
                              [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
                              [-Web <WebPipeBind>]
                              -Path <String>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ClearNavigation|SwitchParameter|False|Override the RemoveExistingNodes attribute in the Navigation elements of the template. If you specify this value the navigation nodes will always be removed before adding the nodes in the template|
|ExcludeHandlers|Handlers|False|Allows you to run all handlers, excluding the ones specified.|
|ExtensibilityHandlers|ExtensibilityHandler[]|False|Allows you to specify ExtensbilityHandlers to execute while applying a template|
|GalleryTemplateId|Guid|False||
|Handlers|Handlers|False|Allows you to only process a specific part of the template. Notice that this might fail, as some of the handlers require other artifacts in place if they are not part of what your applying.|
|InputInstance|ProvisioningTemplate|False|Allows you to provide an in-memory instance of the ProvisioningTemplate type of the PnP Core Component. When using this parameter, the -Path parameter refers to the path of any supporting file for the template.|
|OverwriteSystemPropertyBagValues|SwitchParameter|False|Specify this parameter if you want to overwrite and/or create properties that are known to be system entries (starting with vti_, dlc_, etc.)|
|Parameters|Hashtable|False|Allows you to specify parameters that can be referred to in the template by means of the {parameter:<Key>} token. See examples on how to use this parameter.|
|Path|String|True|Path to the xml or pnp file containing the provisioning template.|
|ResourceFolder|String|False|Root folder where resources/files that are being referenced in the template are located. If not specified the same folder as where the provisioning template is located will be used.|
|TemplateProviderExtensions|ITemplateProviderExtension[]|False|Allows you to specify ITemplateProviderExtension to execute while applying a template.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Apply-PnPProvisioningTemplate -Path template.xml
```
Applies a provisioning template in XML format to the current web.

###Example 2
```powershell
PS:> Apply-PnPProvisioningTemplate -Path template.xml -ResourceFolder c:\provisioning\resources
```
Applies a provisioning template in XML format to the current web. Any resources like files that are referenced in the template will be retrieved from the folder as specified with the ResourceFolder parameter.

###Example 3
```powershell
PS:> Apply-PnPProvisioningTemplate -Path template.xml -Parameters @{"ListTitle"="Projects";"parameter2"="a second value"}
```
Applies a provisioning template in XML format to the current web. It will populate the parameter in the template the values as specified and in the template you can refer to those values with the {parameter:<key>} token.

For instance with the example above, specifying {parameter:ListTitle} in your template will translate to 'Projects' when applying the template. These tokens can be used in most string values in a template.

###Example 4
```powershell
PS:> Apply-PnPProvisioningTemplate -Path template.xml -Handlers Lists, SiteSecurity
```
Applies a provisioning template in XML format to the current web. It will only apply the lists and site security part of the template.

###Example 5
```powershell
PS:> Apply-PnPProvisioningTemplate -Path template.pnp
```
Applies a provisioning template from a pnp package to the current web.

###Example 6
```powershell
PS:> Apply-PnPProvisioningTemplate -Path https://tenant.sharepoint.com/sites/templatestorage/Documents/template.pnp
```
Applies a provisioning template from a pnp package stored in a library to the current web.

###Example 7
```powershell

PS:> $handler1 = New-PnPExtensibilityHandlerObject -Assembly Contoso.Core.Handlers -Type Contoso.Core.Handlers.MyExtensibilityHandler1
PS:> $handler2 = New-PnPExtensibilityHandlerObject -Assembly Contoso.Core.Handlers -Type Contoso.Core.Handlers.MyExtensibilityHandler1
PS:> Apply-PnPProvisioningTemplate -Path NewTemplate.xml -ExtensibilityHandlers $handler1,$handler2
```
This will create two new ExtensibilityHandler objects that are run while provisioning the template

###Example 8
```powershell
PS:> Apply-PnPProvisioningTemplate -Path .\ -InputInstance $template
```
Applies a provisioning template from an in-memory instance of a ProvisioningTemplate type of the PnP Core Component, reading the supporting files, if any, from the current (.\) path. The syntax can be used together with any other supported parameters.
