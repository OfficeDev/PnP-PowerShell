#Add-SPOCustomAction
*Topic automatically generated on: 2015-10-13*

Adds a custom action to a web
##Syntax
```powershell
Add-SPOCustomAction -Name <String> -Title <String> -Description <String> -Group <String> -Location <String> [-Sequence <Int32>] [-Url <String>] [-ImageUrl <String>] [-CommandUIExtension <String>] [-RegistrationId <String>] [-Rights <PermissionKind[]>] [-RegistrationType <UserCustomActionRegistrationType>] [-Scope <CustomActionScope>] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|CommandUIExtension|String|False||
|Description|String|True||
|Group|String|True||
|ImageUrl|String|False||
|Location|String|True||
|Name|String|True||
|RegistrationId|String|False||
|RegistrationType|UserCustomActionRegistrationType|False||
|Rights|PermissionKind[]|False||
|Scope|CustomActionScope|False||
|Sequence|Int32|False||
|Title|String|True||
|Url|String|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
