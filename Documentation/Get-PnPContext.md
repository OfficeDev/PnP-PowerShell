---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPContext

## SYNOPSIS
Returns the current context

## DESCRIPTION
Returns a Client Side Object Model context

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> $ctx = Get-PnPContext
```

This will put the current context in the $ctx variable.

## OUTPUTS

### [Microsoft.SharePoint.Client.ClientContext](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.clientcontext.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)