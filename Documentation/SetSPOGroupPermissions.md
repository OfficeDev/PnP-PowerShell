#Set-SPOGroupPermissions
Adds and/or removes permissions of a specific SharePoint group
##Syntax
```powershell
Set-SPOGroupPermissions [-AddRole <String[]>] [-RemoveRole <String[]>] [-Web <WebPipeBind>] -Identity <GroupPipeBind>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AddRole|String[]|False|Name of the permission set to add to this SharePoint group|
|Identity|GroupPipeBind|True|Get the permissions of a specific group by name|
|RemoveRole|String[]|False|Name of the permission set to remove from this SharePoint group|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Set-SPOGroupPermissions -Identity 'My Site Members' -AddRole Contribute
```
Adds the 'Contribute' permission to the SharePoint group with the name 'My Site Members'

###Example 2
```powershell
PS:> Set-SPOGroupPermissions -Identity 'My Site Members' -RemoveRole 'Full Control' -AddRole 'Read'
```
Removes the 'Full Control' from and adds the 'Contribute' permissions to the SharePoint group with the name 'My Site Members'

###Example 3
```powershell
PS:> Set-SPOGroupPermissions -Identity 'My Site Members' -AddRole @('Contribute', 'Design')
```
Adds the 'Contribute' and 'Design' permissions to the SharePoint group with the name 'My Site Members'

###Example 4
```powershell
PS:> Set-SPOGroupPermissions -Identity 'My Site Members' -RemoveRole @('Contribute', 'Design')
```
Removes the 'Contribute' and 'Design' permissions from the SharePoint group with the name 'My Site Members'
