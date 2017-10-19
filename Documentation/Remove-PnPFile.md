---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPFile

## SYNOPSIS
Removes a file.

## SYNTAX 

### Server Relative
```powershell
Remove-PnPFile -ServerRelativeUrl <String>
               [-Recycle [<SwitchParameter>]]
               [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
```

### Site Relative
```powershell
Remove-PnPFile -SiteRelativeUrl <String>
               [-Recycle [<SwitchParameter>]]
               [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:>Remove-PnPFile -ServerRelativeUrl /sites/project/_catalogs/themes/15/company.spcolor
```

Removes the file company.spcolor

### ------------------EXAMPLE 2------------------
```powershell
PS:>Remove-PnPFile -SiteRelativeUrl _catalogs/themes/15/company.spcolor
```

Removes the file company.spcolor

### ------------------EXAMPLE 3------------------
```powershell
PS:>Remove-PnPFile -SiteRelativeUrl _catalogs/themes/15/company.spcolor -Recycle
```

Removes the file company.spcolor and saves it to the Recycle Bin

## PARAMETERS

### -Force


```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Recycle


```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ServerRelativeUrl
Server relative URL to the file

```yaml
Type: String
Parameter Sets: Server Relative

Required: True
Position: 0
Accept pipeline input: True
```

### -SiteRelativeUrl
Site relative URL to the file

```yaml
Type: String
Parameter Sets: Site Relative

Required: True
Position: 0
Accept pipeline input: True
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