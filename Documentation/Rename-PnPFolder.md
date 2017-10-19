---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Rename-PnPFolder

## SYNOPSIS
Renames a folder

## SYNTAX 

```powershell
Rename-PnPFolder -Folder <String>
                 -TargetFolderName <String>
                 [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Rename-PnPFolder -Folder Documents/Reports -TargetFolderName 'Archived Reports'
```

This will rename the folder Reports in the Documents library to 'Archived Reports'

## PARAMETERS

### -Folder
The folder to rename

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -TargetFolderName
The new folder name

```yaml
Type: String
Parameter Sets: (All)

Required: True
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

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)