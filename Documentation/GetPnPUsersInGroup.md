# Get-PnPUsersInGroup
Retrieves all users in a group
## Syntax
```powershell
Get-PnPUsersInGroup -Identity <GroupPipeBind>
                    [-Web <WebPipeBind>]
```


## Detailed Description
This command will return all the users that are a member of the provided SharePoint Group

## Returns
>[Microsoft.SharePoint.Client.User](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.user.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|GroupPipeBind|True|A group object, an ID or a name of a group|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPUserInGroup -GroupName 'Marketing Site Members'
```
Returns all the users that are a member of the group 'Marketing Site Members'

### Example 2
```powershell
PS:> Get-PnPGroup | Get-PnPUsersInGroup
```
Returns all the users that are a member of any of the groups in the current site

### Example 3
```powershell
PS:> Get-PnPGroup -Web subsite1 | ? Title -Like 'Marketing*' | Get-PnPUsersInGroup
```
Returns all the users that are a member of any of the groups of which their name starts with the word 'Marketing' in the subsite named 'subsite1'
