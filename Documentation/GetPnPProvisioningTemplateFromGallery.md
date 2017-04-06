# Get-PnPProvisioningTemplateFromGallery
Retrieves or searches provisioning templates from the PnP Template Gallery
## Syntax
```powershell
Get-PnPProvisioningTemplateFromGallery [-Identity <Guid>]
                                       [-Path <String>]
                                       [-Force [<SwitchParameter>]]
```


```powershell
Get-PnPProvisioningTemplateFromGallery [-Search <String>]
                                       [-TargetPlatform <TargetPlatform>]
                                       [-TargetScope <TargetScope>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False||
|Identity|Guid|False||
|Path|String|False||
|Search|String|False||
|TargetPlatform|TargetPlatform|False||
|TargetScope|TargetScope|False||
## Examples

### Example 1
```powershell
Get-PnPProvisioningTemplateFromGallery
```
Retrieves all templates from the gallery

### Example 2
```powershell
Get-PnPProvisioningTemplateFromGallery -Search "Data"
```
Searches for a templates containing the word 'Data' in the Display Name

### Example 3
```powershell
Get-PnPProvisioningTemplateFromGallery -Identity ae925674-8aa6-438b-acd0-d2699a022edd
```
Retrieves a template with the specified ID

### Example 4
```powershell
$template = Get-PnPProvisioningTemplateFromGallery -Identity ae925674-8aa6-438b-acd0-d2699a022edd
Apply-PnPProvisioningTemplate -InputInstance $template
```
Retrieves a template with the specified ID and applies it to the site.

### Example 5
```powershell
$template = Get-PnPProvisioningTemplateFromGallery -Identity ae925674-8aa6-438b-acd0-d2699a022edd -Path c:\temp
```
Retrieves a template with the specified ID and saves the template to the specified path
