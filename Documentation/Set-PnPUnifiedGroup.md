---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPUnifiedGroup

## SYNOPSIS
Sets Office 365 Group (aka Unified Group) properties

## SYNTAX 

```powershell
Set-PnPUnifiedGroup -Identity <UnifiedGroupPipeBind>
                    [-DisplayName <String>]
                    [-Description <String>]
                    [-Owners <String[]>]
                    [-Members <String[]>]
                    [-IsPrivate [<SwitchParameter>]]
                    [-GroupLogoPath <String>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPUnifiedGroup -Identity $group -DisplayName "My Displayname"
```

Sets the display name of the group where $group is a Group entity

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPUnifiedGroup -Identity $groupId -Descriptions "My Description" -DisplayName "My DisplayName"
```

Sets the display name and description of a group based upon its ID

### ------------------EXAMPLE 3------------------
```powershell
PS:> Set-PnPUnifiedGroup -Identity $group -GroupLogoPath ".\MyLogo.png"
```

Sets a specific Office 365 Group logo.

### ------------------EXAMPLE 4------------------
```powershell
PS:> Set-PnPUnifiedGroup -Identity $group -IsPrivate:$false
```

Sets a group to be Public if previously Private.

### ------------------EXAMPLE 5------------------
```powershell
PS:> Set-PnPUnifiedGroup -Identity $group -Owners demo@contoso.com
```

Adds demo@contoso.com as an additional owner to the group.

## PARAMETERS

### -Description
The Description of the group to set.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -DisplayName
The DisplayName of the group to set.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -GroupLogoPath
The path to the logo file of to set.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
The Identity of the Office 365 Group.

```yaml
Type: UnifiedGroupPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: True
```

### -IsPrivate
Makes the group private when selected.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Members
The array UPN values of members to add to the group.

```yaml
Type: String[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Owners
The array UPN values of owners to add to the group.

```yaml
Type: String[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)