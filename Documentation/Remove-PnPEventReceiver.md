---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPEventReceiver

## SYNOPSIS
Remove an eventreceiver

## SYNTAX 

### List
```powershell
Remove-PnPEventReceiver -Identity <EventReceiverPipeBind>
                        [-List <ListPipeBind>]
                        [-Force [<SwitchParameter>]]
                        [-Web <WebPipeBind>]
                        [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Removes/unregisters a specific eventreceiver

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPEventReceiver -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22
```

This will remove the event receiver with ReceiverId "fb689d0e-eb99-4f13-beb3-86692fd39f22" from the current web

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPEventReceiver -List ProjectList -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22
```

This will remove the event receiver with ReceiverId "fb689d0e-eb99-4f13-beb3-86692fd39f22" from the "ProjectList" list

### ------------------EXAMPLE 3------------------
```powershell
PS:> Remove-PnPEventReceiver -List ProjectList -Identity MyReceiver
```

This will remove the event receiver with ReceiverName "MyReceiver" from the "ProjectList" list

### ------------------EXAMPLE 4------------------
```powershell
PS:> Remove-PnPEventReceiver -List ProjectList
```

This will remove all event receivers from the "ProjectList" list

### ------------------EXAMPLE 5------------------
```powershell
PS:> Remove-PnPEventReceiver
```

This will remove all event receivers from the current site

### ------------------EXAMPLE 6------------------
```powershell
PS:> Get-PnPEventReceiver | ? ReceiverUrl -Like "*azurewebsites.net*" | Remove-PnPEventReceiver
```

This will remove all event receivers from the current site which are pointing to a service hosted on Azure Websites

## PARAMETERS

### -Force
Specifying the Force parameter will skip the confirmation question

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
The Guid of the event receiver on the list

```yaml
Type: EventReceiverPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: True
```

### -List
The list object from where to remove the event receiver object

```yaml
Type: ListPipeBind
Parameter Sets: List

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