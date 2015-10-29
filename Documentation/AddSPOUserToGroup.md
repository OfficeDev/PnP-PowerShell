#Add-SPOUserToGroup
*Topic automatically generated on: 2015-10-13*

Adds a user to a group
##Syntax
```powershell
Add-SPOUserToGroup -LoginName <String> -Identity <GroupPipeBind> [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|GroupPipeBind|True|The group id, group name or group object to add the user to.|
|LoginName|String|True|The login name of the user|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Add-SPOUserToGroup -LoginName user@company.com -Identity 'Marketing Site Members'
```


###Example 2
```powershell
PS:> Add-SPOUserToGroup -LoginName user@company.com -Identity 5
```
Add the specified user to the group with Id 5
