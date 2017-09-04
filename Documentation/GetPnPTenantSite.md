# Get-PnPTenantSite
Uses the tenant API to retrieve site information.
>*Only available for SharePoint Online*
## Syntax
```powershell
Get-PnPTenantSite [-Template <String>]
                  [-Detailed [<SwitchParameter>]]
                  [-IncludeOneDriveSites [<SwitchParameter>]]
                  [-Force [<SwitchParameter>]]
                  [-WebTemplate <String>]
                  [-Filter <String>]
                  [-Url <String>]
```


## Returns
>[Microsoft.Online.SharePoint.TenantAdministration.SiteProperties](https://msdn.microsoft.com/en-us/library/microsoft.online.sharepoint.tenantadministration.siteproperties.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Detailed|SwitchParameter|False|By default, not all returned attributes are populated. This switch populates all attributes. It can take several seconds to run. Without this, some attributes will show default values that may not be correct.|
|Filter|String|False|Specifies the script block of the server-side filter to apply. See https://technet.microsoft.com/en-us/library/fp161380.aspx|
|Force|SwitchParameter|False|When the switch IncludeOneDriveSites is used, this switch ignores the question shown that the command can take a long time to execute|
|IncludeOneDriveSites|SwitchParameter|False|By default, the OneDrives are not returned. This switch includes all OneDrives.|
|Template|String|False|By default, all sites will be return. Specify a template value alike 'STS#0' here to filter on the template|
|Url|String|False|The URL of the site|
|WebTemplate|String|False|Limit results to a specific web template name.|
## Examples

### Example 1
```powershell
PS:> Get-PnPTenantSite
```
Returns all site collections

### Example 2
```powershell
PS:> Get-PnPTenantSite -Url http://tenant.sharepoint.com/sites/projects
```
Returns information about the project site.

### Example 3
```powershell
PS:> Get-PnPTenantSite -Detailed
```
Returns all sites with the full details of these sites

### Example 4
```powershell
PS:> Get-PnPTenantSite -IncludeOneDriveSites
```
Returns all sites including all OneDrive 4 Business sites

### Example 5
```powershell
PS:> Get-PnPTenantSite -IncludeOneDriveSites -Filter "Url -like '-my.sharepoint.com/personal/'"
```
Returns all OneDrive for Business sites.

### Example 6
```powershell
PS:> Get-PnPTenantSite -WebTemplate SITEPAGEPUBLISHING#0
```
Returns all Communication sites

### Example 7
```powershell
PS:> Get-PnPTenantSite -Filter "Url -like 'sales'" 
```
Returns all sites including 'sales' in the url.
