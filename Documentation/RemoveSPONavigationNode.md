#Remove-SPONavigationNode
*Topic automatically generated on: 2015-10-26*

Removes a menu item from either the quicklaunch or top navigation
##Syntax
```powershell
Remove-SPONavigationNode -Location <NavigationType> -Title <String> [-Header <String>] [-Force [<SwitchParameter>]] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False||
|Header|String|False||
|Location|NavigationType|True||
|Title|String|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Remove-SPONavigationNode -Title Recent -Location QuickLaunch
```
Will remove the recent navigation node from the quick launch in the current web.

###Example 2
```powershell
PS:> Remove-SPONavigationNode -Title Home -Location TopNavigationBar -Force
```
Will remove the home navigation node from the top navigation bar without prompting for a confirmation in the current web.
