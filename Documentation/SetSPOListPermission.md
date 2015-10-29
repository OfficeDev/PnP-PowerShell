#Set-SPOListPermission
*Topic automatically generated on: 2015-10-13*

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
