---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Ensure-PnPFolder

## SYNOPSIS
Returns a folder from a given site relative path, and will create it if it does not exist.

## SYNTAX 

### 
```powershell
Ensure-PnPFolder -SiteRelativePath <String>
                 [-Web <WebPipeBind>]
                 [-Includes <String[]>]
```

## DESCRIPTION
If you do not want the folder to be created, for instance just to test if a folder exists, check Get-PnPFolder

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Ensure-PnPFolder -SiteRelativePath "demofolder/subfolder"
```

Creates a folder called subfolder in a folder called demofolder located in the root folder of the site. If the folder hierarchy does not exist, it will be created.

## PARAMETERS

### -Includes
Specify properties to include when retrieving objects from the server.

```yaml
Type: String[]
Parameter Sets: 

Required: False
Position: 0
Accept pipeline input: False
```

### -SiteRelativePath
Site Relative Folder Path

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: False
```

### -Web
The web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

## OUTPUTS

### [Microsoft.SharePoint.Client.Folder](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.folder.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)[Get-PnPFolder](https://github.com/OfficeDev/PnP-PowerShell/blob/master/Documentation/GetPnPFolder.md)