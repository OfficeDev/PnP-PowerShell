---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPNavigationNode

## SYNOPSIS
Adds an item to a navigation element

## SYNTAX 

```powershell
Add-PnPNavigationNode -Location <NavigationType>
                      -Title <String>
                      [-Url <String>]
                      [-Header <String>]
                      [-First [<SwitchParameter>]]
                      [-External [<SwitchParameter>]]
                      [-Web <WebPipeBind>]
```

## DESCRIPTION
Adds a menu item to either the quicklaunch or top navigation

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPNavigationNode -Title "Contoso" -Url "http://contoso.sharepoint.com/sites/contoso/" -Location "QuickLaunch"
```

Adds a navigation node to the quicklaunch. The navigation node will have the title "Contoso" and will link to the url "http://contoso.sharepoint.com/sites/contoso/"

### ------------------EXAMPLE 2------------------
```powershell
PS:> Add-PnPNavigationNode -Title "Contoso USA" -Url "http://contoso.sharepoint.com/sites/contoso/usa/" -Location "QuickLaunch" -Header "Contoso"
```

Adds a navigation node to the quicklaunch. The navigation node will have the title "Contoso USA", will link to the url "http://contoso.sharepoint.com/sites/contoso/usa/" and will have "Contoso" as a parent navigation node.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Add-PnPNavigationNode -Title "Contoso" -Url "http://contoso.sharepoint.com/sites/contoso/" -Location "QuickLaunch" -First
```

Adds a navigation node to the quicklaunch, as the first item. The navigation node will have the title "Contoso" and will link to the url "http://contoso.sharepoint.com/sites/contoso/"

### ------------------EXAMPLE 4------------------
```powershell
PS:> Add-PnPNavigationNode -Title "Contoso Pharmaceuticals" -Url "http://contoso.sharepoint.com/sites/contosopharma/" -Location "QuickLaunch" -External
```

Adds a navigation node to the quicklaunch. The navigation node will have the title "Contoso Pharmaceuticals" and will link to the external url "http://contoso.sharepoint.com/sites/contosopharma/"

## PARAMETERS

### -External
Indicates the destination URL is outside of the site collection.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -First
Add the new menu item to beginning of the collection.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Header
Optionally value of a header entry to add the menu item to.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Location
The location of the node to add. Either TopNavigationBar, QuickLaunch or SearchNav

```yaml
Type: NavigationType
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Title
The title of the node to add

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Url
The url to navigate to when clicking the new menu item.

```yaml
Type: String
Parameter Sets: (All)

Required: False
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