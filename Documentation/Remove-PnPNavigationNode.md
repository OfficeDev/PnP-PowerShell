---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPNavigationNode

## SYNOPSIS
Removes a menu item from either the quicklaunch or top navigation

## SYNTAX 

```powershell
Remove-PnPNavigationNode -Location <NavigationType>
                         -Title <String>
                         [-Header <String>]
                         [-Force [<SwitchParameter>]]
                         [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPNavigationNode -Title Recent -Location QuickLaunch
```

Will remove the recent navigation node from the quick launch in the current web.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPNavigationNode -Title Home -Location TopNavigationBar -Force
```

Will remove the home navigation node from the top navigation bar without prompting for a confirmation in the current web.

## PARAMETERS

### -Force
Specifying the Force parameter will skip the confirmation question.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Header
The header where the node is located

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Location
The location from where to remove the node (QuickLaunch, TopNavigationBar

```yaml
Type: NavigationType
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Title
The title of the node that needs to be removed

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Web
The GUID, server relative url (i.e. /sites/team1) or web instance of the web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)