---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Restore-PnPTenantRecycleBinItem

## SYNOPSIS
Restores a site collection from the tenant scoped recycle bin

## SYNTAX 

```powershell
Restore-PnPTenantRecycleBinItem -Url <String>
                                [-Wait [<SwitchParameter>]]
                                [-Force [<SwitchParameter>]]
```

## DESCRIPTION
The Reset-PnPTenantRecycleBinItem cmdlet allows a site collection that has been deleted and still exists in the tenant recycle bin to be restored to its original location.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Reset-PnPTenantRecycleBinItem -Url https://tenant.sharepoint.com/sites/contoso
```

This will restore the deleted site collection with the url 'https://tenant.sharepoint.com/sites/contoso' to its original location

### ------------------EXAMPLE 2------------------
```powershell
PS:> Reset-PnPTenantRecycleBinItem -Url https://tenant.sharepoint.com/sites/contoso -Wait
```

This will restore the deleted site collection with the url 'https://tenant.sharepoint.com/sites/contoso' to its original location and will wait with executing further PowerShell commands until the site collection restore has completed

## PARAMETERS

### -Force
If provided, no confirmation will be asked to restore the site collection from the tenant recycle bin

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Url
Url of the site collection to restore from the tenant recycle bin

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Wait
If provided, the PowerShell execution will halt until the site restore process has completed

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)