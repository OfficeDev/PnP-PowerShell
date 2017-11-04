---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPSiteClosure

## SYNOPSIS
Opens or closes a site which has a site policy applied

## SYNTAX 

```powershell
Set-PnPSiteClosure -State <ClosureState>
                   [-Web <WebPipeBind>]
                   [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPSiteClosure -State Open
```

This opens a site which has been closed and has a site policy applied.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPSiteClosure -State Closed
```

This closes a site which is open and has a site policy applied.

## PARAMETERS

### -State
The state of the site

```yaml
Type: ClosureState
Parameter Sets: (All)

Required: True
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