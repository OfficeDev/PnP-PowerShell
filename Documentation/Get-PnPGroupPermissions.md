---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPGroupPermissions

## SYNOPSIS
Returns the permissions for a specific SharePoint group

## SYNTAX 

### ByName
```powershell
Get-PnPGroupPermissions -Identity <GroupPipeBind>
                        [-Web <WebPipeBind>]
                        [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPGroupPermissions -Identity 'My Site Members'
```

Returns the permissions for the SharePoint group with the name 'My Site Members'

## PARAMETERS

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

### [Microsoft.SharePoint.Client.RoleDefinitionBindingCollection](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.roledefinitionbindingcollection.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)