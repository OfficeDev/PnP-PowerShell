---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Rename-PnPFile

## SYNOPSIS
Renames a file in its current location

## SYNTAX 

### SERVER
```powershell
Rename-PnPFile -ServerRelativeUrl <String>
               -TargetFileName <String>
               [-OverwriteIfAlreadyExists [<SwitchParameter>]]
               [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
```

### SITE
```powershell
Rename-PnPFile -SiteRelativeUrl <String>
               -TargetFileName <String>
               [-OverwriteIfAlreadyExists [<SwitchParameter>]]
               [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:>Rename-PnPFile -ServerRelativeUrl /sites/project/Documents/company.docx -TargetFileName mycompany.docx
```

Renames a file named company.docx located in the document library called Documents located in the projects sitecollection under the managed path sites to mycompany.docx. If a file named mycompany.aspx already exists, it won't perform the rename.

### ------------------EXAMPLE 2------------------
```powershell
PS:>Rename-PnPFile -SiteRelativeUrl Documents/company.aspx -TargetFileName mycompany.docx
```

Renames a file named company.docx located in the document library called Documents located in the current site to mycompany.aspx. If a file named mycompany.aspx already exists, it won't perform the rename.

### ------------------EXAMPLE 3------------------
```powershell
PS:>Rename-PnPFile -ServerRelativeUrl /sites/project/Documents/company.docx -TargetFileName mycompany.docx -OverwriteIfAlreadyExists
```

Renames a file named company.docx located in the document library called Documents located in the projects sitecollection under the managed path sites to mycompany.aspx. If a file named mycompany.aspx already exists, it will still perform the rename and replace the original mycompany.aspx file.

## PARAMETERS

### -Force
If provided, no confirmation will be requested and the action will be performed

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -OverwriteIfAlreadyExists
If provided, if a file already exist with the provided TargetFileName, it will be overwritten. If ommitted, the rename operation will be canceled if a file already exists with the TargetFileName file name.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ServerRelativeUrl
Server relative Url specifying the file to rename. Must include the file name.

```yaml
Type: String
Parameter Sets: SERVER

Required: True
Position: 0
Accept pipeline input: True
```

### -SiteRelativeUrl
Site relative Url specifying the file to rename. Must include the file name.

```yaml
Type: String
Parameter Sets: SITE

Required: True
Position: 0
Accept pipeline input: True
```

### -TargetFileName
File name to rename the file to. Should only be the file name and not include the path to its location. Use Move-PnPFile to move the file to another location.

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 1
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

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)