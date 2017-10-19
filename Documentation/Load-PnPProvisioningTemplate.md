---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Load-PnPProvisioningTemplate

## SYNOPSIS
Loads a PnP file from the file systems

## SYNTAX 

```powershell
Load-PnPProvisioningTemplate -Path <String>
                             [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Load-PnPProvisioningTemplate -Path template.pnp
```

Loads a PnP file from the file systems

### ------------------EXAMPLE 2------------------
```powershell
PS:> Load-PnPProvisioningTemplate -Path template.pnp -TemplateProviderExtensions $extensions
```

Loads a PnP file from the file systems using some custom template provider extenions while loading the file.

## PARAMETERS

### -Path
Filename to read from, optionally including full path.

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: False
```

### -TemplateProviderExtensions
Allows you to specify ITemplateProviderExtension to execute while loading the template.

```yaml
Type: ITemplateProviderExtension[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)