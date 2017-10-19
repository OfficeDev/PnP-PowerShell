---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPSitePolicy

## SYNOPSIS
Retrieves all or a specific site policy

## SYNTAX 

```powershell
Get-PnPSitePolicy [-AllAvailable [<SwitchParameter>]]
                  [-Name <String>]
                  [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPSitePolicy
```

Retrieves the current applied site policy.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPSitePolicy -AllAvailable
```

Retrieves all available site policies.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPSitePolicy -Name "Contoso HBI"
```

Retrieves an available site policy with the name "Contoso HBI".

## PARAMETERS

### -AllAvailable
Retrieve all available site policies

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Name
Retrieves a site policy with a specific name

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

## OUTPUTS

### OfficeDevPnP.Core.Entities.SitePolicyEntity

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)