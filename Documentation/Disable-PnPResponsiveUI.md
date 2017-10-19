---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Disable-PnPResponsiveUI

## SYNOPSIS
Deactive the PnP Response UI add-on

## SYNTAX 

```powershell
Disable-PnPResponsiveUI [-Web <WebPipeBind>]
```

## DESCRIPTION
Disables the PnP Responsive UI implementation on a classic SharePoint Site

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Disable-PnPResponsiveUI
```

If enabled previously, this will remove the PnP Responsive UI from a site.

## PARAMETERS

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