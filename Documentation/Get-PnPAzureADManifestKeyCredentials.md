---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPAzureADManifestKeyCredentials

## SYNOPSIS
Return the JSON Manifest snippet for Azure Apps

## SYNTAX 

```powershell
Get-PnPAzureADManifestKeyCredentials -CertPath <String>
```

## DESCRIPTION
Creates the JSON snippet that is required for the manifest JSON file for Azure WebApplication / WebAPI apps

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPAzureADManifestKeyCredentials -CertPath .\mycert.cer
```

Output the JSON snippet which needs to be replaced in the application manifest file

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPAzureADManifestKeyCredentials -CertPath .\mycert.cer | Set-Clipboard
```

Output the JSON snippet which needs to be replaced in the application manifest file and copies it to the clipboard

## PARAMETERS

### -CertPath
Specifies the path to the certificate like .\mycert.cer

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

## OUTPUTS

### System.String

Outputs a JSON formatted string

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)