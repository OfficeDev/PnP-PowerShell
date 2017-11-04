---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPUser

## SYNOPSIS
Removes a specific user from the site collection User Information List

## SYNTAX 

```powershell
Remove-PnPUser -Identity <UserPipeBind>
               [-Force [<SwitchParameter>]]
               [-Confirm [<SwitchParameter>]]
               [-Web <WebPipeBind>]
               [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
This command will allow the removal of a specific user from the User Information List

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPUser -Identity 23
```

Remove the user with Id 23 from the User Information List of the current site collection

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPUser -Identity i:0#.f|membership|user@tenant.onmicrosoft.com
```

Remove the user with LoginName i:0#.f|membership|user@tenant.onmicrosoft.com from the User Information List of the current site collection

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPUser | ? Email -eq "user@tenant.onmicrosoft.com" | Remove-PnPUser
```

Remove the user with e-mail address user@tenant.onmicrosoft.com from the User Information List of the current site collection

### ------------------EXAMPLE 4------------------
```powershell
PS:> Remove-PnPUser -Identity i:0#.f|membership|user@tenant.onmicrosoft.com -Confirm:$false
```

Remove the user with LoginName i:0#.f|membership|user@tenant.onmicrosoft.com from the User Information List of the current site collection without asking to confirm the removal first

## PARAMETERS

### -Confirm
Specifying the Confirm parameter will allow the confirmation question to be skipped

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Force
Specifying the Force parameter will skip the confirmation question

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
User ID or login name

```yaml
Type: UserPipeBind
Parameter Sets: (All)

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

### [Microsoft.SharePoint.Client.User](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.user.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)