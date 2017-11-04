---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPSearchConfiguration

## SYNOPSIS
Returns the search configuration

## SYNTAX 

### Xml
```powershell
Get-PnPSearchConfiguration [-Path <String>]
                           [-Scope <SearchConfigurationScope>]
                           [-Web <WebPipeBind>]
                           [-Connection <SPOnlineConnection>]
```

### OutputFormat
```powershell
Get-PnPSearchConfiguration [-OutputFormat <OutputFormat>]
                           [-Scope <SearchConfigurationScope>]
                           [-Web <WebPipeBind>]
                           [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPSearchConfiguration
```

Returns the search configuration for the current web

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPSearchConfiguration -Scope Site
```

Returns the search configuration for the current site collection

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPSearchConfiguration -Scope Subscription
```

Returns the search configuration for the current tenant

### ------------------EXAMPLE 4------------------
```powershell
PS:> Get-PnPSearchConfiguration -Path searchconfig.xml -Scope Subscription
```

Returns the search configuration for the current tenant and saves it to the specified file

### ------------------EXAMPLE 5------------------
```powershell
PS:> Get-PnPSearchConfiguration -Scope Site -OutputFormat ManagedPropertyMappings
```

Returns all custom managed properties and crawled property mapping at the current site collection

## PARAMETERS

### -OutputFormat
Output format for of the configuration. Defaults to complete XML

```yaml
Type: OutputFormat
Parameter Sets: OutputFormat

Required: False
Position: Named
Accept pipeline input: False
```

### -Path
Local path where the search configuration will be saved

```yaml
Type: String
Parameter Sets: Xml

Required: False
Position: Named
Accept pipeline input: False
```

### -Scope
Scope to use. Either Web, Site, or Subscription. Defaults to Web

```yaml
Type: SearchConfigurationScope
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

## OUTPUTS

### System.String

Does not return a string when the -Path parameter has been specified.

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)