# Get-PnPAzureADManifestKeyCredentials
Creates the JSON snippet that is required for the manifest JSON file for Azure WebApplication / WebAPI apps
## Syntax
```powershell
Get-PnPAzureADManifestKeyCredentials -CertPath <String>
```


## Returns
>System.String

Outputs a JSON formatted string

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|CertPath|String|True|Specifies the path to the certificate like .\mycert.cer|
## Examples

### Example 1
```powershell
PS:> Get-PnPAzureADManifestKeyCredentials -CertPath .\mycert.cer
```
Output the JSON snippet which needs to be replaced in the application manifest file

### Example 2
```powershell
PS:> Get-PnPAzureADManifestKeyCredentials -CertPath .\mycert.cer | Set-Clipboard
```
Output the JSON snippet which needs to be replaced in the application manifest file and copies it to the clipboard
