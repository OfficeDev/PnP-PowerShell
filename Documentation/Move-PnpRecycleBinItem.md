---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Move-PnpRecycleBinItem

## SYNOPSIS
Moves all items or a specific item in the first stage recycle bin of the current site collection to the second stage recycle bin

## SYNTAX 

```powershell
Move-PnpRecycleBinItem [-Identity <RecycleBinItemPipeBind>]
                       [-Force [<SwitchParameter>]]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Move-PnpRecycleBinItem
```

Moves all the items in the first stage recycle bin of the current site collection to the second stage recycle bin

### ------------------EXAMPLE 2------------------
```powershell
PS:> Move-PnpRecycleBinItem -Identity 26ffff29-b526-4451-9b6f-7f0e56ba7125
```

Moves the item with the provided ID in the first stage recycle bin of the current site collection to the second stage recycle bin without asking for confirmation first

### ------------------EXAMPLE 3------------------
```powershell
PS:> Move-PnpRecycleBinItem -Force
```

Moves all the items in the first stage recycle bin of the current context to the second stage recycle bin without asking for confirmation first

## PARAMETERS

### -Force
If provided, no confirmation will be asked to move the first stage recycle bin items to the second stage

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
If provided, moves the item with the specific ID to the second stage recycle bin

```yaml
Type: RecycleBinItemPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: True
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)