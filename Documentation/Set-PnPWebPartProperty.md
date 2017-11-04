---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPWebPartProperty

## SYNOPSIS
Sets a web part property

## SYNTAX 

```powershell
Set-PnPWebPartProperty -ServerRelativePageUrl <String>
                       -Identity <GuidPipeBind>
                       -Key <String>
                       -Value <PSObject>
                       [-Web <WebPipeBind>]
                       [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPWebPartProperty -ServerRelativePageUrl /sites/demo/sitepages/home.aspx -Identity ccd2c98a-c9ae-483b-ae72-19992d583914 -Key "Title" -Value "New Title" 
```

Sets the title property of the webpart.

## PARAMETERS

### -Identity
The Guid of the webpart

```yaml
Type: GuidPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Key
Name of a single property to be set

```yaml
Type: String
Parameter Sets: (All)

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

### -Value
Value of the property to be set

```yaml
Type: PSObject
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