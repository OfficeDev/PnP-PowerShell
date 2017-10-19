---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPProvisioningTemplate

## SYNOPSIS
Generates a provisioning template from a web

## SYNTAX 

```powershell
Get-PnPProvisioningTemplate [-IncludeAllTermGroups [<SwitchParameter>]]
                            [-IncludeSiteCollectionTermGroup [<SwitchParameter>]]
                            [-IncludeSiteGroups [<SwitchParameter>]]
                            [-IncludeTermGroupsSecurity [<SwitchParameter>]]
                            [-IncludeSearchConfiguration [<SwitchParameter>]]
                            [-PersistBrandingFiles [<SwitchParameter>]]
                            [-PersistPublishingFiles [<SwitchParameter>]]
                            [-IncludeNativePublishingFiles [<SwitchParameter>]]
                            [-SkipVersionCheck [<SwitchParameter>]]
                            [-PersistMultiLanguageResources [<SwitchParameter>]]
                            [-ResourceFilePrefix <String>]
                            [-Handlers <Handlers>]
                            [-ExcludeHandlers <Handlers>]
                            [-ExtensibilityHandlers <ExtensibilityHandler[]>]
                            [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
                            [-ContentTypeGroups <String[]>]
                            [-Force [<SwitchParameter>]]
                            [-Encoding <Encoding>]
                            [-TemplateDisplayName <String>]
                            [-TemplateImagePreviewUrl <String>]
                            [-TemplateProperties <Hashtable>]
                            [-OutputInstance [<SwitchParameter>]]
                            [-ExcludeContentTypesFromSyndication [<SwitchParameter>]]
                            [-Web <WebPipeBind>]
                            [-Out <String>]
                            [-Schema <XMLPnPSchemaVersion>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPProvisioningTemplate -Out template.pnp
```

Extracts a provisioning template in Office Open XML from the current web.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPProvisioningTemplate -Out template.xml
```

Extracts a provisioning template in XML format from the current web.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPProvisioningTemplate -Out template.pnp -Schema V201503
```

Extracts a provisioning template in Office Open XML from the current web and saves it in the V201503 version of the schema.

### ------------------EXAMPLE 4------------------
```powershell
PS:> Get-PnPProvisioningTemplate -Out template.pnp -IncludeAllTermGroups
```

Extracts a provisioning template in Office Open XML from the current web and includes all term groups, term sets and terms from the Managed Metadata Service Taxonomy.

### ------------------EXAMPLE 5------------------
```powershell
PS:> Get-PnPProvisioningTemplate -Out template.pnp -IncludeSiteCollectionTermGroup
```

Extracts a provisioning template in Office Open XML from the current web and includes the term group currently (if set) assigned to the site collection.

### ------------------EXAMPLE 6------------------
```powershell
PS:> Get-PnPProvisioningTemplate -Out template.pnp -PersistComposedLookFiles
```

Extracts a provisioning template in Office Open XML from the current web and saves the files that make up the composed look to the same folder as where the template is saved.

### ------------------EXAMPLE 7------------------
```powershell
PS:> Get-PnPProvisioningTemplate -Out template.pnp -Handlers Lists, SiteSecurity
```

Extracts a provisioning template in Office Open XML from the current web, but only processes lists and site security when generating the template.

### ------------------EXAMPLE 8------------------
```powershell

PS:> $handler1 = New-PnPExtensibilityHandlerObject -Assembly Contoso.Core.Handlers -Type Contoso.Core.Handlers.MyExtensibilityHandler1
PS:> $handler2 = New-PnPExtensibilityHandlerObject -Assembly Contoso.Core.Handlers -Type Contoso.Core.Handlers.MyExtensibilityHandler1
PS:> Get-PnPProvisioningTemplate -Out NewTemplate.xml -ExtensibilityHandlers $handler1,$handler2
```

This will create two new ExtensibilityHandler objects that are run during extraction of the template

### ------------------EXAMPLE 9------------------
Only supported on SP2016 and SP Online
```powershell
PS:> Get-PnPProvisioningTemplate -Out template.pnp -PersistMultiLanguageResources
```

Extracts a provisioning template in Office Open XML from the current web, and for supported artifacts it will create a resource file for each supported language (based upon the language settings of the current web). The generated resource files will be named after the value specified in the Out parameter. For instance if the Out parameter is specified as -Out 'template.xml' the generated resource file will be called 'template.en-US.resx'.

