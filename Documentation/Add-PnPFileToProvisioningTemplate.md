---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPFileToProvisioningTemplate

## SYNOPSIS
Adds a file to a PnP Provisioning Template

## SYNTAX 

```powershell
Add-PnPFileToProvisioningTemplate -Path <String>
                                  -Source <String>
                                  -Folder <String>
                                  [-Container <String>]
                                  [-TemplateProviderExtensions <ITemplateProviderExtension[]>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source $sourceFilePath -Folder $targetFolder
```

Adds a file to an in-memory PnP Provisioning Template

### ------------------EXAMPLE 2------------------
```powershell
PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source $sourceFilePath -Folder $targetFolder -Container $container
```

Adds a file to an in-memory PnP Provisioning Template with a custom container for the file

## PARAMETERS

### -Container
The target Container for the file to add to the in-memory template, optional argument.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: 3
Accept pipeline input: False
```

### -Folder
The target Folder for the file to add to the in-memory template.

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 2
Accept pipeline input: False
```

### -Path
Filename of the .PNP Open XML provisioning template to read from, optionally including full path.

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: False
```

### -Source
The file to add to the in-memory template, optionally including full path.

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 1
Accept pipeline input: False
```

### -TemplateProviderExtensions
Allows you to specify ITemplateProviderExtension to execute while loading the template.

```yaml
Type: ITemplateProviderExtension[]
Parameter Sets: (All)

Required: False
Position: 4
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)