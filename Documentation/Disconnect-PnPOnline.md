---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Disconnect-PnPOnline

## SYNOPSIS
Disconnects the context

## SYNTAX 

```powershell
Disconnect-PnPOnline [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Disconnects the current context and requires you to build up a new connection in order to use the Cmdlets again. Using Connect-PnPOnline to connect to a different site has the same effect.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Disconnect-PnPOnline
```

This will disconnect you from the server.

## PARAMETERS

### -Connection
Connection to be used by cmdlet

```yaml
Type: SPOnlineConnection
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)