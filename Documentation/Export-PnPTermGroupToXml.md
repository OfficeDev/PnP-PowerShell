---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Export-PnPTermGroupToXml

## SYNOPSIS
Exports a taxonomy TermGroup to either the output or to an XML file.

## SYNTAX 

```powershell
Export-PnPTermGroupToXml [-Identity <Id, Title or TermGroup>]
                         [-Out <String>]
                         [-FullTemplate [<SwitchParameter>]]
                         [-Encoding <Encoding>]
                         [-Force [<SwitchParameter>]]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Export-PnPTermGroupToXml
```

Exports all term groups in the default site collection term store to the standard output

### ------------------EXAMPLE 2------------------
```powershell
PS:> Export-PnPTermGroupToXml -Out output.xml
```

Exports all term groups in the default site collection term store to the file 'output.xml' in the current folder

### ------------------EXAMPLE 3------------------
```powershell
PS:> Export-PnPTermGroupToXml -Out c:\output.xml -Identity "Test Group"
```

Exports the term group with the specified name to the file 'output.xml' located in the root folder of the C: drive.

### ------------------EXAMPLE 4------------------
```powershell
PS:> $termgroup = Get-PnPTermGroup -GroupName Test
PS:> $termgroup | Export-PnPTermGroupToXml -Out c:\output.xml
```

Retrieves a termgroup and subsequently exports that term group to a the file named 'output.xml'

## PARAMETERS

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

### -FullTemplate
If specified, a full provisioning template structure will be returned

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
The ID or name of the termgroup

```yaml
Type: Id, Title or TermGroup
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: True
```

### -Out
File to export the data to.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)