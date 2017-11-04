---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPWebPartToWebPartPage

## SYNOPSIS
Adds a webpart to a web part page in a specified zone

## SYNTAX 

### XML
```powershell
Add-PnPWebPartToWebPartPage -Xml <String>
                            -ServerRelativePageUrl <String>
                            -ZoneId <String>
                            -ZoneIndex <Int>
                            [-Web <WebPipeBind>]
                            [-Connection <SPOnlineConnection>]
```

### FILE
```powershell
Add-PnPWebPartToWebPartPage -Path <String>
                            -ServerRelativePageUrl <String>
                            -ZoneId <String>
                            -ZoneIndex <Int>
                            [-Web <WebPipeBind>]
                            [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPWebPartToWebPartPage -ServerRelativePageUrl "/sites/demo/sitepages/home.aspx" -Path "c:\myfiles\listview.webpart" -ZoneId "Header" -ZoneIndex 1 
```

This will add the webpart as defined by the XML in the listview.webpart file to the specified page in the specified zone and with the order index of 1

### ------------------EXAMPLE 2------------------
```powershell
PS:> Add-PnPWebPartToWebPartPage -ServerRelativePageUrl "/sites/demo/sitepages/home.aspx" -XML $webpart -ZoneId "Header" -ZoneIndex 1 
```

This will add the webpart as defined by the XML in the $webpart variable to the specified page in the specified zone and with the order index of 1

## PARAMETERS

### -Path
A path to a webpart file on a the file system.

```yaml
Type: String
Parameter Sets: FILE

Required: True
Position: Named
Accept pipeline input: False
```

### -ServerRelativePageUrl
Server Relative Url of the page to add the webpart to.

```yaml
Type: String
Parameter Sets: (All)
Aliases: PageUrl

Required: True
Position: Named
Accept pipeline input: False
```

### -Xml
A string containing the XML for the webpart.

```yaml
Type: String
Parameter Sets: XML

Required: True
Position: Named
Accept pipeline input: False
```

### -ZoneId
The Zone Id where the webpart must be placed

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -ZoneIndex
The Zone Index where the webpart must be placed

```yaml
Type: Int
Parameter Sets: (All)

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