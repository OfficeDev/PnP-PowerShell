---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Remove-PnPUnifiedGroup

## SYNOPSIS
Removes one Office 365 Group (aka Unified Group) or a list of Office 365 Groups

## SYNTAX 

```powershell
Remove-PnPUnifiedGroup -Identity <UnifiedGroupPipeBind>
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPUnifiedGroup -Identity $groupId
```

Removes an Office 365 Groups based on its ID

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPUnifiedGroup -Identity $group
```

Removes the provided Office 365 Groups

## PARAMETERS

### -Identity
The Identity of the Office 365 Group.

```yaml
Type: UnifiedGroupPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)