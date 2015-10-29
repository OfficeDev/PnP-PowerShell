#Add-SPONavigationNode
*Topic automatically generated on: 2015-10-13*

Adds a menu item to either the quicklaunch or top navigation
##Syntax
```powershell
Add-SPONavigationNode -Location <NavigationType> -Title <String> [-Url <String>] [-Header <String>] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Header|String|False|Optionally value of a header entry to add the menu item to.|
|Location|NavigationType|True|The location of the node to add. Either TopNavigationBar, QuickLaunch or SearchNav|
|Title|String|True|The title of the node to add|
|Url|String|False|The url to navigate to when clicking the new menu item.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
