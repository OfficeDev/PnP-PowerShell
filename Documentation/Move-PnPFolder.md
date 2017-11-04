---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Move-PnPFolder

## SYNOPSIS
Move a folder to another location in the current web

## SYNTAX 

```powershell
Move-PnPFolder -Folder <String>
               -TargetFolder <String>
               [-Web <WebPipeBind>]
               [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Move-PnPFolder -Folder Documents/Reports -TargetFolder 'Archived Reports'
```

This will move the folder Reports in the Documents library to the 'Archived Reports' library

### ------------------EXAMPLE 2------------------
```powershell
PS:> Move-PnPFolder -Folder 'Shared Documents/Reports/2016/Templates' -TargetFolder 'Shared Documents/Reports'
```

This will move the folder Templates to the new location in 'Shared Documents/Reports'

## PARAMETERS

### -Folder
The folder to move

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -TargetFolder
The new parent location to which the folder should be moved to

```yaml
Type: String
Parameter Sets: (All)

Required: True
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

### -Web
The GUID, server relative url (i.e. /sites/team1) or web instance of the web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)