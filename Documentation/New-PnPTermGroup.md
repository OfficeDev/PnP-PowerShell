---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# New-PnPTermGroup

## SYNOPSIS
Creates a taxonomy term group

## SYNTAX 

```powershell
New-PnPTermGroup -Name <String>
                 [-Id <Guid>]
                 [-Description <String>]
                 [-TermStore <Id, Name or Object>]
                 [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> New-PnPTermGroup -GroupName "Countries"
```

Creates a new taxonomy term group named "Countries"

## PARAMETERS

### -Description
Description to use for the term group.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Id
GUID to use for the term group; if not specified, or the empty GUID, a random GUID is generated and used.

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: GroupId

Required: False
Position: Named
Accept pipeline input: False
```

### -Name
Name of the taxonomy term group to create.

```yaml
Type: String
Parameter Sets: (All)
Aliases: GroupName

Required: True
Position: Named
Accept pipeline input: True
```

### -TermStore
Term store to add the group to; if not specified the default term store is used.

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