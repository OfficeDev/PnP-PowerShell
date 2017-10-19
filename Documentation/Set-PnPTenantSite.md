---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Set-PnPTenantSite

## SYNOPSIS
Set site information.

## SYNTAX 

```powershell
Set-PnPTenantSite -Url <String>
                  [-Title <String>]
                  [-Sharing <Nullable`1>]
                  [-StorageMaximumLevel <Nullable`1>]
                  [-StorageWarningLevel <Nullable`1>]
                  [-UserCodeMaximumLevel <Nullable`1>]
                  [-UserCodeWarningLevel <Nullable`1>]
                  [-AllowSelfServiceUpgrade <Nullable`1>]
                  [-Owners <List`1>]
                  [-LockState <SiteLockState>]
                  [-NoScriptSite [<SwitchParameter>]]
                  [-Wait [<SwitchParameter>]]
```

## DESCRIPTION
Sets site properties for existing sites.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com -Title "Contoso Website" -Sharing Disabled
```

This will set the title of the site collection with the URL 'https://contoso.sharepoint.com' to 'Contoso Website' and disable sharing on this site collection.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com -Title "Contoso Website" -StorageWarningLevel 8000 -StorageMaximumLevel 10000
```

This will set the title of the site collection with the URL 'https://contoso.sharepoint.com' to 'Contoso Website', set the storage warning level to 8GB and set the storage maximum level to 10GB.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com/sites/sales -Owners "user@contoso.onmicrosoft.com"
```

This will add user@contoso.onmicrosoft.com as an additional site collection owner at 'https://contoso.sharepoint.com/sites/sales'.

### ------------------EXAMPLE 4------------------
```powershell
PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com/sites/sales -Owners @("user1@contoso.onmicrosoft.com", "user2@contoso.onmicrosoft.com")
```

This will add user1@contoso.onmicrosoft.com and user2@contoso.onmicrosoft.com as additional site collection owners at 'https://contoso.sharepoint.com/sites/sales'.

### ------------------EXAMPLE 5------------------
```powershell
PS:> Set-PnPTenantSite -Url https://contoso.sharepoint.com/sites/sales -NoScriptSite:$false
```

This will enable script support for the site 'https://contoso.sharepoint.com/sites/sales' if disabled.

## PARAMETERS

### -AllowSelfServiceUpgrade
Specifies if the site administrator can upgrade the site collection

```yaml
Type: Nullable`1
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -LockState
Sets the lockstate of a site

```yaml
Type: SiteLockState
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -NoScriptSite
Specifies if a site allows custom script or not. See https://support.office.com/en-us/article/Turn-scripting-capabilities-on-or-off-1f2c515f-5d7e-448a-9fd7-835da935584f for more information.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Owners
Specifies owner(s) to add as site collection adminstrators. They will be added as additional site collection administrators. Existing administrators will stay. Can be both users and groups.

```yaml
Type: List`1
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Sharing
Specifies what the sharing capablilites are for the site. Possible values: Disabled, ExternalUserSharingOnly, ExternalUserAndGuestSharing, ExistingExternalUserSharingOnly

```yaml
Type: Nullable`1
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -StorageMaximumLevel
Specifies the storage quota for this site collection in megabytes. This value must not exceed the company's available quota.

```yaml
Type: Nullable`1
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -StorageWarningLevel
Specifies the warning level for the storage quota in megabytes. This value must not exceed the values set for the StorageMaximumLevel parameter

```yaml
Type: Nullable`1
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Title
Specifies the title of the site

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Url
Specifies the URL of the site

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -UserCodeMaximumLevel
Specifies the quota for this site collection in Sandboxed Solutions units. This value must not exceed the company's aggregate available Sandboxed Solutions quota. The default value is 0. For more information, see Resource Usage Limits on Sandboxed Solutions in SharePoint 2010 : http://msdn.microsoft.com/en-us/library/gg615462.aspx.

```yaml
Type: Nullable`1
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -UserCodeWarningLevel
Specifies the warning level for the resource quota. This value must not exceed the value set for the UserCodeMaximumLevel parameter

```yaml
Type: Nullable`1
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Wait
Wait for the operation to complete

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)