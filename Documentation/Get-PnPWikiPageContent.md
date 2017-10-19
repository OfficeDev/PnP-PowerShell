---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPWikiPageContent

## SYNOPSIS
Gets the contents/source of a wiki page

## SYNTAX 

```powershell
Get-PnPWikiPageContent -ServerRelativePageUrl <String>
                       [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPWikiPageContent -PageUrl '/sites/demo1/pages/wikipage.aspx'
```

Gets the content of the page '/sites/demo1/pages/wikipage.aspx'

## PARAMETERS

### -ServerRelativePageUrl
The server relative URL for the wiki page

```yaml
Type: String
Parameter Sets: (All)
Aliases: PageUrl

Required: True
Position: 0
Accept pipeline input: True
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