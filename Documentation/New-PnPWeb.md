---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# New-PnPWeb

## SYNOPSIS
Creates a new subweb under the current web

## SYNTAX 

```powershell
New-PnPWeb -Title <String>
           -Url <String>
           -Template <String>
           [-Description <String>]
           [-Locale <Int>]
           [-BreakInheritance [<SwitchParameter>]]
           [-InheritNavigation [<SwitchParameter>]]
           [-Web <WebPipeBind>]
           [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> New-PnPWeb -Title "Project A Web" -Url projectA -Description "Information about Project A" -Locale 1033 -Template "STS#0"
```

Creates a new subweb under the current web with URL projectA

## PARAMETERS

### -BreakInheritance
By default the subweb will inherit its security from its parent, specify this switch to break this inheritance

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Description
The description of the new web

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -InheritNavigation
Specifies whether the site inherits navigation.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Locale
The language id of the new web. default = 1033 for English

```yaml
Type: Int
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Template
The site definition template to use for the new web, e.g. STS#0. Use Get-PnPWebTemplates to fetch a list of available templates

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Title
The title of the new web

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Url
The URL of the new web

```yaml
Type: String
Parameter Sets: (All)

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

## OUTPUTS

### [Microsoft.SharePoint.Client.Web](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)