---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Enable-PnPResponsiveUI

## SYNOPSIS
Activates the PnP Response UI Add-on

## SYNTAX 

```powershell
Enable-PnPResponsiveUI [-InfrastructureSiteUrl <String>]
                       [-Web <WebPipeBind>]
```

## DESCRIPTION
Enables the PnP Responsive UI implementation on a classic SharePoint Site

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Enable-PnPResponsiveUI
```

Will upload a CSS file, a JavaScript file and adds a custom action to the root web of the current site collection, enabling the responsive UI on the site collection. The CSS and JavaScript files are located in the style library, in a folder called SP.Responsive.UI.

## PARAMETERS

### -InfrastructureSiteUrl
A full URL pointing to an infrastructure site. If specified, it will add a custom action pointing to the responsive UI JS code in that site.

```yaml
Type: String
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