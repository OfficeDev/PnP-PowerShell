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
```

### NAME
```powershell
Remove-PnPWebPart -Title <String>
                  -ServerRelativePageUrl <String>
                  [-Web <WebPipeBind>]
```

## PARAMETERS

### -Identity


```yaml
Type: GuidPipeBind
Parameter Sets: ID

Required: True
Position: Named
Accept pipeline input: False
```

### -ServerRelativePageUrl


```yaml
Type: String
Parameter Sets: (All)
Aliases: PageUrl

Required: True
Position: Named
Accept pipeline input: False
```

### -Title


```yaml
Type: String
Parameter Sets: NAME
Aliases: Name

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