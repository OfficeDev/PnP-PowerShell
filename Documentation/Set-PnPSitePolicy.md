---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPSitePolicy

## SYNOPSIS
Sets a site policy

## SYNTAX 

```powershell
Set-PnPSitePolicy -Name <String>
                  [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPSitePolicy -Name "Contoso HBI"
```

This applies a site policy with the name "Contoso HBI" to the current site. The policy needs to be available in the site.

## PARAMETERS

### -Name
The name of the site policy to apply

```yaml
Type: String
Parameter Sets: (All)

Required: True
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