---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Export-PnPTaxonomy

## SYNOPSIS
Exports a taxonomy to either the output or to a file.

## SYNTAX 

### TermSet
```powershell
Export-PnPTaxonomy [-TermSetId <GuidPipeBind>]
                   [-TermStoreName <String>]
                   [-IncludeID [<SwitchParameter>]]
                   [-Path <String>]
                   [-Force [<SwitchParameter>]]
                   [-Delimiter <String>]
                   [-Encoding <Encoding>]
                   [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Export-PnPTaxonomy
```

Exports the full taxonomy to the standard output

### ------------------EXAMPLE 2------------------
```powershell
PS:> Export-PnPTaxonomy -Path c:\output.txt
```

Exports the full taxonomy the file output.txt

### ------------------EXAMPLE 3------------------
```powershell
PS:> Export-PnPTaxonomy -Path c:\output.txt -TermSet f6f43025-7242-4f7a-b739-41fa32847254
```

Exports the term set with the specified id

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

### -Encoding
Defaults to Unicode

```yaml
Type: Encoding
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Force
Overwrites the output file if it exists.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -IncludeID
If specified will include the ids of the taxonomy items in the output. Format: <label>;#<guid>

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Path
File to export the data to.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -TermSetId
If specified, will export the specified termset only

```yaml
Type: GuidPipeBind
Parameter Sets: TermSet

Required: False
Position: Named
Accept pipeline input: False
```

### -TermStoreName
Term store to export; if not specified the default term store is used.

```yaml
Type: String
Parameter Sets: TermSet

Required: False
Position: Named
Accept pipeline input: False
```

### -Connection
Optional connection to be used by cmdlet. Retrieve the value for this parameter by eiter specifying -ReturnConnection on Connect-PnPOnline or by executing Get-PnPConnection.

```yaml
Type: SPOnlineConnection
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)