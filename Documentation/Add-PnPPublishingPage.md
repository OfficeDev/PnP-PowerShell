---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPPublishingPage

## SYNOPSIS
Adds a publishing page

## SYNTAX 

### WithTitle
```powershell
Add-PnPPublishingPage -PageName <String>
                      -PageTemplateName <String>
                      [-Title <String>]
                      [-FolderPath <String>]
                      [-Publish [<SwitchParameter>]]
                      [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPPublishingPage -PageName 'OurNewPage' -Title 'Our new page' -PageTemplateName 'ArticleLeft'
```

Creates a new page based on the pagelayout 'ArticleLeft'

### ------------------EXAMPLE 2------------------
```powershell
PS:> Add-PnPPublishingPage -PageName 'OurNewPage' -Title 'Our new page' -PageTemplateName 'ArticleLeft' -Folder '/Pages/folder'
```

Creates a new page based on the pagelayout 'ArticleLeft' with a site relative folder path

## PARAMETERS

### -FolderPath
The site relative folder path of the page to be added

```yaml
Type: String
Parameter Sets: (All)
Aliases: Folder

Required: False
Position: Named
Accept pipeline input: False
```

### -PageName
The name of the page to be added as an aspx file

```yaml
Type: String
Parameter Sets: (All)
Aliases: Name

Required: True
Position: Named
Accept pipeline input: False
```

### -PageTemplateName
The name of the page layout you want to use. Specify without the .aspx extension. So 'ArticleLeft' or 'BlankWebPartPage'

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Publish
Publishes the page. Also Approves it if moderation is enabled on the Pages library.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Title
The title of the page

```yaml
Type: String
Parameter Sets: WithTitle

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

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)