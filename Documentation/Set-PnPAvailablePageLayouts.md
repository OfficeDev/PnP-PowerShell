---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPAvailablePageLayouts

## SYNOPSIS
Sets the available page layouts for the current site

## SYNTAX 

### SPECIFIC
```powershell
Set-PnPAvailablePageLayouts -PageLayouts <String[]>
                            [-Web <WebPipeBind>]
                            [-Connection <SPOnlineConnection>]
```

### ALL
```powershell
Set-PnPAvailablePageLayouts -AllowAllPageLayouts [<SwitchParameter>]
                            [-Web <WebPipeBind>]
                            [-Connection <SPOnlineConnection>]
```

### INHERIT
```powershell
Set-PnPAvailablePageLayouts -InheritPageLayouts [<SwitchParameter>]
                            [-Web <WebPipeBind>]
                            [-Connection <SPOnlineConnection>]
```

## PARAMETERS

### -AllowAllPageLayouts
An array of page layout files to set as available page layouts for the site.

```yaml
Type: SwitchParameter
Parameter Sets: ALL

Required: True
Position: Named
Accept pipeline input: False
```

### -InheritPageLayouts
Set the available page layouts to inherit from the parent site.

```yaml
Type: SwitchParameter
Parameter Sets: INHERIT

Required: True
Position: Named
Accept pipeline input: False
```

### -PageLayouts
An array of page layout files to set as available page layouts for the site.

```yaml
Type: String[]
Parameter Sets: SPECIFIC

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