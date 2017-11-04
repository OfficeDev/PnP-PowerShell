---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPEventReceiver

## SYNOPSIS
Return registered eventreceivers

## SYNTAX 

### List
```powershell
Get-PnPEventReceiver [-List <ListPipeBind>]
                     [-Identity <EventReceiverPipeBind>]
                     [-Web <WebPipeBind>]
                     [-Connection <SPOnlineConnection>]
```

### 
```powershell
Get-PnPEventReceiver [-Identity <EventReceiverPipeBind>]
                     [-Web <WebPipeBind>]
                     [-Includes <String[]>]
                     [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Returns all registered or a specific eventreceiver

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPEventReceiver
```

This will return all registered event receivers on the current web

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPEventReceiver -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22
```

This will return the event receiver with the provided ReceiverId "fb689d0e-eb99-4f13-beb3-86692fd39f22" from the current web

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPEventReceiver -Identity MyReceiver
```

This will return the event receiver with the provided ReceiverName "MyReceiver" from the current web

### ------------------EXAMPLE 4------------------
```powershell
PS:> Get-PnPEventReceiver -List "ProjectList"
```

This will return all registered event receivers in the provided "ProjectList" list

### ------------------EXAMPLE 5------------------
```powershell
PS:> Get-PnPEventReceiver -List "ProjectList" -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22
```

This will return the event receiver in the provided "ProjectList" list with with the provided ReceiverId "fb689d0e-eb99-4f13-beb3-86692fd39f22"

### ------------------EXAMPLE 6------------------
```powershell
PS:> Get-PnPEventReceiver -List "ProjectList" -Identity MyReceiver
```

This will return the event receiver in the "ProjectList" list with the provided ReceiverName "MyReceiver"

## PARAMETERS

### -Identity
The Guid of the event receiver

```yaml
Type: EventReceiverPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: True
```

### -Includes
Specify properties to include when retrieving objects from the server.

```yaml
Type: String[]
Parameter Sets: 

Required: False
Position: 0
Accept pipeline input: False
```

### -List
The list object from which to get the event receiver object

```yaml
Type: ListPipeBind
Parameter Sets: List

Required: False
Position: Named
Accept pipeline input: False
```

### -Web
The web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
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

## OUTPUTS

### [Microsoft.SharePoint.Client.EventReceiverDefinition](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.eventreceiverdefinition.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)