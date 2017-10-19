---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPProvisioningTemplateFromGallery

## SYNOPSIS
Retrieves or searches provisioning templates from the PnP Template Gallery

## SYNTAX 

### Identity
```powershell
Get-PnPProvisioningTemplateFromGallery [-Identity <Guid>]
                                       [-Path <String>]
                                       [-Force [<SwitchParameter>]]
```

### Search
```powershell
Get-PnPProvisioningTemplateFromGallery [-Search <String>]
                                       [-TargetPlatform <TargetPlatform>]
                                       [-TargetScope <TargetScope>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
Get-PnPProvisioningTemplateFromGallery
```

Retrieves all templates from the gallery

### ------------------EXAMPLE 2------------------
```powershell
Get-PnPProvisioningTemplateFromGallery -Search "Data"
```

Searches for a templates containing the word 'Data' in the Display Name

### ------------------EXAMPLE 3------------------
```powershell
Get-PnPProvisioningTemplateFromGallery -Identity ae925674-8aa6-438b-acd0-d2699a022edd
```

Retrieves a template with the specified ID

### ------------------EXAMPLE 4------------------
```powershell
$template = Get-PnPProvisioningTemplateFromGallery -Identity ae925674-8aa6-438b-acd0-d2699a022edd
Apply-PnPProvisioningTemplate -InputInstance $template
```

Retrieves a template with the specified ID and applies it to the site.

### ------------------EXAMPLE 5------------------
```powershell
$template = Get-PnPProvisioningTemplateFromGallery -Identity ae925674-8aa6-438b-acd0-d2699a022edd -Path c:\temp
```

Retrieves a template with the specified ID and saves the template to the specified path

## PARAMETERS

### -Force


```yaml
Type: SwitchParameter
Parameter Sets: Identity

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity


```yaml
Type: Guid
Parameter Sets: Identity

Required: False
Position: Named
Accept pipeline input: False
```

### -Path


```yaml
Type: String
Parameter Sets: Identity

Required: False
Position: Named
Accept pipeline input: False
```

### -Search


```yaml
Type: String
Parameter Sets: Search

Required: False
Position: Named
Accept pipeline input: False
```

### -TargetPlatform


```yaml
Type: TargetPlatform
Parameter Sets: Search

Required: False
Position: Named
Accept pipeline input: False
```

### -TargetScope


```yaml
Type: TargetScope
Parameter Sets: Search

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)