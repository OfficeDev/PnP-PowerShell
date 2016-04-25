#Get-SPOProvisioningTemplate
Generates a provisioning template from a web
##Syntax
```powershell
Get-SPOProvisioningTemplate [-IncludeAllTermGroups [<SwitchParameter>]] [-IncludeSiteCollectionTermGroup [<SwitchParameter>]] [-IncludeSiteGroups [<SwitchParameter>]] [-PersistBrandingFiles [<SwitchParameter>]] [-PersistPublishingFiles [<SwitchParameter>]] [-IncludeNativePublishingFiles [<SwitchParameter>]] [-Handlers <Handlers>] [-ExcludeHandlers <Handlers>] [-ExtensibilityHandlers <ExtensibilityHandler[]>] [-Force [<SwitchParameter>]] [-Encoding <Encoding>] [-Web <WebPipeBind>] [-Out <String>] [-Schema <XMLPnPSchemaVersion>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Encoding|Encoding|False||
|ExcludeHandlers|Handlers|False|Allows you to run all handlers, excluding the ones specified.|
|ExtensibilityHandlers|ExtensibilityHandler[]|False|Allows you to specify ExtensbilityHandlers to execute while extracting a template|
|Force|SwitchParameter|False|Overwrites the output file if it exists.|
|Handlers|Handlers|False|Allows you to only process a specific type of artifact in the site. Notice that this might result in a non-working template, as some of the handlers require other artifacts in place if they are not part of what your extracting.|
|IncludeAllTermGroups|SwitchParameter|False|If specified, all term groups will be included. Overrides IncludeSiteCollectionTermGroup.|
|IncludeNativePublishingFiles|SwitchParameter|False|If specified, out of the box / native publishing files will be saved.|
|IncludeSiteCollectionTermGroup|SwitchParameter|False|If specified, all the site collection term groups will be included. Overridden by IncludeAllTermGroups.|
|IncludeSiteGroups|SwitchParameter|False|If specified all site groups will be included.|
|Out|String|False|Filename to write to, optionally including full path|
|PersistBrandingFiles|SwitchParameter|False|If specified the files used for masterpages, sitelogo, alternate CSS and the files that make up the composed look will be saved.|
|PersistPublishingFiles|SwitchParameter|False|If specified the files used for the publishing feature will be saved.|
|Schema|XMLPnPSchemaVersion|False|The schema of the output to use, defaults to the latest schema|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-SPOProvisioningTemplate -Out template.xml
```
Extracts a provisioning template in XML format from the current web.

###Example 2
```powershell
PS:> Get-SPOProvisioningTemplate -Out template.xml -Schema V201503
```
Extracts a provisioning template in XML format from the current web and saves it in the V201503 version of the schema.

###Example 3
```powershell
PS:> Get-SPOProvisioningTemplate -Out template.xml -IncludeAllTermGroups
```
Extracts a provisioning template in XML format from the current web and includes all term groups, term sets and terms from the Managed Metadata Service Taxonomy.

###Example 4
```powershell
PS:> Get-SPOProvisioningTemplate -Out template.xml -IncludeSiteCollectionTermGroup
```
Extracts a provisioning template in XML format from the current web and includes the term group currently (if set) assigned to the site collection.

###Example 5
```powershell
PS:> Get-SPOProvisioningTemplate -Out template.xml -PersistComposedLookFiles
```
Extracts a provisioning template in XML format from the current web and saves the files that make up the composed look to the same folder as where the template is saved.

###Example 6
```powershell
PS:> Get-SPOProvisioningTemplate -Out template.xml -Handlers Lists, SiteSecurity
```
Extracts a provisioning template in XML format from the current web, but only processes lists and site security when generating the template.

###Example 7
```powershell

PS:> $handler1 = New-SPOExtensibilityHandlerObject -Assembly Contoso.Core.Handlers -Type Contoso.Core.Handlers.MyExtensibilityHandler1
PS:> $handler2 = New-SPOExtensibilityHandlerObject -Assembly Contoso.Core.Handlers -Type Contoso.Core.Handlers.MyExtensibilityHandler1
PS:> Get-SPOProvisioningTemplate -Out NewTemplate.xml -ExtensibilityHandlers $handler1,$handler2
```
This will create two new ExtensibilityHandler objects that are run during extraction of the template
