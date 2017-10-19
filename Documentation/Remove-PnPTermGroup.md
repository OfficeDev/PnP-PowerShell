---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPTermGroup

## SYNOPSIS
Removes a taxonomy term group and all its containing termsets

## SYNTAX 

```powershell
Remove-PnPTermGroup -GroupName <String>
                    [-TermStoreName <String>]
                    [-Force [<SwitchParameter>]]
```

## PARAMETERS

### -Force


```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -GroupName
Name of the taxonomy term group to delete.

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -TermStoreName
Term store to use; if not specified the default term store is used.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)