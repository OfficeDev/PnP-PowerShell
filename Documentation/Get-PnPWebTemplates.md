---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Get-PnPWebTemplates

## SYNOPSIS
Returns the available web templates.

## SYNTAX 

```powershell
Get-PnPWebTemplates [-Lcid <UInt32>]
                    [-CompatibilityLevel <Int>]
                    [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Will list all available templates one can use to create a classic site.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPWebTemplates
```



### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPWebTemplates -LCID 1033
```

Returns all webtemplates for the Locale with ID 1033 (English)

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPWebTemplates -CompatibilityLevel 15
```

Returns all webtemplates for the compatibility level 15

## PARAMETERS

### -CompatibilityLevel
The version of SharePoint

```yaml
Type: Int
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Lcid
The language ID. For instance: 1033 for English

```yaml
Type: UInt32
Parameter Sets: (All)

Required: False
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

## OUTPUTS

### [Microsoft.Online.SharePoint.TenantAdministration.SPOTenantWebTemplateCollection](https://msdn.microsoft.com/en-us/library/microsoft.online.sharepoint.tenantadministration.spotenantwebtemplatecollection.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)[Locale IDs](http://go.microsoft.com/fwlink/p/?LinkId=242911Id=242911)