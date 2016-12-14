#Set-PnPUserProfileProperty
Office365 only: Uses the tenant API to retrieve site information.

You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this command. 

##Syntax
```powershell
Set-PnPUserProfileProperty -Values <String[]>
                           -Account <String>
                           -PropertyName <String>
```


```powershell
Set-PnPUserProfileProperty -Value <String>
                           -Account <String>
                           -PropertyName <String>
```


##Detailed Description
Requires a connection to a SharePoint Tenant Admin site.

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Account|String|True|The account of the user, formatted either as a login name, or as a claims identity, e.g. i:0#.f|membership|user@domain.com|
|PropertyName|String|True|The property to set, for instance SPS-Skills or SPS-Location|
|Value|String|True|The value to set in the case of a single value property|
|Values|String[]|True|The values set in the case of a multi value property, e.g. "Value 1","Value 2"|
##Examples

###Example 1
```powershell
PS:> Set-PnPUserProfileProperty -Account 'user@domain.com' -Property 'SPS-Location' -Value 'Stockholm'
```
Sets the SPS-Location property for the user as specified by the Account parameter

###Example 2
```powershell
PS:> Set-PnPUserProfileProperty -Account 'user@domain.com' -Property 'MyProperty' -Values 'Value 1','Value 2'
```
Sets the MyProperty multi value property for the user as specified by the Account parameter
