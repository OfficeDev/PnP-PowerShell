---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPAuthenticationRealm

## SYNOPSIS
Returns the authentication realm

## SYNTAX 

```powershell
Get-PnPAuthenticationRealm [-Url <String>]
                           [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Gets the authentication realm for the current web

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPAuthenticationRealm
```

This will get the authentication realm for the current connected site

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPAuthenticationRealm -Url https://contoso.sharepoint.com
```

This will get the authentication realm for https://contoso.sharepoint.com

## PARAMETERS

### -Url
Specifies the URL of the site

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: 0
Accept pipeline input: True
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

### System.String

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)