---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Import-PnPTaxonomy

## SYNOPSIS
Imports a taxonomy from either a string array or a file

## SYNTAX 

### Direct
```powershell
Import-PnPTaxonomy [-Terms <String[]>]
                   [-Lcid <Int>]
                   [-TermStoreName <String>]
                   [-Delimiter <String>]
                   [-SynchronizeDeletions [<SwitchParameter>]]
```

### File
```powershell
Import-PnPTaxonomy -Path <String>
                   [-Lcid <Int>]
                   [-TermStoreName <String>]
                   [-Delimiter <String>]
                   [-SynchronizeDeletions [<SwitchParameter>]]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Import-PnPTaxonomy -Terms 'Company|Locations|Stockholm'
```

Creates a new termgroup, 'Company', a termset 'Locations' and a term 'Stockholm'

### ------------------EXAMPLE 2------------------
```powershell
PS:> Import-PnPTaxonomy -Terms 'Company|Locations|Stockholm|Central','Company|Locations|Stockholm|North'
```

Creates a new termgroup, 'Company', a termset 'Locations', a term 'Stockholm' and two subterms: 'Central', and 'North'

## PARAMETERS

### -Delimiter
The path delimiter to be used, by default this is '|'

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Lcid


```yaml
Type: Int
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Path
Specifies a file containing terms per line, in the format as required by the Terms parameter.

```yaml
Type: String
Parameter Sets: File

Required: True
Position: Named
Accept pipeline input: False
```

### -SynchronizeDeletions
If specified, terms that exist in the termset, but are not in the imported data, will be removed.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Terms
An array of strings describing termgroup, termset, term, subterms using a default delimiter of '|'.

```yaml
Type: String[]
Parameter Sets: Direct

Required: False
Position: Named
Accept pipeline input: True
```

### -TermStoreName
Term store to import to; if not specified the default term store is used.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)