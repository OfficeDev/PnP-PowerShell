# Get-PnPSitePolicy
Retrieves all or a specific site policy
## Syntax
```powershell
Get-PnPSitePolicy [-AllAvailable [<SwitchParameter>]]
                  [-Name <String>]
                  [-Web <WebPipeBind>]
```


## Returns
>OfficeDevPnP.Core.Entities.SitePolicyEntity

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AllAvailable|SwitchParameter|False|Retrieve all available site policies|
|Name|String|False|Retrieves a site policy with a specific name|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPSitePolicy
```
Retrieves the current applied site policy.

### Example 2
```powershell
PS:> Get-PnPSitePolicy -AllAvailable
```
Retrieves all available site policies.

### Example 3
```powershell
PS:> Get-PnPSitePolicy -Name "Contoso HBI"
```
Retrieves an available site policy with the name "Contoso HBI".
