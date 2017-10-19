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
                     [-Identity <GuidPipeBind>]
                     [-Web <WebPipeBind>]
```

### 
```powershell
Get-PnPEventReceiver [-Identity <GuidPipeBind>]
                     [-Web <WebPipeBind>]
                     [-Includes <String[]>]
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

This will return a specific registered event receiver from the current web

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPEventReceiver -List "ProjectList"
```

This will return all registered event receivers in the list with the name ProjectList

### ------------------EXAMPLE 4------------------
```powershell
PS:> Get-PnPEventReceiver -List "ProjectList" -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22
```

This will return a specific registered event receiver in the list with the name ProjectList

## PARAMETERS

### -Identity
The Guid of the event receiver on the list

```yaml
Type: GuidPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
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

## OUTPUTS

### [Microsoft.SharePoint.Client.EventReceiverDefinition](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.eventreceiverdefinition.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)