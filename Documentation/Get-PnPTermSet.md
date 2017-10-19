---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPTermSet

## SYNOPSIS
Returns a taxonomy term set

## SYNTAX 

### 
```powershell
Get-PnPTermSet -TermGroup <Id, Title or TermGroup>
               [-Identity <Id, Name or Object>]
               [-TermStore <Id, Name or Object>]
               [-Includes <String[]>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPTermSet -TermGroup "Corporate"
```

Returns all termsets in the group "Corporate" from the site collection termstore

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPTermSet -Identity "Departments" -TermGroup "Corporate"
```

Returns the termset named "Departments" from the termgroup called "Corporate" from the site collection termstore

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPTermSet -Identity ab2af486-e097-4b4a-9444-527b251f1f8d -TermGroup "Corporate
```

Returns the termset with the given id from the termgroup called "Corporate" from the site collection termstore

## PARAMETERS

### -Identity
The Id or Name of a termset

```yaml
Type: Id, Name or Object
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
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

### -TermGroup
Name of the term group to check.

```yaml
Type: Id, Title or TermGroup
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -TermStore
Term store to check; if not specified the default term store is used.

```yaml
Type: Id, Name or Object
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

## OUTPUTS

### [Microsoft.SharePoint.Client.Taxonomy.TermSet](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termset.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)