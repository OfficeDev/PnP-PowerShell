---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# New-PnPGroup

## SYNOPSIS
Adds group to the Site Groups List and returns a group object

## SYNTAX 

```powershell
New-PnPGroup -Title <String>
             [-Description <String>]
             [-Owner <String>]
             [-AllowRequestToJoinLeave [<SwitchParameter>]]
             [-AutoAcceptRequestToJoinLeave [<SwitchParameter>]]
             [-AllowMembersEditMembership [<SwitchParameter>]]
             [-DisallowMembersViewMembership [<SwitchParameter>]]
             [-RequestToJoinEmail <String>]
             [-Web <WebPipeBind>]
             [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> New-PnPGroup -Title "My Site Users"
```



## PARAMETERS

### -AllowMembersEditMembership
A switch parameter that specifies whether group members can modify membership in the group

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -AllowRequestToJoinLeave
A switch parameter that specifies whether to allow users to request membership in the group and to allow users to request to leave the group

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -AutoAcceptRequestToJoinLeave
A switch parameter that specifies whether users are automatically added or removed when they make a request

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Description
The description for the group

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -DisallowMembersViewMembership
A switch parameter that disallows group members to view membership.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Owner
The owner for the group, which can be a user or another group

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -RequestToJoinEmail
The e-mail address to which membership requests are sent

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Title
The Title of the group

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Connection
Optional connection to be used by cmdlet. Retrieve the value for this parameter by eiter specifying -ReturnConnection on Connect-PnPOnline or by executing Get-PnPConnection.

```yaml
Type: SPOnlineConnection
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Web
The GUID, server relative url (i.e. /sites/team1) or web instance of the web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

## OUTPUTS

### [Microsoft.SharePoint.Client.Group](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.group.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)