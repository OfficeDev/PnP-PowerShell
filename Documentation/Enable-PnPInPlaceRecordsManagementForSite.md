---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Enable-PnPInPlaceRecordsManagementForSite

## SYNOPSIS
Enables in place records management for a site.

## SYNTAX 

```powershell
Enable-PnPInPlaceRecordsManagementForSite [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Enable-PnPInPlaceRecordsManagementForSite
```

The in place records management feature will be enabled and the in place record management will be enabled in all locations with record declaration allowed by all contributors and undeclaration by site admins

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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)