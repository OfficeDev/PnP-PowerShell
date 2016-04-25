#Set-SPOGroup
Updates a group
##Syntax
```powershell
Set-SPOGroup -Identity <GroupPipeBind> [-SetAssociatedGroup <AssociatedGroupType>] [-AddRole <String>] [-RemoveRole <String>] [-Title <String>] [-Owner <String>] [-Description <String>] [-AllowRequestToJoinLeave <Boolean>] [-AutoAcceptRequestToJoinLeave <Boolean>] [-AllowMembersEditMembership <Boolean>] [-OnlyAllowMembersViewMembership <Boolean>] [-RequestToJoinEmail <String>] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AddRole|String|False|Name of the permission set to add to this SharePoint group|
|AllowMembersEditMembership|Boolean|False|A switch parameter that specifies whether group members can modify membership in the group|
|AllowRequestToJoinLeave|Boolean|False|A switch parameter that specifies whether to allow users to request membership in the group and to allow users to request to leave the group|
|AutoAcceptRequestToJoinLeave|Boolean|False|A switch parameter that specifies whether users are automatically added or removed when they make a request|
|Description|String|False|The description for the group|
|Identity|GroupPipeBind|True||
|OnlyAllowMembersViewMembership|Boolean|False|A switch parameter that specifies whether only group members are allowed to view the list of members in the group|
|Owner|String|False|The owner for the group, which can be a user or another group|
|RemoveRole|String|False|Name of the permission set to remove from this SharePoint group|
|RequestToJoinEmail|String|False|The e-mail address to which membership requests are sent|
|SetAssociatedGroup|AssociatedGroupType|False||
|Title|String|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
