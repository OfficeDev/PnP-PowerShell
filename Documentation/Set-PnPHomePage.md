---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPHomePage

## SYNOPSIS
Sets the home page of the current web.

## SYNTAX 

```powershell
Set-PnPHomePage -RootFolderRelativeUrl <String>
                [-Web <WebPipeBind>]
                [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPHomePage -RootFolderRelativeUrl SitePages/Home.aspx
```

Sets the home page to the home.aspx file which resides in the SitePages library

## PARAMETERS

### -RootFolderRelativeUrl
The root folder relative url of the homepage, e.g. 'sitepages/home.aspx'

```yaml
Type: String
Parameter Sets: (All)
Aliases: Path

Required: True
Position: 0
Accept pipeline input: True
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