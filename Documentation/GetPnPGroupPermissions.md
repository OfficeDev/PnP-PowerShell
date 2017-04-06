# Get-PnPGroupPermissions
Returns the permissions for a specific SharePoint group
## Syntax
```powershell
Get-PnPGroupPermissions -Identity <GroupPipeBind>
                        [-Web <WebPipeBind>]
```


## Returns
>[Microsoft.SharePoint.Client.RoleDefinitionBindingCollection](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.roledefinitionbindingcollection.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|GroupPipeBind|True|Get the permissions of a specific group by name|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPGroupPermissions -Identity 'My Site Members'
```
Returns the permissions for the SharePoint group with the name 'My Site Members'
