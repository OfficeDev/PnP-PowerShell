---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPSubWebs

## SYNOPSIS
Returns the subwebs of the current web

## SYNTAX 

```powershell
Get-PnPSubWebs [-Recurse [<SwitchParameter>]]
               [-Web <WebPipeBind>]
               [-Identity <WebPipeBind>]
```

## PARAMETERS

### -Identity


```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: 0
Accept pipeline input: True
```

### -Recurse


```yaml
Type: SwitchParameter
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

### [List<Microsoft.SharePoint.Client.Web>](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)