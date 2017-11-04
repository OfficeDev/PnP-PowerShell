---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Update-PnPSiteClassification

## SYNOPSIS
Updates Site Classifications for the tenant. Requires a connection to the Microsoft Graph.

## SYNTAX 

### Specific
```powershell
Update-PnPSiteClassification [-Classifications <List`1>]
                             [-DefaultClassification <String>]
                             [-UsageGuidelinesUrl <String>]
```

### Settings
```powershell
Update-PnPSiteClassification -Settings <SiteClassificationSettings>
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Connect-PnPOnline -Scopes "Directory.ReadWrite.All"
PS:> Update-PnPSiteClassification -Classifications "HBI","Top Secret"
```

Replaces the existing values of the site classification settings

### ------------------EXAMPLE 2------------------
```powershell
PS:> Connect-PnPOnline -Scopes "Directory.ReadWrite.All"
PS:> Update-PnPSiteClassification -DefaultClassification "LBI"
```

Sets the default classification value to "LBI". This value needs to be present in the list of classification values.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Connect-PnPOnline -Scopes "Directory.ReadWrite.All"
PS:> Update-PnPSiteClassification -UsageGuidelinesUrl http://aka.ms/sppnp
```

sets the usage guideliness URL to the specified URL.

## PARAMETERS

### -Classifications
A list of classifications, separated by commas. E.g. "HBI","LBI","Top Secret"

```yaml
Type: List`1
Parameter Sets: Specific

Required: False
Position: Named
Accept pipeline input: False
```

### -DefaultClassification
The default classification to be used. The value needs to be present in the list of possible classifications

```yaml
Type: String
Parameter Sets: Specific

Required: False
Position: Named
Accept pipeline input: False
```

### -Settings
A settings object retrieved by Get-PnPSiteClassification

```yaml
Type: SiteClassificationSettings
Parameter Sets: Settings

Required: True
Position: Named
Accept pipeline input: False
```

### -UsageGuidelinesUrl
The UsageGuidelinesUrl. Set to "" to clear.

```yaml
Type: String
Parameter Sets: Specific

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)