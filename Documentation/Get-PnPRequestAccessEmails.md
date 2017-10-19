---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Get-PnPRequestAccessEmails

## SYNOPSIS
Returns the request access e-mail addresses

## SYNTAX 

```powershell
Get-PnPRequestAccessEmails [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPRequestAccessEmails
```

This will return all the request access e-mail addresses for the current web

## PARAMETERS

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

### List<System.String>

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)