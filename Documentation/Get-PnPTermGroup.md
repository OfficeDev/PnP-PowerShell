---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPTermGroup

## SYNOPSIS
Returns a taxonomy term group

## SYNTAX 

### 
```powershell
Get-PnPTermGroup [-TermStore <Id, Name or Object>]
                 [-Includes <String[]>]
                 [-Identity <Id, Title or TaxonomyItem>]
                 [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPTermGroup
```

Returns all Term Groups in the site collection termstore

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPTermGroup -Identity "Departments"
```

Returns the termgroup named "Departments" from the site collection termstore

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPTermGroup -Identity ab2af486-e097-4b4a-9444-527b251f1f8d
```

Returns the termgroup with the given ID from the site collection termstore

## PARAMETERS

### -Identity
Name of the taxonomy term group to retrieve.

```yaml
Type: Id, Title or TaxonomyItem
Parameter Sets: (All)
Aliases: GroupName

Required: False
Position: 0
Accept pipeline input: True
```

### -Includes
Specify properties to include when retrieving objects from the server.

```yaml
Type: String[]
Parameter Sets: 

Required: False
Position: 0
Accept pipeline input: False
```

### -TermStore
Term store to check; if not specified the default term store is used.

```yaml
Type: Id, Name or Object
Parameter Sets: (All)
Aliases: TermStoreName

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

## OUTPUTS

### [Microsoft.SharePoint.Client.Taxonomy.TermGroup](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termgroup.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)