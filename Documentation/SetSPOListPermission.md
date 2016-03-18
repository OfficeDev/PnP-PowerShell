#Set-SPOListPermission
Sets list permissions
##Syntax
```powershell
Set-SPOListPermission -Group <GroupPipeBind> -Identity <ListPipeBind> [-AddRole <String>] [-RemoveRole <String>] [-Web <WebPipeBind>]
```


```powershell
Set-SPOListPermission -User <String> -Identity <ListPipeBind> [-AddRole <String>] [-RemoveRole <String>] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AddRole|String|False||
|Group|GroupPipeBind|True||
|Identity|ListPipeBind|True||
|RemoveRole|String|False||
|User|String|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Set-SPOListPermission -Identity 'Documents' -User 'user@contoso.com' -AddRole 'Contribute'
```
Adds the 'Contribute' permission to the user 'user@contoso.com' for the list 'Documents'

###Example 2
```powershell
PS:> Set-SPOListPermission -Identity 'Documents' -User 'user@contoso.com' -RemoveRole 'Contribute'
```
Removes the 'Contribute' permission to the user 'user@contoso.com' for the list 'Documents'
