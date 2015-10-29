#Get-SPOTenantSite
*Topic automatically generated on: 2015-10-13*

Office365 only: Uses the tenant API to retrieve site information.

##Syntax
```powershell
Get-SPOTenantSite [-Detailed [<SwitchParameter>]] [-IncludeOneDriveSites [<SwitchParameter>]] [-Force [<SwitchParameter>]] [-Url <String>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Detailed|SwitchParameter|False||
|Force|SwitchParameter|False||
|IncludeOneDriveSites|SwitchParameter|False||
|Url|String|False|The URL of the site|
##Examples

###Example 1
```powershell

PS:> Get-SPOTenantSite
```
Returns all site collections

###Example 2
```powershell

PS:> Get-SPOTenantSite -Url http://tenant.sharepoint.com/sites/projects
```
Returns information about the project site.
