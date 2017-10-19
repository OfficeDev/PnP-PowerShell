---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Clear-PnPTenantRecycleBinItem

## SYNOPSIS
Permanently deletes a site collection from the tenant scoped recycle bin

## SYNTAX 

```powershell
Clear-PnPTenantRecycleBinItem -Url <String>
                              [-Wait [<SwitchParameter>]]
                              [-Force [<SwitchParameter>]]
```

## DESCRIPTION
The Clear-PnPTenantRecycleBinItem cmdlet allows a site collection that has been deleted and still exists in the tenant recycle bin to be permanently deleted from the recycle bin as well.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Clear-PnPTenantRecycleBinItem -Url https://tenant.sharepoint.com/sites/contoso
```

This will permanently delete site collection with the url 'https://tenant.sharepoint.com/sites/contoso' from the tenant recycle bin

### ------------------EXAMPLE 2------------------
```powershell
PS:> Clear-PnPTenantRecycleBinItem -Url https://tenant.sharepoint.com/sites/contoso -Wait
```

This will permanently delete site collection with the url 'https://tenant.sharepoint.com/sites/contoso' from the tenant recycle bin and will wait with executing further PowerShell commands until the operation has completed

## PARAMETERS

### -Force
If provided, no confirmation will be asked to permanently delete the site collection from the tenant recycle bin

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Url
Url of the site collection to permanently delete from the tenant recycle bin

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Wait
If provided, the PowerShell execution will halt until the operation has completed

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)