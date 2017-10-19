---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPWeb

## SYNOPSIS
Returns the current web object

## SYNTAX 

### 
```powershell
Get-PnPWeb [-Includes <String[]>]
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

### -Includes
Specify properties to include when retrieving objects from the server.

```yaml
Type: String[]
Parameter Sets: 

Required: False
Position: 0
Accept pipeline input: False
```

## OUTPUTS

### [Microsoft.SharePoint.Client.Web](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)