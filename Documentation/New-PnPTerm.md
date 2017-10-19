---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# New-PnPTerm

## SYNOPSIS
Creates a taxonomy term

## SYNTAX 

```powershell
New-PnPTerm -Name <String>
            -TermGroup <Id, Title or TermGroup>
            -TermSet <Id, Title or TaxonomyItem>
            [-Id <Guid>]
            [-Lcid <Int>]
            [-Description <String>]
            [-CustomProperties <Hashtable>]
            [-LocalCustomProperties <Hashtable>]
            [-TermStore <Id, Name or Object>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> New-PnPTerm -TermSet "Departments" -TermGroup "Corporate" -Name "Finance"
```

Creates a new taxonomy term named "Finance" in the termset Departments which is located in the "Corporate" termgroup

## PARAMETERS

### -CustomProperties
Custom Properties

```yaml
Type: Hashtable
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Description
Descriptive text to help users understand the intended use of this term.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Id
The Id to use for the term; if not specified, or the empty GUID, a random GUID is generated and used.

```yaml
Type: Guid
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Lcid
The locale id to use for the term. Defaults to the current locale id.

```yaml
Type: Int
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -LocalCustomProperties
Custom Properties

```yaml
Type: Hashtable
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Name
The name of the term.

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: True
```

### -TermGroup
The termgroup to create the term in.

```yaml
Type: Id, Title or TermGroup
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: True
```

### -TermSet
The termset to add the term to.

```yaml
Type: Id, Title or TaxonomyItem
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
Aliases: TermStoreName

Required: False
Position: Named
Accept pipeline input: False
```

## OUTPUTS

### [Microsoft.SharePoint.Client.Taxonomy.Term](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.term.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)