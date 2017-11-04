---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPHomePage

## SYNOPSIS
Return the homepage

## SYNTAX 

```powershell
Get-PnPHomePage [-Web <WebPipeBind>]
                [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Returns the URL to the page set as home page

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPHomePage
```

Will return the URL of the home page of the web.

## PARAMETERS

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

## OUTPUTS

### System.String

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)