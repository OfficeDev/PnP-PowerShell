---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Set-PnPUserProfileProperty

## SYNOPSIS
Office365 only: Uses the tenant API to retrieve site information.

You must connect to the tenant admin website (https://:<tenant>-admin.sharepoint.com) with Connect-PnPOnline in order to use this command. 


## SYNTAX 

### Single
```powershell
Set-PnPUserProfileProperty -Value <String>
                           -Account <String>
                           -PropertyName <String>
                           [-Connection <SPOnlineConnection>]
```

### Multi
```powershell
Set-PnPUserProfileProperty -Values <String[]>
                           -Account <String>
                           -PropertyName <String>
                           [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Requires a connection to a SharePoint Tenant Admin site.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPUserProfileProperty -Account 'user@domain.com' -Property 'SPS-Location' -Value 'Stockholm'
```

Sets the SPS-Location property for the user as specified by the Account parameter

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPUserProfileProperty -Account 'user@domain.com' -Property 'MyProperty' -Values 'Value 1','Value 2'
```

Sets the MyProperty multi value property for the user as specified by the Account parameter

## PARAMETERS

### -Account
The account of the user, formatted either as a login name, or as a claims identity, e.g. i:0#.f|membership|user@domain.com

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -PropertyName
The property to set, for instance SPS-Skills or SPS-Location

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Value
The value to set in the case of a single value property

```yaml
Type: String
Parameter Sets: Single

Required: True
Position: Named
Accept pipeline input: False
```

### -Values
The values set in the case of a multi value property, e.g. "Value 1","Value 2"

```yaml
Type: String[]
Parameter Sets: Multi

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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)