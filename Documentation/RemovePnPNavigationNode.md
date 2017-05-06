# Remove-PnPNavigationNode
Removes a menu item from either the quicklaunch or top navigation
## Syntax
```powershell
Remove-PnPNavigationNode -Location <NavigationType>
                         -Title <String>
                         [-Header <String>]
                         [-Force [<SwitchParameter>]]
                         [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Location|NavigationType|True|The location from where to remove the node (QuickLaunch, TopNavigationBar|
|Title|String|True|The title of the node that needs to be removed|
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question.|
|Header|String|False|The header where the node is located|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPNavigationNode -Title Recent -Location QuickLaunch
```
Will remove the recent navigation node from the quick launch in the current web.

### Example 2
```powershell
PS:> Remove-PnPNavigationNode -Title Home -Location TopNavigationBar -Force
```
Will remove the home navigation node from the top navigation bar without prompting for a confirmation in the current web.
