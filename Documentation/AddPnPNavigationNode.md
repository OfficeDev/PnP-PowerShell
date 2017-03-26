# Add-PnPNavigationNode
Adds a menu item to either the quicklaunch or top navigation
## Syntax
```powershell
Add-PnPNavigationNode -Location <NavigationType>
                      -Title <String>
                      [-Url <String>]
                      [-Header <String>]
                      [-First [<SwitchParameter>]]
                      [-External [<SwitchParameter>]]
                      [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Location|NavigationType|True|The location of the node to add. Either TopNavigationBar, QuickLaunch or SearchNav|
|Title|String|True|The title of the node to add|
|External|SwitchParameter|False|Indicates the destination URL is outside of the site collection.|
|First|SwitchParameter|False|Add the new menu item to beginning of the collection.|
|Header|String|False|Optionally value of a header entry to add the menu item to.|
|Url|String|False|The url to navigate to when clicking the new menu item.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPNavigationNode -Title "Contoso" -Url "http://contoso.sharepoint.com/sites/contoso/" -Location "QuickLaunch"
```
Adds a navigation node to the quicklaunch. The navigation node will have the title "Contoso" and will link to the url "http://contoso.sharepoint.com/sites/contoso/"

### Example 2
```powershell
PS:> Add-PnPNavigationNode -Title "Contoso USA" -Url "http://contoso.sharepoint.com/sites/contoso/usa/" -Location "QuickLaunch" -Header "Contoso"
```
Adds a navigation node to the quicklaunch. The navigation node will have the title "Contoso USA", will link to the url "http://contoso.sharepoint.com/sites/contoso/usa/" and will have "Contoso" as a parent navigation node.

### Example 3
```powershell
PS:> Add-PnPNavigationNode -Title "Contoso" -Url "http://contoso.sharepoint.com/sites/contoso/" -Location "QuickLaunch" -First
```
Adds a navigation node to the quicklaunch, as the first item. The navigation node will have the title "Contoso" and will link to the url "http://contoso.sharepoint.com/sites/contoso/"

### Example 4
```powershell
PS:> Add-PnPNavigationNode -Title "Contoso Pharmaceuticals" -Url "http://contoso.sharepoint.com/sites/contosopharma/" -Location "QuickLaunch" -External
```
Adds a navigation node to the quicklaunch. The navigation node will have the title "Contoso Pharmaceuticals" and will link to the external url "http://contoso.sharepoint.com/sites/contosopharma/"
