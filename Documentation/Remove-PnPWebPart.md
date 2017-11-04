---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPWebPart

## SYNOPSIS
Removes a webpart from a page

## SYNTAX 

### ID
```powershell
Remove-PnPWebPart -Identity <GuidPipeBind>
                  -ServerRelativePageUrl <String>
                  [-Web <WebPipeBind>]
                  [-Connection <SPOnlineConnection>]
```

### NAME
```powershell
Remove-PnPWebPart -Title <String>
                  -ServerRelativePageUrl <String>
                  [-Web <WebPipeBind>]
                  [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPWebPartToWikiPage -ServerRelativePageUrl "/sites/demo/sitepages/home.aspx" -Identity a2875399-d6ff-43a0-96da-be6ae5875f82
```

This will remove the webpart as defined by the XML in the listview.webpart file to the specified page in the first row and the first column of the HTML table present on the page

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPWebPartToWikiPage -ServerRelativePageUrl "/sites/demo/sitepages/home.aspx" -Name MyWebpart
```

This will remove the webpart as defined by the XML in the listview.webpart file to the specified page in the first row and the first column of the HTML table present on the page

## PARAMETERS

### -Identity
The Guid of the webpart

```yaml
Type: GuidPipeBind
Parameter Sets: ID

Required: True
Position: Named
Accept pipeline input: False
```

### -ServerRelativePageUrl
Full server relative url of the webpart page, e.g. /sites/demo/sitepages/home.aspx

```yaml
Type: String
Parameter Sets: (All)
Aliases: PageUrl

Required: True
Position: Named
Accept pipeline input: False
```

### -Title
The name of the webpart

```yaml
Type: String
Parameter Sets: NAME
Aliases: Name

Required: True
Position: Named
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

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)