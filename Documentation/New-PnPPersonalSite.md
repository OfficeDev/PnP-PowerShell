---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# New-PnPPersonalSite

## SYNOPSIS
Office365 only: Creates a personal / OneDrive For Business site

## SYNTAX 

```powershell
New-PnPPersonalSite -Email <String[]>
                    [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> $users = ('katiej@contoso.onmicrosoft.com','garth@contoso.onmicrosoft.com')
                 PS:> New-PnPPersonalSite -Email $users
```

Creates a personal / OneDrive For Business site for the 2 users in the variable $users

## PARAMETERS

### -Email
The UserPrincipalName (UPN) of the users

```yaml
Type: String[]
Parameter Sets: (All)

Required: True
Position: 0
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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)