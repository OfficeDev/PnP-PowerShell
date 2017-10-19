---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# New-PnPSite

## SYNOPSIS
BETA: This cmdlet is using early release APIs. Notice that functionality and parameters can change. Creates a new site collection

## SYNTAX 

### Communication Site With Built-in Design
```powershell
New-PnPSite -Type <SiteType>
            -Title <String>
            -Url <String>
            [-Description <String>]
            [-Classification <String>]
            [-AllowFileSharingForGuestUsers [<SwitchParameter>]]
            [-SiteDesign <CommunicationSiteDesign>]
            [-Lcid <UInt32>]
```

### Team Site
```powershell
New-PnPSite -Type <SiteType>
            -Title <String>
            -Alias <String>
            [-Description <String>]
            [-Classification <String>]
            [-IsPublic <String>]
```

### CommunicationCustomInDesign
```powershell
New-PnPSite -Type <SiteType>
            -Title <String>
            -Url <String>
            -SiteDesignId <GuidPipeBind>
            [-Description <String>]
            [-Classification <String>]
            [-AllowFileSharingForGuestUsers [<SwitchParameter>]]
            [-Lcid <UInt32>]
```

## DESCRIPTION
The New-PnPSite cmdlet creates a new site collection for the current tenant. Currently only 'modern' sites like Communication Site and the Modern Team Site are supported. If you want to create a classic site, use New-PnPTenantSite.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso
```

This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'

### ------------------EXAMPLE 2------------------
```powershell
PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -SiteDesign Showcase
```

This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'. It will use the 'Showcase' design for the site.

### ------------------EXAMPLE 3------------------
```powershell
PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -SiteDesignId ae2349d5-97d6-4440-94d1-6516b72449ac
```

This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'. It will use the specified custom site design for the site.

### ------------------EXAMPLE 4------------------
```powershell
PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -Classification "HBI"
```

This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'. The classification for the site will be set to "HBI"

### ------------------EXAMPLE 5------------------
```powershell
PS:> New-PnPSite -Type CommunicationSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -AllowFileSharingForGuestUsers
```

This will create a new Communications Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'. File sharing for guest users will be enabled.

### ------------------EXAMPLE 6------------------
```powershell
PS:> New-PnPSite -Type TeamSite -Title Contoso -Alias contoso
```

This will create a new Modern Team Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso'.

### ------------------EXAMPLE 7------------------
```powershell
PS:> New-PnPSite -Type TeamSite -Title Contoso -Alias contoso -IsPublic
```

This will create a new Modern Team Site collection with the title 'Contoso' and the url 'https://tenant.sharepoint.com/sites/contoso' and sets the site to public.

## PARAMETERS

### -Alias
Specifies the alias of the new site collection

```yaml
Type: String
Parameter Sets: Team Site

Required: True
Position: 0
Accept pipeline input: False
```

### -AllowFileSharingForGuestUsers
Specifies if guest users can share files in the new site collection

```yaml
Type: SwitchParameter
Parameter Sets: Communication Site With Built-in Design

Required: False
Position: 0
Accept pipeline input: False
```

### -Classification
Specifies the classification of the new site collection

```yaml
Type: String
Parameter Sets: Communication Site With Built-in Design

Required: False
Position: 0
Accept pipeline input: False
```

### -Description
Specifies the description of the new site collection

```yaml
Type: String
Parameter Sets: Communication Site With Built-in Design

Required: False
Position: 0
Accept pipeline input: False
```

### -IsPublic
Specifies if new site collection is public. Defaults to false.

```yaml
Type: String
Parameter Sets: Team Site

Required: False
Position: 0
Accept pipeline input: False
```

### -Lcid
Specifies the language of the new site collection. Defaults to the current language of the web connected to.

```yaml
Type: UInt32
Parameter Sets: Communication Site With Built-in Design

Required: False
Position: 0
Accept pipeline input: False
```

### -SiteDesign
Specifies the site design of the new site collection. Defaults to 'Topic'

```yaml
Type: CommunicationSiteDesign
Parameter Sets: Communication Site With Built-in Design

Required: False
Position: 0
Accept pipeline input: False
```

### -SiteDesignId
Specifies the site design id to use for the new site collection. If specified will override SiteDesign

```yaml
Type: GuidPipeBind
Parameter Sets: CommunicationCustomInDesign

Required: True
Position: 0
Accept pipeline input: False
```

### -Title
Specifies the title of the new site collection

```yaml
Type: String
Parameter Sets: Communication Site With Built-in Design

Required: True
Position: 0
Accept pipeline input: False
```

### -Type
@Specifies with type of site to create.

```yaml
Type: SiteType
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Url
Specifies the full url of the new site collection

```yaml
Type: String
Parameter Sets: Communication Site With Built-in Design

Required: True
Position: 0
Accept pipeline input: False
```

## OUTPUTS

### System.String

Returns the url of the newly created site collection

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)