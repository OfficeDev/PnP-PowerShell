# Get-PnPTheme
Returns the current theme/composed look of the current web.
## Syntax
```powershell
Get-PnPTheme [-DetectCurrentComposedLook [<SwitchParameter>]]
             [-Web <WebPipeBind>]
```


## Returns
>OfficeDevPnP.Core.Entities.ThemeEntity

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|DetectCurrentComposedLook|SwitchParameter|False|Specify this switch to not use the PnP Provisioning engine based composed look information but try to detect the current composed look as is.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPTheme
```
Returns the current composed look of the current web.

### Example 2
```powershell
PS:> Get-PnPTheme -DetectCurrentComposedLook
```
Returns the current composed look of the current web, and will try to detect the currently applied composed look based upon the actual site. Without this switch the cmdlet will first check for the presence of a property bag variable called _PnP_ProvisioningTemplateComposedLookInfo that contains composed look information when applied through the provisioning engine or the Set-PnPTheme cmdlet.
