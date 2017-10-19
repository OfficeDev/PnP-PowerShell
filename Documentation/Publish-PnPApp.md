---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Publish-PnPApp

## SYNOPSIS
Publishes/Deploys/Trusts an available app in the app catalog

## SYNTAX 

```powershell
Publish-PnPApp -Identity <AppMetadataPipeBind>
               [-SkipFeatureDeployment [<SwitchParameter>]]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Publish-PnPApp
```

This will deploy/trust an app into the app catalog. Notice that the app needs to be available in the app catalog

## PARAMETERS

### -Identity
Specifies the Id of the app

```yaml
Type: AppMetadataPipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -SkipFeatureDeployment


```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)