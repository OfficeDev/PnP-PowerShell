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
Remove-PnPEventReceiver -Identity <GuidPipeBind>
                        [-List <ListPipeBind>]
                        [-Force [<SwitchParameter>]]
                        [-Web <WebPipeBind>]
```

## DESCRIPTION
Removes/unregisters a specific eventreceiver

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPEventReceiver -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22
```

This will remove an event receiver with id fb689d0e-eb99-4f13-beb3-86692fd39f22 from the current web

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPEventReceiver -List ProjectList -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22
```

This will remove an event receiver with id fb689d0e-eb99-4f13-beb3-86692fd39f22 from the list with name "ProjectList"

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
The Guid of the event receiver on the list

```yaml
Type: GuidPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -List
The list object from where to get the event receiver object

```yaml
Type: ListPipeBind
Parameter Sets: List

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