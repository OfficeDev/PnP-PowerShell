---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Remove-PnPSiteClassification

## SYNOPSIS
Removes one or more existing site classification values from the list of available values. Requires a connection to the Microsoft Graph

## SYNTAX 

```powershell
Remove-PnPSiteClassification -Classifications <List`1>
                             [-Confirm [<SwitchParameter>]]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Connect-PnPOnline -Scopes "Directory.ReadWrite.All"
PS:> Remove-PnPSiteClassification -Classifications "HBI"
```

Removes the "HBI" site classification from the list of available values.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Connect-PnPOnline -Scopes "Directory.ReadWrite.All"
PS:> Enable-PnPSiteClassification -Classifications "HBI","LBI","Top Secret" -DefaultClassification "LBI"
```

Enables Site Classifications for your tenant and provides three classification values. The default value will be set to "LBI"

### ------------------EXAMPLE 3------------------
```powershell
PS:> Connect-PnPOnline -Scopes "Directory.ReadWrite.All"
PS:> Enable-PnPSiteClassification -Classifications "HBI","LBI","Top Secret" -UsageGuidelinesUrl http://aka.ms/sppnp
```

Enables Site Classifications for your tenant and provides three classification values. The first value will be set as the default and the usage guideliness will be set to the specified URL.

## PARAMETERS

### -Classifications


```yaml
Type: List`1
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Confirm
Specifying the Confirm parameter will allow the confirmation question to be skipped

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)