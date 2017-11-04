---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPAuditing

## SYNOPSIS
Set Auditing setting for a site

## SYNTAX 

### EnableAll
```powershell
Set-PnPAuditing [-EnableAll [<SwitchParameter>]]
                [-RetentionTime <Int>]
                [-TrimAuditLog [<SwitchParameter>]]
                [-Connection <SPOnlineConnection>]
```

### DisableAll
```powershell
Set-PnPAuditing [-DisableAll [<SwitchParameter>]]
                [-Connection <SPOnlineConnection>]
```

### Other
```powershell
Set-PnPAuditing [-RetentionTime <Int>]
                [-TrimAuditLog [<SwitchParameter>]]
                [-EditItems [<SwitchParameter>]]
                [-CheckOutCheckInItems [<SwitchParameter>]]
                [-MoveCopyItems [<SwitchParameter>]]
                [-DeleteRestoreItems [<SwitchParameter>]]
                [-EditContentTypesColumns [<SwitchParameter>]]
                [-SearchContent [<SwitchParameter>]]
                [-EditUsersPermissions [<SwitchParameter>]]
                [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPAuditing -EnableAll
```

Enables all auditing settings for the current site

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPAuditing -DisableAll
```

Disables all auditing settings for the current site
                    This also disables the automatic trimming of the audit log

### ------------------EXAMPLE 3------------------
```powershell
PS:> Set-PnPAuditing -RetentionTime 7
```

Sets the audit log trimming to 7 days, this also enables the automatic trimming of the audit log

### ------------------EXAMPLE 4------------------
```powershell
PS:> Set-PnPAuditing -TrimAuditLog
```

Enables the automatic trimming of the audit log

### ------------------EXAMPLE 5------------------
```powershell
PS:> Set-PnPAuditing -RetentionTime 7 -CheckOutCheckInItems -MoveCopyItems -SearchContent
```

Sets the audit log trimming to 7 days, this also enables the automatic trimming of the audit log.
                    Do auditing for:
                    - Checking out or checking in items
                    - Moving or copying items to another location in the site
                    - Searching site content

## PARAMETERS

### -CheckOutCheckInItems


```yaml
Type: SwitchParameter
Parameter Sets: Other

Required: False
Position: Named
Accept pipeline input: False
```

### -DeleteRestoreItems


```yaml
Type: SwitchParameter
Parameter Sets: Other

Required: False
Position: Named
Accept pipeline input: False
```

### -DisableAll


```yaml
Type: SwitchParameter
Parameter Sets: DisableAll

Required: False
Position: Named
Accept pipeline input: False
```

### -EditContentTypesColumns


```yaml
Type: SwitchParameter
Parameter Sets: Other

Required: False
Position: Named
Accept pipeline input: False
```

### -EditItems


```yaml
Type: SwitchParameter
Parameter Sets: Other

Required: False
Position: Named
Accept pipeline input: False
```

### -EditUsersPermissions


```yaml
Type: SwitchParameter
Parameter Sets: Other

Required: False
Position: Named
Accept pipeline input: False
```

### -EnableAll


```yaml
Type: SwitchParameter
Parameter Sets: EnableAll

Required: False
Position: Named
Accept pipeline input: False
```

### -MoveCopyItems


```yaml
Type: SwitchParameter
Parameter Sets: Other

Required: False
Position: Named
Accept pipeline input: False
```

### -RetentionTime


```yaml
Type: Int
Parameter Sets: Other

Required: False
Position: Named
Accept pipeline input: False
```

### -SearchContent


```yaml
Type: SwitchParameter
Parameter Sets: Other

Required: False
Position: Named
Accept pipeline input: False
```

### -TrimAuditLog


```yaml
Type: SwitchParameter
Parameter Sets: Other

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