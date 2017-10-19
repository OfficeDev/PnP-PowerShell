---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPEventReceiver

## SYNOPSIS
Adds a new event receiver

## SYNTAX 

```powershell
Add-PnPEventReceiver -Name <String>
                     -Url <String>
                     -EventReceiverType <EventReceiverType>
                     -Synchronization <EventReceiverSynchronization>
                     [-List <ListPipeBind>]
                     [-SequenceNumber <Int>]
                     [-Force [<SwitchParameter>]]
                     [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPEventReceiver -List "ProjectList" -Name "TestEventReceiver" -Url https://yourserver.azurewebsites.net/eventreceiver.svc -EventReceiverType ItemAdded -Synchronization Asynchronous
```

This will add a new event receiver that is executed after an item has been added to the ProjectList list

## PARAMETERS

### -EventReceiverType
The type of the event receiver like ItemAdded, ItemAdding

```yaml
Type: EventReceiverType
Parameter Sets: (All)
Aliases: Type

Required: True
Position: Named
Accept pipeline input: False
```

### -Force
Overwrites the output file if it exists.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -List
The list object or name where the event receiver needs to be added

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Name
The name of the event receiver

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -SequenceNumber
The sequence number where this event receiver should be placed

```yaml
Type: Int
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Synchronization
The Synchronization type, Asynchronous or Synchronous

```yaml
Type: EventReceiverSynchronization
Parameter Sets: (All)
Aliases: Sync

Required: True
Position: Named
Accept pipeline input: False
```

### -Url
The URL of the event receiver web service

```yaml
Type: String
Parameter Sets: (All)

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

## OUTPUTS

### [Microsoft.SharePoint.Client.EventReceiverDefinition](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.eventreceiverdefinition.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)