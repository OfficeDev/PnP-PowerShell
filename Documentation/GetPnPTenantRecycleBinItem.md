# Get-PnPTenantRecycleBinItem
Returns the items in the tenant scoped recycle bin
## Detailed Description
This command will return all the items in the tenant recycle bin for the Office 365 tenant you're connected to. Be sure to connect to the SharePoint Online Admin endpoint (https://tenant-admin.sharepoint.com) in order for this command to work.

## Returns
>[Microsoft.Online.SharePoint.TenantAdministration.DeletedSiteProperties](https://msdn.microsoft.com/en-us/library/microsoft.online.sharepoint.tenantadministration.deletedsiteproperties.aspx)

## Examples

### Example 1
```powershell
PS:> Get-PnPTenantRecycleBinItem
```
Returns all site collections in the tenant scoped recycle bin
