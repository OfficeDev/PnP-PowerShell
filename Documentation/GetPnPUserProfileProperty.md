# Get-PnPUserProfileProperty
You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this cmdlet. 

## Syntax
```powershell
Get-PnPUserProfileProperty -Account <String[]>
```


## Detailed Description
Requires a connection to a SharePoint Tenant Admin site.

## Returns
>[Microsoft.SharePoint.Client.UserProfiles.PersonProperties](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.userprofiles.personproperties.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Account|String[]|True|The account of the user, formatted either as a login name, or as a claims identity, e.g. i:0#.f|membership|user@domain.com|
## Examples

### Example 1
```powershell
PS:> Get-PnPUserProfileProperty -Account 'user@domain.com'
```
Returns the profile properties for the specified user

### Example 2
```powershell
PS:> Get-PnPUserProfileProperty -Account 'user@domain.com','user2@domain.com'
```
Returns the profile properties for the specified users
