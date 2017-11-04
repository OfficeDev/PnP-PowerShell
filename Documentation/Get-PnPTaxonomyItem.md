---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPTaxonomyItem

## SYNOPSIS
Returns a taxonomy item

## SYNTAX 

```powershell
Get-PnPTaxonomyItem -TermPath <String>
                    [-Connection <SPOnlineConnection>]
```

## PARAMETERS

### -TermPath
The path, delimited by | of the taxonomy item to retrieve, alike GROUPLABEL|TERMSETLABEL|TERMLABEL

```yaml
Type: String
Parameter Sets: (All)
Aliases: Term

Required: True
Position: 0
Accept pipeline input: True
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

## OUTPUTS

### [Microsoft.SharePoint.Client.Taxonomy.TaxonomyItem](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.taxonomyitem.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)