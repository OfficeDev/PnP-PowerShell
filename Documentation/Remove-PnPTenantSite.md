---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Remove-PnPTenantSite

## SYNOPSIS
Removes a site collection

## SYNTAX 

```powershell
Remove-PnPTenantSite -Url <String>
                     [-SkipRecycleBin [<SwitchParameter>]]
                     [-Force [<SwitchParameter>]]
```

## DESCRIPTION
Removes a site collection which is listed in your tenant administration site.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPTenantSite -Url https://tenant.sharepoint.com/sites/contoso
```

This will remove the site collection with the url 'https://tenant.sharepoint.com/sites/contoso'  and put it in the recycle bin.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPTenantSite -Url https://tenant.sharepoint.com/sites/contoso -Force -SkipRecycleBin
```

This will remove the site collection with the url 'https://tenant.sharepoint.com/sites/contoso' with force and it will skip the recycle bin.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Remove-PnPTenantSite -Url https://tenant.sharepoint.com/sites/contoso -FromRecycleBin
```

This will remove the site collection with the url 'https://tenant.sharepoint.com/sites/contoso' from the recycle bin.

## PARAMETERS

### -Force
Do not ask for confirmation.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -SkipRecycleBin
Do not add to the tenant scoped recycle bin when selected.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: SkipTrash

Required: False
Position: Named
Accept pipeline input: False
```

### -Url
Specifies the full URL of the site collection that needs to be deleted

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)