### ------------------EXAMPLE 10------------------
Only supported on SP2016 and SP Online
```powershell
PS:> Get-PnPProvisioningTemplate -Out template.pnp -PersistMultiLanguageResources -ResourceFilePrefix MyResources
```

Extracts a provisioning template in Office Open XML from the current web, and for supported artifacts it will create a resource file for each supported language (based upon the language settings of the current web). The generated resource files will be named 'MyResources.en-US.resx' etc.

### ------------------EXAMPLE 11------------------
```powershell
PS:> $template = Get-PnPProvisioningTemplate -OutputInstance
```

Extracts an instance of a provisioning template object from the current web. This syntax cannot be used together with the -Out parameter, but it can be used together with any other supported parameters.

### ------------------EXAMPLE 12------------------
```powershell
PS:> Get-PnPProvisioningTemplate -Out template.pnp -ContentTypeGroups "Group A","Group B"
```

Extracts a provisioning template in Office Open XML from the current web, but only processes content types from the to given content type groups.

### ------------------EXAMPLE 13------------------
```powershell
PS:> Get-PnPProvisioningTemplate -Out template.pnp -ExcludeContentTypesFromSyndication
```

Extracts a provisioning template in Office Open XML from the current web, excluding content types provisioned through content type syndication (content type hub), in order to prevent provisioning errors if the target also provision the content type using syndication.

## PARAMETERS

### -ContentTypeGroups
Allows you to specify from which content type group(s) the content types should be included into the template.

```yaml
Type: String[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Encoding
The encoding type of the XML file, Unicode is default

```yaml
Type: Encoding
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ExcludeContentTypesFromSyndication
Specify whether or not content types issued from a content hub should be exported. By default, these content types are included.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ExcludeHandlers
Allows you to run all handlers, excluding the ones specified.

```yaml
Type: Handlers
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ExtensibilityHandlers
Allows you to specify ExtensbilityHandlers to execute while extracting a template.

```yaml
Type: ExtensibilityHandler[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Force
Overwrites the output file if it exists.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Handlers
Allows you to only process a specific type of artifact in the site. Notice that this might result in a non-working template, as some of the handlers require other artifacts in place if they are not part of what your extracting.

```yaml
Type: Handlers
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -IncludeAllTermGroups
If specified, all term groups will be included. Overrides IncludeSiteCollectionTermGroup.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -IncludeNativePublishingFiles
If specified, out of the box / native publishing files will be saved.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -IncludeSearchConfiguration
If specified the template will contain the current search configuration of the site.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -IncludeSiteCollectionTermGroup
If specified, all the site collection term groups will be included. Overridden by IncludeAllTermGroups.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -IncludeSiteGroups
If specified all site groups will be included.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -IncludeTermGroupsSecurity
If specified all the managers and contributors of term groups will be included.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Out
Filename to write to, optionally including full path

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: 0
Accept pipeline input: False
```

### -OutputInstance
Returns the template as an in-memory object, which is an instance of the ProvisioningTemplate type of the PnP Core Component. It cannot be used together with the -Out parameter.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -PersistBrandingFiles
If specified the files used for masterpages, sitelogo, alternate CSS and the files that make up the composed look will be saved.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -PersistMultiLanguageResources
If specified, resource values for applicable artifacts will be persisted to a resource file

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -PersistPublishingFiles
If specified the files used for the publishing feature will be saved.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ResourceFilePrefix
If specified, resource files will be saved with the specified prefix instead of using the template name specified. If no template name is specified the files will be called PnP-Resources.<language>.resx. See examples for more info.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Schema
The schema of the output to use, defaults to the latest schema

```yaml
Type: XMLPnPSchemaVersion
Parameter Sets: (All)

Required: False
Position: 1
Accept pipeline input: False
```

### -SkipVersionCheck
During extraction the version of the server will be checked for certain actions. If you specify this switch, this check will be skipped.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -TemplateDisplayName
It can be used to specify the DisplayName of the template file that will be extracted.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -TemplateImagePreviewUrl
It can be used to specify the ImagePreviewUrl of the template file that will be extracted.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -TemplateProperties
It can be used to specify custom Properties for the template file that will be extracted.

```yaml
Type: Hashtable
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -TemplateProviderExtensions
Allows you to specify ITemplateProviderExtension to execute while extracting a template.

```yaml
Type: ITemplateProviderExtension[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Web
The GUID, server relative url (i.e. /sites/team1) or web instance of the web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)[Encoding](https://msdn.microsoft.com/en-us/library/system.text.encoding_properties.aspx)