#Set-SPOTenantSite
Office365 only: Uses the tenant API to set site information.
##Syntax
```powershell
Set-SPOTenantSite [-Title <String>] [-Sharing <Nullable`1>] [-StorageMaximumLevel <Nullable`1>] [-StorageWarningLevel <Nullable`1>] [-UserCodeMaximumLevel <Nullable`1>] [-UserCodeWarningLevel <Nullable`1>] [-AllowSelfServiceUpgrade <Nullable`1>] [-Url <String>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AllowSelfServiceUpgrade|Nullable`1|False||
|Sharing|Nullable`1|False||
|StorageMaximumLevel|Nullable`1|False||
|StorageWarningLevel|Nullable`1|False||
|Title|String|False||
|Url|String|False|The URL of the site|
|UserCodeMaximumLevel|Nullable`1|False||
|UserCodeWarningLevel|Nullable`1|False||
##Examples

###Example 1
```powershell
PS:> Set-SPOTenantSite -Url https://contoso.sharepoint.com -Title 'Contoso Website' -Sharing Disabled
```
This will set the title of the site collection with the URL 'https://contoso.sharepoint.com' to 'Contoso Website' and disable sharing on this site collection.
