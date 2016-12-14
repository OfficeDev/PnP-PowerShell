#New-PnPTenantSite
Creates a new site collection for the current tenant
##Syntax
```powershell
New-PnPTenantSite -Title <String>
                  -Url <String>
                  [-Description <String>]
                  -Owner <String>
                  [-Lcid <UInt32>]
                  [-Template <String>]
                  -TimeZone <Int32>
                  [-ResourceQuota <Double>]
                  [-ResourceQuotaWarningLevel <Double>]
                  [-StorageQuota <Int64>]
                  [-StorageQuotaWarningLevel <Int64>]
                  [-RemoveDeletedSite [<SwitchParameter>]]
                  [-Wait [<SwitchParameter>]]
```


##Detailed Description
The New-PnPTenantSite cmdlet creates a new site collection for the current company. However, creating a new SharePoint
Online site collection fails if a deleted site with the same URL exists in the Recycle Bin. If you want to use this command for an on-premises farm, please refer to http://blogs.msdn.com/b/vesku/archive/2014/06/09/provisioning-site-collections-using-sp-app-model-in-on-premises-with-just-csom.aspx 

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Description|String|False|Specifies the description of the new site collection|
|Lcid|UInt32|False|Specifies the language of this site collection. For more information, see Locale IDs Assigned by Microsoft: http://go.microsoft.com/fwlink/p/?LinkId=242911Id=242911.|
|Owner|String|True|Specifies the user name of the site collection's primary owner. The owner must be a user instead of a security group or an email-enabled security group.|
|RemoveDeletedSite|SwitchParameter|False|Specifies if any existing site with the same URL should be removed from the recycle bin|
|ResourceQuota|Double|False|Specifies the quota for this site collection in Sandboxed Solutions units. This value must not exceed the company's aggregate available Sandboxed Solutions quota. The default value is 0. For more information, see Resource Usage Limits on Sandboxed Solutions in SharePoint 2010 : http://msdn.microsoft.com/en-us/library/gg615462.aspx.|
|ResourceQuotaWarningLevel|Double|False|Specifies the warning level for the resource quota. This value must not exceed the value set for the ResourceQuota parameter|
|StorageQuota|Int64|False|Specifies the storage quota for this site collection in megabytes. This value must not exceed the company's available quota.|
|StorageQuotaWarningLevel|Int64|False|Specifies the warning level for the storage quota in megabytes. This value must not exceed the values set for the StorageQuota parameter|
|Template|String|False|Specifies the site collection template type. Use the Get-PnPWebTemplate cmdlet to get the list of valid templates. If no template is specified, one can be added later. The Template and LocaleId parameters must be a valid combination as returned from the Get-PnPWebTemplates cmdlet.|
|TimeZone|Int32|True|Use Get-PnPTimeZoneId to retrieve possible timezone values|
|Title|String|True|Specifies the title of the new site collection|
|Url|String|True|Specifies the full URL of the new site collection. It must be in a valid managed path in the company's site. For example, for company contoso, valid managed paths are https://contoso.sharepoint.com/sites and https://contoso.sharepoint.com/teams.|
|Wait|SwitchParameter|False||
##Examples

###Example 1
```powershell
PS:> New-PnPTenantSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -Owner user@example.org -TimeZone 4
```
This will add a site collection with the title 'Contoso', the url 'https://tenant.sharepoint.com/sites/contoso', the timezone 'UTC+01:00' and the owner 'user@example.org'
