---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Enable-PnPSiteClassification

## SYNOPSIS
Enables Site Classifications for the tenant. Requires a connection to the Microsoft Graph.

## SYNTAX 

```powershell
Enable-PnPSiteClassification -Classifications <List`1>
                             -DefaultClassification <String>
                             [-UsageGuidelinesUrl <String>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Connect-PnPOnline -Scopes "Directory.ReadWrite.All"
PS:> Enable-PnPSiteClassification -Classifications "HBI","LBI","Top Secret" -DefaultClassification "LBI"
```

Enables Site Classifications for your tenant and provides three classification values. The default value will be set to "LBI"

### ------------------EXAMPLE 2------------------
```powershell
PS:> Connect-PnPOnline -Scopes "Directory.ReadWrite.All"
PS:> Enable-PnPSiteClassification -Classifications "HBI","LBI","Top Secret" -UsageGuidelinesUrl http://aka.ms/sppnp
```

Enables Site Classifications for your tenant and provides three classification values. The usage guideliness will be set to the specified URL.

## PARAMETERS

### -Classifications


```yaml
Type: List`1
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -DefaultClassification


```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -UsageGuidelinesUrl


```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)