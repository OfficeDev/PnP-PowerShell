---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPMasterPage

## SYNOPSIS
Adds a Masterpage

## SYNTAX 

```powershell
Add-PnPMasterPage -SourceFilePath <String>
                  -Title <String>
                  -Description <String>
                  [-DestinationFolderHierarchy <String>]
                  [-UIVersion <String>]
                  [-DefaultCssFile <String>]
                  [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPMasterPage -SourceFilePath "page.master" -Title "MasterPage" -Description "MasterPage for Web" -DestinationFolderHierarchy "SubFolder"
```

Adds a MasterPage from the local file "page.master" to the folder "SubFolder" in the Masterpage gallery.

## PARAMETERS

### -DefaultCssFile
Default CSS file for the MasterPage, this Url is SiteRelative

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Description
Description for the Masterpage

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -DestinationFolderHierarchy
Folder hierarchy where the MasterPage will be deployed

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -SourceFilePath
Path to the file which will be uploaded

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Title
Title for the Masterpage

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -UIVersion
UIVersion of the Masterpage. Default = 15

```yaml
Type: String
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

## OUTPUTS

### [Microsoft.SharePoint.Client.File](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.file.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)