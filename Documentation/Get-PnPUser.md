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
Get-PnPUser [-Web <WebPipeBind>]
            [-Identity <UserPipeBind>]
```

## DESCRIPTION
This command will return all the users that exist in the current site collection its User Information List

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPUser
```

Returns all users from the User Information List of the current site collection

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