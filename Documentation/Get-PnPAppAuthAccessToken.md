---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPAppAuthAccessToken

## SYNOPSIS
Returns the access token

## DESCRIPTION
Returns the access token from the current client context (only works with App-Only authentication)

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> $accessToken = Get-PnPAppAuthAccessToken
```

This will put the access token from current context in the $accessToken variable. Will only work in App authentication flow (App+user or App-Only)

## OUTPUTS

### [System.String](https://msdn.microsoft.com/en-us/library/system.string.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)