---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPAppAuthAccessToken

## SYNOPSIS
Returns the access token

## SYNTAX 

```powershell
Get-PnPAppAuthAccessToken [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Returns the access token from the current client context (only works with App-Only authentication)

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> $accessToken = Get-PnPAppAuthAccessToken
```

This will put the access token from current context in the $accessToken variable. Will only work in App authentication flow (App+user or App-Only)

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

## OUTPUTS

### [System.String](https://msdn.microsoft.com/en-us/library/system.string.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)