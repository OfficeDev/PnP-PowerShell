---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPTheme

## SYNOPSIS
Sets the theme of the current web.

## SYNTAX 

```powershell
Set-PnPTheme [-ColorPaletteUrl <String>]
             [-FontSchemeUrl <String>]
             [-BackgroundImageUrl <String>]
             [-ResetSubwebsToInherit [<SwitchParameter>]]
             [-UpdateRootWebOnly [<SwitchParameter>]]
             [-Web <WebPipeBind>]
             [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
 Sets the theme of the current web, if any of the attributes is not set, that value will be set to null

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPTheme
```

Removes the current theme and resets it to the default.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPTheme -ColorPaletteUrl /_catalogs/theme/15/company.spcolor
```



### ------------------EXAMPLE 3------------------
```powershell
PS:> Set-PnPTheme -ColorPaletteUrl /_catalogs/theme/15/company.spcolor -BackgroundImageUrl '/sites/Team Site/style library/background.png'
```



### ------------------EXAMPLE 4------------------
```powershell
PS:> Set-PnPTheme -ColorPaletteUrl /_catalogs/theme/15/company.spcolor -BackgroundImageUrl '/sites/Team Site/style library/background.png' -ResetSubwebsToInherit
```

Sets the theme to the web, and updates all subwebs to inherit the theme from this web.

## PARAMETERS

### -BackgroundImageUrl
Specifies the Background Image Url based on the server relative url

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ColorPaletteUrl
Specifies the Color Palette Url based on the site relative url

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -FontSchemeUrl
Specifies the Font Scheme Url based on the server relative url

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ResetSubwebsToInherit
Resets subwebs to inherit the theme from the rootweb

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -UpdateRootWebOnly
Updates only the rootweb, even if subwebs are set to inherit the theme.

```yaml
Type: SwitchParameter
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