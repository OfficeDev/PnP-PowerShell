---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Add-PnPSiteClassification

## SYNOPSIS
Adds one ore more site classification values to the list of possible values. Requires a connection to the Microsoft Graph.

## SYNTAX 

```powershell
Add-PnPSiteClassification -Classifications <List`1>
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Connect-PnPOnline -Scopes "Directory.ReadWrite.All"
PS:> Add-PnPSiteClassification -Classifications "Top Secret"
```

Adds the "Top Secret" classification to the already existing classification values.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Connect-PnPOnline -Scopes "Directory.ReadWrite.All"
PS:> Add-PnPSiteClassification -Classifications "Top Secret","For Your Eyes Only"
```

Adds the "Top Secret" and the "For Your Eyes Only" classification to the already existing classification values.

## PARAMETERS

### -Classifications


```yaml
Type: List`1
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)