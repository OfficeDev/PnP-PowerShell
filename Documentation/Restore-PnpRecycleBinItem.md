---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Restore-PnpRecycleBinItem

## SYNOPSIS
Restores the provided recycle bin item to its original location

## SYNTAX 

### Identity
```powershell
Restore-PnpRecycleBinItem -Identity <RecycleBinItemPipeBind>
                          [-Force [<SwitchParameter>]]
```

### All
```powershell
Restore-PnpRecycleBinItem -All [<SwitchParameter>]
                          [-Force [<SwitchParameter>]]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Restore-PnpRecycleBinItem -Identity 72e4d749-d750-4989-b727-523d6726e442
```

Restores the recycle bin item with Id 72e4d749-d750-4989-b727-523d6726e442 to its original location

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPRecycleBinItems | ? FileLeafName -like "*.docx" | Restore-PnpRecycleBinItem
```

Restores all the items in the first and second stage recycle bins to their original location of which the filename ends with the .docx extension

## PARAMETERS

### -All
If provided all items will be stored 

```yaml
Type: SwitchParameter
Parameter Sets: All

Required: True
Position: Named
Accept pipeline input: True
```

### -Force
If provided, no confirmation will be asked to restore the recycle bin item

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
Id of the recycle bin item or the recycle bin item object itself to restore

```yaml
Type: RecycleBinItemPipeBind
Parameter Sets: Identity

Required: True
Position: Named
Accept pipeline input: True
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)