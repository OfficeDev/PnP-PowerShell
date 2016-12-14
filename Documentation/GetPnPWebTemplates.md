#Get-PnPWebTemplates
Office365 only: Returns the available web templates.
##Syntax
```powershell
Get-PnPWebTemplates [-Lcid <UInt32>]
                    [-CompatibilityLevel <Int32>]
```


##Returns
>[Microsoft.Online.SharePoint.TenantAdministration.SPOTenantWebTemplateCollection](https://msdn.microsoft.com/en-us/library/microsoft.online.sharepoint.tenantadministration.spotenantwebtemplatecollection.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|CompatibilityLevel|Int32|False|The version of SharePoint|
|Lcid|UInt32|False|The language ID. For instance: 1033 for English|
##Examples

###Example 1
```powershell
PS:> Get-PnPWebTemplates
```


###Example 2
```powershell
PS:> Get-PnPWebTemplates -LCID 1033
```
Returns all webtemplates for the Locale with ID 1033 (English)

###Example 3
```powershell
PS:> Get-PnPWebTemplates -CompatibilityLevel 15
```
Returns all webtemplates for the compatibility level 15
