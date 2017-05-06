# Get-PnPTimeZoneId
Returns a time zone ID
## Syntax
```powershell
Get-PnPTimeZoneId [-Match <String>]
```


## Returns
>List<SharePointPnP.PowerShell.Commands.GetTimeZoneId+Zone>

Returns a list of matching zones. Use the ID property of the object for use in New-SPOTenantSite

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Match|String|False|A string to search for like 'Stockholm'|
## Examples

### Example 1
```powershell
PS:> Get-PnPTimeZoneId
```
This will return all time zone IDs in use by Office 365.

### Example 2
```powershell
PS:> Get-PnPTimeZoneId -Match Stockholm
```
This will return the time zone IDs for Stockholm
