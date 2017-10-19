---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPHealthScore

## SYNOPSIS
Retrieves the healthscore

## DESCRIPTION
Retrieves the current health score value of the server which is a value between 0 and 10. Lower is better.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPHealthScore
```

This will retrieve the current health score of the server.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPHealthScore -Url https://contoso.sharepoint.com
```

This will retrieve the current health score for the url https://contoso.sharepoint.com.

## OUTPUTS

### System.Int32

Returns a int value representing the current health score value of the server.

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)