---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Import-PnPTermSet

## SYNOPSIS
Imports a taxonomy term set from a file in the standard format.

## SYNTAX 

```powershell
Import-PnPTermSet -GroupName <String>
                  -Path <String>
                  [-TermSetId <Guid>]
                  [-SynchronizeDeletions [<SwitchParameter>]]
                  [-IsOpen <Nullable`1>]
                  [-Contact <String>]
                  [-Owner <String>]
                  [-TermStoreName <String>]
                  [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
The format of the file is the same as that used by the import function in the web interface. A sample file can be obtained from the web interface.

This is a CSV file, with the following headings:

  Term Set Name,Term Set Description,LCID,Available for Tagging,Term Description,Level 1 Term,Level 2 Term,Level 3 Term,Level 4 Term,Level 5 Term,Level 6 Term,Level 7 Term

The first data row must contain the Term Set Name, Term Set Description, and LCID, and should also contain the first term. 

It is recommended that a fixed GUID be used as the termSetId, to allow the term set to be easily updated (so do not pass Guid.Empty).

In contrast to the web interface import, this is not a one-off import but runs synchronisation logic allowing updating of an existing Term Set. When synchronising, any existing terms are matched (with Term Description and Available for Tagging updated as necessary), any new terms are added in the correct place in the hierarchy, and (if synchroniseDeletions is set) any terms not in the imported file are removed.

The import file also supports an expanded syntax for the Term Set Name and term names (Level 1 Term, Level 2 Term, etc). These columns support values with the format 'Name | GUID', with the name and GUID separated by a pipe character (note that the pipe character is invalid to use within a taxomony item name). This expanded syntax is not required, but can be used to ensure all terms have fixed IDs.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Import-PnPTermSet -GroupName 'Standard Terms' -Path 'C:\\Temp\\ImportTermSet.csv' -SynchronizeDeletions
```

Creates (or updates) the term set specified in the import file, in the group specified, removing any existing terms not in the file.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Import-PnPTermSet -TermStoreName 'My Term Store' -GroupName 'Standard Terms' -Path 'C:\\Temp\\ImportTermSet.csv' -TermSetId '{15A98DB6-D8E2-43E6-8771-066C1EC2B8D8}' 
```

Creates (or updates) the term set specified in the import file, in the term store and group specified, using the specified ID.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Import-PnPTermSet -GroupName 'Standard Terms' -Path 'C:\\Temp\\ImportTermSet.csv' -IsOpen $true -Contact 'user@example.org' -Owner 'user@example.org'
```

Creates (or updates) the term set specified in the import file, setting the IsOpen, Contact, and Owner properties as specified.

## PARAMETERS

### -Contact
Contact for the term set; if not specified, the existing setting is retained.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -GroupName
Group to import the term set to; an error is returned if the group does not exist.

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -IsOpen
Whether the term set should be marked open; if not specified, then the existing setting is not changed.

```yaml
Type: Nullable`1
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Owner
Owner for the term set; if not specified, the existing setting is retained.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Path
Local path to the file containing the term set to import, in the standard format (as the 'sample import file' available in the Term Store Administration).

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -SynchronizeDeletions
If specified, the import will remove any terms (and children) previously in the term set but not in the import file; default is to leave them.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -TermSetId
GUID to use for the term set; if not specified, or the empty GUID, a random GUID is generated and used.

```yaml
Type: Guid
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -TermStoreName
Term store to import into; if not specified the default term store is used.

```yaml
Type: String
Parameter Sets: (All)

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