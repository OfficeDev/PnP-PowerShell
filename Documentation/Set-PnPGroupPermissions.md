---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPGroupPermissions

## SYNOPSIS
Adds and/or removes permissions of a specific SharePoint group

## SYNTAX 

### ByName
```powershell
Set-PnPGroupPermissions -Identity <GroupPipeBind>
                        [-List <ListPipeBind>]
                        [-AddRole <String[]>]
                        [-RemoveRole <String[]>]
                        [-Web <WebPipeBind>]
                        [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -AddRole Contribute
```

Adds the 'Contribute' permission to the SharePoint group with the name 'My Site Members'

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -RemoveRole 'Full Control' -AddRole 'Read'
```

Removes the 'Full Control' from and adds the 'Contribute' permissions to the SharePoint group with the name 'My Site Members'

### ------------------EXAMPLE 3------------------
```powershell
PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -AddRole @('Contribute', 'Design')
```

Adds the 'Contribute' and 'Design' permissions to the SharePoint group with the name 'My Site Members'

### ------------------EXAMPLE 4------------------
```powershell
PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -RemoveRole @('Contribute', 'Design')
```

Removes the 'Contribute' and 'Design' permissions from the SharePoint group with the name 'My Site Members'

### ------------------EXAMPLE 5------------------
```powershell
PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -List 'MyList' -RemoveRole @('Contribute')
```

Removes the 'Contribute' permissions from the list 'MyList' for the group with the name 'My Site Members'

## PARAMETERS

### -AddRole
Name of the permission set to add to this SharePoint group

```yaml
Type: String[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
Get the permissions of a specific group by name

```yaml
Type: GroupPipeBind
Parameter Sets: ByName
Aliases: Name

Required: True
Position: 0
Accept pipeline input: True
```

### -List
The list to apply the command to.

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -RemoveRole
Name of the permission set to remove from this SharePoint group

```yaml
Type: String[]
Parameter Sets: (All)

Required: False
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