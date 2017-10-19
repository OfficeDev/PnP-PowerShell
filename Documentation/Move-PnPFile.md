---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Move-PnPFile

## SYNOPSIS
Moves a file to a different location

## SYNTAX 

### Server Relative
```powershell
Move-PnPFile -ServerRelativeUrl <String>
             -TargetUrl <String>
             [-OverwriteIfAlreadyExists [<SwitchParameter>]]
             [-Force [<SwitchParameter>]]
             [-Web <WebPipeBind>]
```

### Site Relative
```powershell
Move-PnPFile -SiteRelativeUrl <String>
             -TargetUrl <String>
             [-OverwriteIfAlreadyExists [<SwitchParameter>]]
             [-Force [<SwitchParameter>]]
             [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:>Move-PnPFile -ServerRelativeUrl /sites/project/Documents/company.docx -TargetUrl /sites/otherproject/Documents/company.docx
```

Moves a file named company.docx located in the document library called Documents located in the projects sitecollection under the managed path sites to the site collection otherproject located in the managed path sites. If a file named company.aspx already exists, it won't perform the move.

### ------------------EXAMPLE 2------------------
```powershell
PS:>Move-PnPFile -SiteRelativeUrl Documents/company.aspx -TargetUrl /sites/otherproject/Documents/company.docx
```

Moves a file named company.docx located in the document library called Documents located in the current site to the Documents library in the site collection otherproject located in the managed path sites. If a file named company.aspx already exists, it won't perform the move.

### ------------------EXAMPLE 3------------------
```powershell
PS:>Move-PnPFile -ServerRelativeUrl /sites/project/Documents/company.docx -TargetUrl /sites/otherproject/Documents/company.docx -OverwriteIfAlreadyExists
```

Moves a file named company.docx located in the document library called Documents located in the projects sitecollection under the managed path sites to the site collection otherproject located in the managed path sites. If a file named company.aspx already exists, it will still perform the move and replace the original company.aspx file.

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
If provided, if a file already exists at the TargetUrl, it will be overwritten. If ommitted, the move operation will be canceled if the file already exists at the TargetUrl location.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ServerRelativeUrl
Server relative Url specifying the file to move. Must include the file name.

```yaml
Type: String
Parameter Sets: Server Relative

Required: True
Position: 0
Accept pipeline input: True
```

### -SiteRelativeUrl
Site relative Url specifying the file to move. Must include the file name.

```yaml
Type: String
Parameter Sets: Site Relative

Required: True
Position: 0
Accept pipeline input: True
```

### -TargetUrl
Server relative Url where to move the file to. Must include the file name.

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