---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# New-PnPTermSet

## SYNOPSIS
Creates a taxonomy term set

## SYNTAX 

```powershell
New-PnPTermSet -Name <String>
               -TermGroup <Id, Title or TermGroup>
               [-Id <Guid>]
               [-Lcid <Int>]
               [-Contact <String>]
               [-Description <String>]
               [-IsOpenForTermCreation [<SwitchParameter>]]
               [-IsNotAvailableForTagging [<SwitchParameter>]]
               [-Owner <String>]
               [-StakeHolders <String[]>]
               [-CustomProperties <Hashtable>]
               [-TermStore <Id, Name or Object>]
               [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> New-PnPTermSet -Name "Department" -TermGroup "Corporate"
```

Creates a new termset named "Department" in the group named "Corporate"

## PARAMETERS

### -Contact
An e-mail address for term suggestion and feedback. If left blank the suggestion feature will be disabled.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -CustomProperties


```yaml
Type: Hashtable
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Description
Descriptive text to help users understand the intended use of this term set.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Id
The Id to use for the term set; if not specified, or the empty GUID, a random GUID is generated and used.

```yaml
Type: Guid
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -IsNotAvailableForTagging
By default a term set is available to be used by end users and content editors of sites consuming this term set. Specify this switch to turn this off

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -IsOpenForTermCreation
When a term set is closed, only metadata managers can add terms to this term set. When it is open, users can add terms from a tagging application. Not specifying this switch will make the term set closed.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Lcid
The locale id to use for the term set. Defaults to the current locale id.

```yaml
Type: Int
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Name
The name of the termset.

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: True
```

### -Owner
The primary user or group of this term set.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -StakeHolders
People and groups in the organization that should be notified before major changes are made to the term set. You can enter multiple users or groups.

```yaml
Type: String[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -TermGroup
Name, id or actualy termgroup to create the termset in.

```yaml
Type: Id, Title or TermGroup
Parameter Sets: (All)

Required: True
Position: Named
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

### [Microsoft.SharePoint.Client.Taxonomy.TermSet](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termset.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)