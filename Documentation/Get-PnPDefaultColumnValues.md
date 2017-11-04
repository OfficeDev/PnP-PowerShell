---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPDefaultColumnValues

## SYNOPSIS
Gets the default column values for all folders in document library

## SYNTAX 

```powershell
Get-PnPDefaultColumnValues -List <ListPipeBind>
                           [-Web <WebPipeBind>]
                           [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Gets the default column values for a document library, per folder. Supports both text, people and taxonomy fields.

## PARAMETERS

### -List
The ID, Name or Url of the list.

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