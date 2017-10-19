---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Clear-PnPListItemAsRecord

## SYNOPSIS
Undeclares a list item as a record

## SYNTAX 

```powershell
Clear-PnPListItemAsRecord -Identity <ListItemPipeBind>
                          -List <ListPipeBind>
                          [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Clear-PnPListItemAsRecord -List "Documents" -Identity 4
```

Undeclares the document in the documents library with id 4 as a record

## PARAMETERS

### -Identity
The ID of the listitem, or actual ListItem object

```yaml
Type: ListItemPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: True
```

### -List
The ID, Title or Url of the list.

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
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