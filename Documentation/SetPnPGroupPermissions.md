# Set-PnPGroupPermissions
Adds and/or removes permissions of a specific SharePoint group
## Syntax
```powershell
Set-PnPGroupPermissions -Identity <GroupPipeBind>
                        [-List <ListPipeBind>]
                        [-AddRole <String[]>]
                        [-RemoveRole <String[]>]
                        [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|GroupPipeBind|True|Get the permissions of a specific group by name|
|AddRole|String[]|False|Name of the permission set to add to this SharePoint group|
|List|ListPipeBind|False|The list to apply the command to.|
|RemoveRole|String[]|False|Name of the permission set to remove from this SharePoint group|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -AddRole Contribute
```
Adds the 'Contribute' permission to the SharePoint group with the name 'My Site Members'

### Example 2
```powershell
PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -RemoveRole 'Full Control' -AddRole 'Read'
```
Removes the 'Full Control' from and adds the 'Contribute' permissions to the SharePoint group with the name 'My Site Members'

### Example 3
```powershell
PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -AddRole @('Contribute', 'Design')
```
Adds the 'Contribute' and 'Design' permissions to the SharePoint group with the name 'My Site Members'

### Example 4
```powershell
PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -RemoveRole @('Contribute', 'Design')
```
Removes the 'Contribute' and 'Design' permissions from the SharePoint group with the name 'My Site Members'

### Example 5
```powershell
PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -List 'MyList' -RemoveRole @('Contribute')
```
Removes the 'Contribute' permissions from the list 'MyList' for the group with the name 'My Site Members'
