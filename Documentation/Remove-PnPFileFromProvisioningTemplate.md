---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPFileFromProvisioningTemplate

## SYNOPSIS
Removes a file from a PnP Provisioning Template

## SYNTAX 

```powershell
Remove-PnPFileFromProvisioningTemplate -Path <String>
                                       -FilePath <String>
                                       [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPFileFromProvisioningTemplate -Path template.pnp -FilePath filePath
```

Removes a file from an in-memory PnP Provisioning Template

## PARAMETERS

### -FilePath
The relative File Path of the file to remove from the in-memory template

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 1
Accept pipeline input: False
```

### -Path
Filename to read the template from, optionally including full path.

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: False
```

### -TemplateProviderExtensions
Allows you to specify ITemplateProviderExtension to execute while saving the template.

```yaml
Type: ITemplateProviderExtension[]
Parameter Sets: (All)

Required: False
Position: 2
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)