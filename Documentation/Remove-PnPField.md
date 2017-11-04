---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPField

## SYNOPSIS
Removes a field from a list or a site

## SYNTAX 

```powershell
Remove-PnPField -Identity <FieldPipeBind>
                [-Force [<SwitchParameter>]]
                [-List <ListPipeBind>]
                [-Web <WebPipeBind>]
                [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPField -Identity "Speakers"
```

Removes the speakers field from the site columns

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPField -List "Demo list" -Identity "Speakers"
```

Removes the speakers field from the list Demo list

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
The field object or name to remove

```yaml
Type: FieldPipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -List
The list object or name where to remove the field from

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: False
Position: 1
Accept pipeline input: True
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