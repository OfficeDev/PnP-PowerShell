---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPUser

## SYNOPSIS
Returns site users of current web

## SYNTAX 

```powershell
Get-PnPUser [-Identity <UserPipeBind>]
            [-WithRightsAssigned [<SwitchParameter>]]
            [-Web <WebPipeBind>]
            [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
This command will return all the users that exist in the current site collection its User Information List

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPUser
```

Returns all users from the User Information List of the current site collection regardless if they currently have rights to access the current site

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPUser -Identity 23
```

Returns the user with Id 23 from the User Information List of the current site collection

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPUser -Identity i:0#.f|membership|user@tenant.onmicrosoft.com
```

Returns the user with LoginName i:0#.f|membership|user@tenant.onmicrosoft.com from the User Information List of the current site collection

### ------------------EXAMPLE 4------------------
```powershell
PS:> Get-PnPUser | ? Email -eq "user@tenant.onmicrosoft.com"
```

Returns the user with e-mail address user@tenant.onmicrosoft.com from the User Information List of the current site collection

### ------------------EXAMPLE 5------------------
```powershell
PS:> Get-PnPUser -WithRightsAssigned
```

Returns only those users from the User Information List of the current site collection who currently have any kind of access rights given either directly to the user or Active Directory Group or given to the user or Active Directory Group via membership of a SharePoint Group to the current site

### ------------------EXAMPLE 6------------------
```powershell
PS:> Get-PnPUser -WithRightsAssigned -Web subsite1
```

Returns only those users from the User Information List of the current site collection who currently have any kind of access rights given either directly to the user or Active Directory Group or given to the user or Active Directory Group via membership of a SharePoint Group to subsite 'subsite1'

## PARAMETERS

### -Identity
User ID or login name

```yaml
Type: UserPipeBind
Parameter Sets: (All)

Required: False
Position: 0
Accept pipeline input: True
```

### -WithRightsAssigned
If provided, only users that currently have any kinds of access rights assigned to the current site collection will be returned. Otherwise all users, even those who previously had rights assigned, but not anymore at the moment, will be returned as the information is pulled from the User Information List. Only works if you don't provide an -Identity.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: 1
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

### [Microsoft.SharePoint.Client.User](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.user.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)