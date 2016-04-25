#Get-SPOTenantSite
Office365 only: Uses the tenant API to retrieve site information.

##Syntax
```powershell
Get-SPOTenantSite [-Detailed [<SwitchParameter>]] [-IncludeOneDriveSites [<SwitchParameter>]] [-Force [<SwitchParameter>]] [-Url <String>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Detailed|SwitchParameter|False|By default, not all returned attributes are populated. This switch populates all attributes. It can take several seconds to run. Without this, some attributes will show default values that may not be correct.|
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
