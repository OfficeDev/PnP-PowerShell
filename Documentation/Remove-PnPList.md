---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPList

## SYNOPSIS
Deletes a list

## SYNTAX 

```powershell
Remove-PnPList -Identity <ListPipeBind>
               [-Recycle [<SwitchParameter>]]
               [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPList -Identity Announcements
```

Removes the list named 'Announcements'. Asks for confirmation.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPList -Identity Announcements -Force
```

Removes the list named 'Announcements' without asking for confirmation.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Remove-PnPList -Title Announcements -Recycle
```

Removes the list named 'Announcements' and saves to the Recycle Bin

## PARAMETERS

### -Force
Specifying the Force parameter will skip the confirmation question.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
The ID or Title of the list.

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -Recycle


```yaml
Type: SwitchParameter
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