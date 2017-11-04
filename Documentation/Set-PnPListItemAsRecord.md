---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Set-PnPListItemAsRecord

## SYNOPSIS
Declares a list item as a record

## SYNTAX 

```powershell
Set-PnPListItemAsRecord -Identity <ListItemPipeBind>
                        -List <ListPipeBind>
                        [-DeclarationDate <DateTime>]
                        [-Web <WebPipeBind>]
                        [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPListItemAsRecord -List "Documents" -Identity 4
```

Declares the document in the documents library with id 4 as a record

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPListItemAsRecord -List "Documents" -Identity 4 -DeclarationDate $date
```

Declares the document in the documents library with id as a record

## PARAMETERS

### -DeclarationDate
The declaration date

```yaml
Type: DateTime
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

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