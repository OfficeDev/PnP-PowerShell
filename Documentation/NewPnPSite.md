# New-PnPSite
BETA: This cmdlet is using early release APIs. Notice that functionality and parameters can change. Creates a new site collection
>*Only available for SharePoint Online*
## Syntax
```powershell
New-PnPSite -Type <SiteType>
            -Title <String>
            -Url <String>
            [-Description <String>]
            [-Classification <String>]
            [-AllowFileSharingForGuestUsers [<SwitchParameter>]]
            [-SiteDesign <CommunicationSiteDesign>]
            [-Lcid <UInt32>]
```


```powershell
New-PnPSite -Type <SiteType>
            -Title <String>
            -Alias <String>
            [-Description <String>]
            [-Classification <String>]
            [-IsPublic <String>]
```


```powershell
New-PnPSite -Type <SiteType>
            -Title <String>
            -Url <String>
            -SiteDesignId <GuidPipeBind>
            [-Description <String>]
            [-Classification <String>]
            [-AllowFileSharingForGuestUsers [<SwitchParameter>]]
            [-Lcid <UInt32>]
```


## Detailed Description
The New-PnPSite cmdlet creates a new site collection for the current tenant. Currently only 'modern' sites like Communication Site and the Modern Team Site are supported. If you want to create a classic site, use New-PnPTenantSite.

## Returns
>System.String

Returns the url of the newly created site collection

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Alias|String|True|Specifies the alias of the new site collection|
|SiteDesignId|GuidPipeBind|True|Specifies the site design id to use for the new site collection. If specified will override SiteDesign|
|Title|String|True|Specifies the title of the new site collection|
|Type|SiteType|True|@Specifies with type of site to create.|
|Url|String|True|Specifies the full url of the new site collection|
|AllowFileSharingForGuestUsers|SwitchParameter|False|Specifies if guest users can share files in the new site collection|
|Classification|String|False|Specifies the classification of the new site collection|
|Description|String|False|Specifies the description of the new site collection|
|IsPublic|String|False|Specifies if new site collection is public. Defaults to false.|
|Lcid|UInt32|False|Specifies the language of the new site collection. Defaults to the current language of the web connected to.|
|SiteDesign|CommunicationSiteDesign|False|Specifies the site design of the new site collection. Defaults to 'Topic'|
## Examples

### Example 1
```powershell
PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso
```
This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'

### Example 2
```powershell
PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -SiteDesign Showcase
```
This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'. It will use the 'Showcase' design for the site.

### Example 3
```powershell
PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -SiteDesignId ae2349d5-97d6-4440-94d1-6516b72449ac
```
This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'. It will use the specified custom site design for the site.

### Example 4
```powershell
PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -Classification "HBI"
```
This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'. The classification for the site will be set to "HBI"

### Example 5
```powershell
PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -AllowFileSharingForGuestUsers
```
This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'. File sharing for guest users will be enabled.

### Example 6
```powershell
PS:> New-PnPSite -Type TeamSite -Title Contoso -Alias contoso
```
This will create a new Modern Team Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'.

### Example 7
```powershell
PS:> New-PnPSite -Type TeamSite -Title Contoso -Alias contoso -IsPublic
```
This will create a new Modern Team Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso' and sets the site to public.
