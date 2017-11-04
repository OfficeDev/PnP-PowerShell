---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPWebPartXml

## SYNOPSIS
Returns the webpart XML of a webpart registered on a site

## SYNTAX 

```powershell
Get-PnPWebPartXml -ServerRelativePageUrl <String>
                  -Identity <WebPartPipeBind>
                  [-Web <WebPipeBind>]
                  [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPWebPartXml -ServerRelativePageUrl "/sites/demo/sitepages/home.aspx" -Identity a2875399-d6ff-43a0-96da-be6ae5875f82
```

Returns the webpart XML for a given webpart on a page.

## PARAMETERS

### -Identity
Id or title of the webpart. Use Get-PnPWebPart to retrieve all webpart Ids

```yaml
Type: WebPartPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -ServerRelativePageUrl
Full server relative url of the webpart page, e.g. /sites/mysite/sitepages/home.aspx

```yaml
Type: String
Parameter Sets: (All)
Aliases: PageUrl

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

## OUTPUTS

### System.String

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)