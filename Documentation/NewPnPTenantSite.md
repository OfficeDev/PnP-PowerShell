# New-PnPTenantSite
Creates a new site collection for the current tenant
## Syntax
```powershell
New-PnPTenantSite -Title <String>
                  -Url <String>
                  -Owner <String>
                  -TimeZone <Int>
                  [-Description <String>]
                  [-Lcid <UInt32>]
                  [-Template <String>]
                  [-ResourceQuota <Double>]
                  [-ResourceQuotaWarningLevel <Double>]
                  [-StorageQuota <Int>]
                  [-StorageQuotaWarningLevel <Int>]
                  [-RemoveDeletedSite [<SwitchParameter>]]
                  [-Wait [<SwitchParameter>]]
                  [-Force [<SwitchParameter>]]
```


## Detailed Description
The New-PnPTenantSite cmdlet creates a new site collection for the current company. However, creating a new SharePoint
Online site collection fails if a deleted site with the same URL exists in the Recycle Bin. If you want to use this command for an on-premises farm, please refer to http://blogs.msdn.com/b/vesku/archive/2014/06/09/provisioning-site-collections-using-sp-app-model-in-on-premises-with-just-csom.aspx 

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Owner|String|True|Specifies the user name of the site collection's primary owner. The owner must be a user instead of a security group or an email-enabled security group.|
|TimeZone|Int|True|Use Get-PnPTimeZoneId to retrieve possible timezone values|
|Title|String|True|Specifies the title of the new site collection|
|Url|String|True|Specifies the full URL of the new site collection. It must be in a valid managed path in the company's site. For example, for company contoso, valid managed paths are https://contoso.sharepoint.com/sites and https://contoso.sharepoint.com/teams.|
|Description|String|False|Specifies the description of the new site collection|
|Force|SwitchParameter|False|Do not ask for confirmation.|
|Lcid|UInt32|False|Specifies the language of this site collection. For more information, see Locale IDs Assigned by Microsoft: https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.splanguage.lcid.aspx|
|RemoveDeletedSite|SwitchParameter|False|Specifies if any existing site with the same URL should be removed from the recycle bin|
|ResourceQuota|Double|False|Specifies the quota for this site collection in Sandboxed Solutions units. This value must not exceed the company's aggregate available Sandboxed Solutions quota. The default value is 0. For more information, see Resource Usage Limits on Sandboxed Solutions in SharePoint 2010 : http://msdn.microsoft.com/en-us/library/gg615462.aspx.|
|ResourceQuotaWarningLevel|Double|False|Specifies the warning level for the resource quota. This value must not exceed the value set for the ResourceQuota parameter|
|StorageQuota|Int|False|Specifies the storage quota for this site collection in megabytes. This value must not exceed the company's available quota.|
|StorageQuotaWarningLevel|Int|False|Specifies the warning level for the storage quota in megabytes. This value must not exceed the values set for the StorageQuota parameter|
|Template|String|False|Specifies the site collection template type. Use the Get-PnPWebTemplate cmdlet to get the list of valid templates. If no template is specified, one can be added later. The Template and LocaleId parameters must be a valid combination as returned from the Get-PnPWebTemplates cmdlet.|
|Wait|SwitchParameter|False||
## Examples

### Example 1
```powershell
PS:> New-PnPTenantSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -Owner user@example.org -TimeZone 4 -Template STS#0
```
This will add a site collection with the title 'Contoso', the url 'https://tenant.sharepoint.com/sites/contoso', the timezone 'UTC+01:00',the owner 'user@example.org' and the template used will be STS#0, a TeamSite

### Example 2
```powershell
PS:> New-PnPTenantSite -Title Contoso -Url /sites/contososite -Owner user@example.org -TimeZone 4 -Template STS#0
```
This will add a site collection with the title 'Contoso', the url 'https://tenant.sharepoint.com/sites/contososite' of which the base part will be picked up from your current connection, the timezone 'UTC+01:00', the owner 'user@example.org' and the template used will be STS#0, a TeamSite
