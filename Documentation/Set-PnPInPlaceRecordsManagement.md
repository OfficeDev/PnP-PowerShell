---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPInPlaceRecordsManagement

## SYNOPSIS
Activates or deactivates in the place records management feature.

## SYNTAX 

### On
```powershell
Set-PnPInPlaceRecordsManagement -On [<SwitchParameter>]
                                [-Web <WebPipeBind>]
```

### Off
```powershell
Set-PnPInPlaceRecordsManagement -Off [<SwitchParameter>]
                                [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPInPlaceRecordsManagement -On
```

Activates In Place Records Management

## PARAMETERS

### -Off
Turn records management off

```yaml
Type: SwitchParameter
Parameter Sets: Off

Required: True
Position: 0
Accept pipeline input: False
```

### -On
Turn records management on

```yaml
Type: SwitchParameter
Parameter Sets: On

Required: True
Position: 0
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