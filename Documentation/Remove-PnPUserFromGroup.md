---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPUserFromGroup

## SYNOPSIS
Removes a user from a group

## SYNTAX 

```powershell
Remove-PnPUserFromGroup -LoginName <String>
                        -Identity <GroupPipeBind>
                        [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPUserFromGroup -LoginName user@company.com -GroupName 'Marketing Site Members'
```

Removes the user user@company.com from the Group 'Marketing Site Members'

## PARAMETERS

### -Identity
A group object, an ID or a name of a group

```yaml
Type: GroupPipeBind
Parameter Sets: (All)
Aliases: GroupName

Required: True
Position: Named
Accept pipeline input: False
```

### -LoginName
A valid login name of a user (user@company.com)

```yaml
Type: String
Parameter Sets: (All)
Aliases: LogonName

Required: True
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