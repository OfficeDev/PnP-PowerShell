---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPWebPart

## SYNOPSIS
Returns a webpart definition object

## SYNTAX 

```powershell
Get-PnPWebPart -ServerRelativePageUrl <String>
               [-Identity <WebPartPipeBind>]
               [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPWebPart -ServerRelativePageUrl "/sites/demo/sitepages/home.aspx"
```

Returns all webparts defined on the given page.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPWebPart -ServerRelativePageUrl "/sites/demo/sitepages/home.aspx" -Identity a2875399-d6ff-43a0-96da-be6ae5875f82
```

Returns a specific webpart defined on the given page.

## PARAMETERS

### -Identity


```yaml
Type: WebPartPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: True
```

### -ServerRelativePageUrl
Full server relative URL of the webpart page, e.g. /sites/mysite/sitepages/home.aspx

```yaml
Type: String
Parameter Sets: (All)
Aliases: PageUrl

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

## OUTPUTS

### [List<Microsoft.SharePoint.Client.WebParts.WebPartDefinition>](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.webparts.webpartdefinition.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)