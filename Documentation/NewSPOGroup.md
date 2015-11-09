#New-SPOGroup
*Topic automatically generated on: 2015-10-19*

Adds a user to the build-in Site User Info List and returns a user object
##Syntax
```powershell
New-SPOGroup -Title <String> [-Description <String>] [-Owner <String>] [-AllowRequestToJoinLeave [<SwitchParameter>]] [-AutoAcceptRequestToJoinLeave [<SwitchParameter>]] [-AllowMembersEditMembership [<SwitchParameter>]] [-OnlyAllowMembersViewMembership [<SwitchParameter>]] [-RequestToJoinEmail <String>] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AllowMembersEditMembership|SwitchParameter|False||
|AllowRequestToJoinLeave|SwitchParameter|False||
|AutoAcceptRequestToJoinLeave|SwitchParameter|False||
|Description|String|False||
|OnlyAllowMembersViewMembership|SwitchParameter|False||
|Owner|String|False||
|RequestToJoinEmail|String|False||
|Title|String|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> New-SPOUser -LogonName user@company.com
```

Adds a user to the build-in Site User Info List and returns a user object
##Syntax
```powershell
New-SPOGroup -Title <String> [-Description <String>] [-Owner <String>] [-AllowRequestToJoinLeave [<SwitchParameter>]] [-AutoAcceptRequestToJoinLeave [<SwitchParameter>]] [-AllowMembersEditMembership [<SwitchParameter>]] [-OnlyAllowMembersViewMembership [<SwitchParameter>]] [-RequestToJoinEmail <String>] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AllowMembersEditMembership|SwitchParameter|False||
|AllowRequestToJoinLeave|SwitchParameter|False||
|AutoAcceptRequestToJoinLeave|SwitchParameter|False||
|Description|String|False||
|OnlyAllowMembersViewMembership|SwitchParameter|False||
|Owner|String|False||
|RequestToJoinEmail|String|False||
|Title|String|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> New-SPOUser -LogonName user@company.com
```

