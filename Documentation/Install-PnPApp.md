---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Install-PnPApp

## SYNOPSIS
Installs an available app from the app catalog

## SYNTAX 

```powershell
Install-PnPApp -Identity <AppMetadataPipeBind>
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Install-PnPApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe
```

This will install an available app, specified by the id, to the current site.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPAvailableApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe | Install-PnPApp
```

This will install the given app into the site.

## PARAMETERS

### -Identity
Specifies the Id or an actual app metadata instance

```yaml
Type: AppMetadataPipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)