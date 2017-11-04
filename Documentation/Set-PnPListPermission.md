---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPListPermission

## SYNOPSIS
Sets list permissions

## SYNTAX 

### Group
```powershell
Set-PnPListPermission -Group <GroupPipeBind>
                      -Identity <ListPipeBind>
                      [-AddRole <String>]
                      [-RemoveRole <String>]
                      [-Web <WebPipeBind>]
                      [-Connection <SPOnlineConnection>]
```

### User
```powershell
Set-PnPListPermission -User <String>
                      -Identity <ListPipeBind>
                      [-AddRole <String>]
                      [-RemoveRole <String>]
                      [-Web <WebPipeBind>]
                      [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPListPermission -Identity 'Documents' -User 'user@contoso.com' -AddRole 'Contribute'
```

Adds the 'Contribute' permission to the user 'user@contoso.com' for the list 'Documents'

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPListPermission -Identity 'Documents' -User 'user@contoso.com' -RemoveRole 'Contribute'
```

Removes the 'Contribute' permission to the user 'user@contoso.com' for the list 'Documents'

## PARAMETERS

### -AddRole
The role that must be assigned to the group or user

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Group


```yaml
Type: GroupPipeBind
Parameter Sets: Group

Required: True
Position: Named
Accept pipeline input: False
```

### -Identity
The ID or Title of the list.

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -RemoveRole
The role that must be removed from the group or user

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -User


```yaml
Type: String
Parameter Sets: User

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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)