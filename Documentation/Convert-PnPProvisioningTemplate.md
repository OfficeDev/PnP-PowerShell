---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Convert-PnPProvisioningTemplate

## SYNOPSIS
Converts a provisioning template to an other schema version

## SYNTAX 

```powershell
Convert-PnPProvisioningTemplate -Path <String>
                                [-Out <String>]
                                [-Encoding <Encoding>]
                                [-Force [<SwitchParameter>]]
                                [-ToSchema <XMLPnPSchemaVersion>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Convert-PnPProvisioningTemplate -Path template.xml
```

Converts a provisioning template to the latest schema and outputs the result to current console.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Convert-PnPProvisioningTemplate -Path template.xml -Out newtemplate.xml
```

Converts a provisioning template to the latest schema and outputs the result the newtemplate.xml file.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Convert-PnPProvisioningTemplate -Path template.xml -Out newtemplate.xml -ToSchema V201512
```

Converts a provisioning template to the latest schema using the 201512 schema and outputs the result the newtemplate.xml file.

## PARAMETERS

### -Encoding
The encoding type of the XML file, Unicode is default

```yaml
Type: Encoding
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Force
Overwrites the output file if it exists

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Out
Filename to write to, optionally including full path

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Path
Path to the xml file containing the provisioning template

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -ToSchema
The schema of the output to use, defaults to the latest schema

```yaml
Type: XMLPnPSchemaVersion
Parameter Sets: (All)

Required: False
Position: 1
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)[Encoding](https://msdn.microsoft.com/en-us/library/system.text.encoding_properties.aspx)