---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPGroup

## SYNOPSIS
Returns a specific group or all groups.

## SYNTAX 

### ByName
```powershell
Get-PnPGroup [-Web <WebPipeBind>]
             [-Identity <GroupPipeBind>]
```

### Members
```powershell
Get-PnPGroup [-AssociatedMemberGroup [<SwitchParameter>]]
             [-Web <WebPipeBind>]
```

### Visitors
```powershell
Get-PnPGroup [-AssociatedVisitorGroup [<SwitchParameter>]]
             [-Web <WebPipeBind>]
```

### Owners
```powershell
Get-PnPGroup [-AssociatedOwnerGroup [<SwitchParameter>]]
             [-Web <WebPipeBind>]
```

### 
```powershell
Get-PnPGroup [-Web <WebPipeBind>]
             [-Includes <String[]>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPGroup
```

Returns all groups

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPGroup -Identity 'My Site Users'
```

This will return the group called 'My Site Users' if available

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPGroup -AssociatedMemberGroup
```

This will return the current members group for the site

## PARAMETERS

### -AssociatedMemberGroup
Retrieve the associated member group

```yaml
Type: SwitchParameter
Parameter Sets: Members

Required: False
Position: Named
Accept pipeline input: False
```

### -AssociatedOwnerGroup
Retrieve the associated owner group

```yaml
Type: SwitchParameter
Parameter Sets: Owners

Required: False
Position: Named
Accept pipeline input: False
```

### -AssociatedVisitorGroup
Retrieve the associated visitor group

```yaml
Type: SwitchParameter
Parameter Sets: Visitors

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
Get a specific group by name

```yaml
Type: GroupPipeBind
Parameter Sets: ByName
Aliases: Name

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

### -Web
The web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

## OUTPUTS

### [List<Microsoft.SharePoint.Client.Group>](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.group.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)