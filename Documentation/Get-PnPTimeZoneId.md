---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPTimeZoneId

## SYNOPSIS
Returns a time zone ID

## SYNTAX 

```powershell
Get-PnPTimeZoneId [-Match <String>]
```

## DESCRIPTION
In order to create a new classic site you need to specify the timezone this site will use. Use the cmdlet to retrieve a list of possible values.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPTimeZoneId
```

This will return all time zone IDs in use by Office 365.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPTimeZoneId -Match Stockholm
```

This will return the time zone IDs for Stockholm

## PARAMETERS

### -Match
A string to search for like 'Stockholm'

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: 0
Accept pipeline input: False
```

## OUTPUTS

### List<SharePointPnP.PowerShell.Commands.GetTimeZoneId+Zone>

Returns a list of matching zones. Use the ID property of the object for use in New-SPOTenantSite

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)