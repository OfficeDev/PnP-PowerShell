---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPProvisioningTemplateMetadata

## SYNOPSIS
Sets metadata of a provisioning template

## SYNTAX 

```powershell
Set-PnPProvisioningTemplateMetadata -Path <String>
                                    [-TemplateDisplayName <String>]
                                    [-TemplateImagePreviewUrl <String>]
                                    [-TemplateProperties <Hashtable>]
                                    [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
                                    [-Web <WebPipeBind>]
                                    [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPProvisioningTemplateMetadata -Path template.xml -TemplateDisplayName "DisplayNameValue"
```

Sets the DisplayName property of a provisioning template in XML format.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPProvisioningTemplateMetadata -Path template.pnp -TemplateDisplayName "DisplayNameValue"
```

Sets the DisplayName property of a provisioning template in Office Open XML format.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Set-PnPProvisioningTemplateMetadata -Path template.xml -TemplateImagePreviewUrl "Full URL of the Image Preview"
```

Sets the Url to the preview image of a provisioning template in XML format.

### ------------------EXAMPLE 4------------------
```powershell
PS:> Set-PnPProvisioningTemplateMetadata -Path template.pnp -TemplateImagePreviewUrl "Full URL of the Image Preview"
```

Sets the to the preview image of a provisioning template in Office Open XML format.

### ------------------EXAMPLE 5------------------
```powershell
PS:> Set-PnPProvisioningTemplateMetadata -Path template.xml -TemplateProperties @{"Property1" = "Test Value 1"; "Property2"="Test Value 2"}
```

Sets the property 'Property1' to the value 'Test Value 1' of a provisioning template in XML format.

### ------------------EXAMPLE 6------------------
```powershell
PS:> Set-PnPProvisioningTemplateMetadata -Path template.pnp -TemplateProperties @{"Property1" = "Test Value 1"; "Property2"="Test Value 2"}
```

Sets the property 'Property1' to the value 'Test Value 1' of a provisioning template in Office Open XML format.

## PARAMETERS

### -Path
Path to the xml or pnp file containing the provisioning template.

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -TemplateDisplayName
It can be used to specify the DisplayName of the template file that will be updated.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -TemplateImagePreviewUrl
It can be used to specify the ImagePreviewUrl of the template file that will be updated.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -TemplateProperties
It can be used to specify custom Properties for the template file that will be updated.

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

### -Connection
Optional connection to be used by cmdlet. Retrieve the value for this parameter by eiter specifying -ReturnConnection on Connect-PnPOnline or by executing Get-PnPConnection.

```yaml
Type: SPOnlineConnection
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

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)