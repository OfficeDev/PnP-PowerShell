#Get-SPOGroup
*Topic automatically generated on: 2015-10-13*

Returns a specific group or all groups.
##Syntax
```powershell
Get-SPOGroup [-Web <WebPipeBind>] [-Identity <GroupPipeBind>]
```


```powershell
Get-SPOGroup [-AssociatedMemberGroup [<SwitchParameter>]] [-Web <WebPipeBind>]
```


```powershell
Get-SPOGroup [-AssociatedOwnerGroup [<SwitchParameter>]] [-Web <WebPipeBind>]
```


```powershell
Get-SPOGroup [-AssociatedVisitorGroup [<SwitchParameter>]] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AssociatedMemberGroup|SwitchParameter|False|Retrieve the associated member group|
|AssociatedOwnerGroup|SwitchParameter|False|Retrieve the associated owner group|
|AssociatedVisitorGroup|SwitchParameter|False|Retrieve the associated visitor group|
|Identity|GroupPipeBind|False|Get a specific group by name|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-SPOGroup
```
Returns all groups

###Example 2
```powershell
PS:> Get-SPOGroup -Identity 'My Site Users'
```
This will return the group called 'My Site Users' if available

###Example 3
```powershell
PS:> Get-SPOGroup -AssociatedMemberGroup
```
This will return the current members group for the site
