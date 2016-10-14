#New-SPOGroup
Adds group to the Site Groups List and returns a group object
##Syntax
```powershell
New-SPOGroup -Title <String>
             [-Description <String>]
             [-Owner <String>]
             [-AllowRequestToJoinLeave [<SwitchParameter>]]
             [-AutoAcceptRequestToJoinLeave [<SwitchParameter>]]
             [-AllowMembersEditMembership [<SwitchParameter>]]
             [-DisallowMembersViewMembership [<SwitchParameter>]]
             [-RequestToJoinEmail <String>]
             [-Web <WebPipeBind>]
```


##Returns
>[Microsoft.SharePoint.Client.Group](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.group.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AllowMembersEditMembership|SwitchParameter|False|A switch parameter that specifies whether group members can modify membership in the group|
|AllowRequestToJoinLeave|SwitchParameter|False|A switch parameter that specifies whether to allow users to request membership in the group and to allow users to request to leave the group|
|AutoAcceptRequestToJoinLeave|SwitchParameter|False|A switch parameter that specifies whether users are automatically added or removed when they make a request|
|Description|String|False|The description for the group|
|DisallowMembersViewMembership|SwitchParameter|False|A switch parameter that disallows group members to view membership.|
|Owner|String|False|The owner for the group, which can be a user or another group|
|RequestToJoinEmail|String|False|The e-mail address to which membership requests are sent|
|Title|String|True|The Title of the group|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> New-SPOGroup -Title "My Site Users"
```

