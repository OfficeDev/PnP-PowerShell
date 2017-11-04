---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# New-PnPProvisioningTemplateFromFolder

## SYNOPSIS
Generates a provisioning template from a given folder, including only files that are present in that folder

## SYNTAX 

```powershell
New-PnPProvisioningTemplateFromFolder [-Match <String>]
                                      [-ContentType <ContentTypePipeBind>]
                                      [-Properties <Hashtable>]
                                      [-AsIncludeFile [<SwitchParameter>]]
                                      [-Force [<SwitchParameter>]]
                                      [-Encoding <Encoding>]
                                      [-Out <String>]
                                      [-Folder <String>]
                                      [-TargetFolder <String>]
                                      [-Schema <XMLPnPSchemaVersion>]
                                      [-Web <WebPipeBind>]
                                      [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml
```

Creates an empty provisioning template, and includes all files in the current folder.

### ------------------EXAMPLE 2------------------
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp
```

Creates an empty provisioning template, and includes all files in the c:\temp folder.

### ------------------EXAMPLE 3------------------
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp -Match *.js
```

Creates an empty provisioning template, and includes all files with a JS extension in the c:\temp folder.

### ------------------EXAMPLE 4------------------
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp -Match *.js -TargetFolder "Shared Documents"
```

Creates an empty provisioning template, and includes all files with a JS extension in the c:\temp folder and marks the files in the template to be added to the 'Shared Documents' folder

### ------------------EXAMPLE 5------------------
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp -Match *.js -TargetFolder "Shared Documents" -ContentType "Test Content Type"
```

Creates an empty provisioning template, and includes all files with a JS extension in the c:\temp folder and marks the files in the template to be added to the 'Shared Documents' folder. It will add a property to the item for the content type.

### ------------------EXAMPLE 6------------------
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp -Match *.js -TargetFolder "Shared Documents" -Properties @{"Title" = "Test Title"; "Category"="Test Category"}
```

Creates an empty provisioning template, and includes all files with a JS extension in the c:\temp folder and marks the files in the template to be added to the 'Shared Documents' folder. It will add the specified properties to the file entries.

### ------------------EXAMPLE 7------------------
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.pnp
```

Creates an empty provisioning template as a pnp package file, and includes all files in the current folder

### ------------------EXAMPLE 8------------------
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.pnp -Folder c:\temp
```

Creates an empty provisioning template as a pnp package file, and includes all files in the c:\temp folder

## PARAMETERS

### -AsIncludeFile
If specified, the output will only contain the <pnp:Files> element. This allows the output to be included in another template.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ContentType
An optional content type to use.

```yaml
Type: ContentTypePipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Encoding
The encoding type of the XML file, Unicode is default

```yaml
Type: Encoding
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Folder
Folder to process. If not specified the current folder will be used.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: 0
Accept pipeline input: False
```

### -Force
Overwrites the output file if it exists.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Match
Optional wildcard pattern to match filenames against. If empty all files will be included.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Out
Filename to write to, optionally including full path.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: 0
Accept pipeline input: False
```

### -Properties
Additional properties to set for every file entry in the generated template.

```yaml
Type: Hashtable
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Schema
The schema of the output to use, defaults to the latest schema

```yaml
Type: XMLPnPSchemaVersion
Parameter Sets: (All)

Required: False
Position: 1
Accept pipeline input: False
```

### -TargetFolder
Target folder to provision to files to. If not specified, the current folder name will be used.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: 1
Accept pipeline input: False
```

### -Connection
Optional connection to be used by cmdlet. Retrieve the value for this parameter by eiter specifying -ReturnConnection on Connect-PnPOnline or by executing Get-PnPConnection.

```yaml
Type: SPOnlineConnection
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Web
The GUID, server relative url (i.e. /sites/team1) or web instance of the web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)[Encoding](https://msdn.microsoft.com/en-us/library/system.text.encoding_properties.aspx)