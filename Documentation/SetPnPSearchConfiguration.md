# Set-PnPSearchConfiguration
Sets the search configuration
## Syntax
```powershell
Set-PnPSearchConfiguration -Configuration <String>
                           [-Scope <SearchConfigurationScope>]
                           [-Web <WebPipeBind>]
```


```powershell
Set-PnPSearchConfiguration -Path <String>
                           [-Scope <SearchConfigurationScope>]
                           [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Configuration|String|True|Search configuration string|
|Path|String|True|Path to a search configuration|
|Scope|SearchConfigurationScope|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Set-PnPSearchConfiguration -Configuration $config
```
Sets the search configuration for the current web

### Example 2
```powershell
PS:> Set-PnPSearchConfiguration -Configuration $config -Scope Site
```
Sets the search configuration for the current site collection

### Example 3
```powershell
PS:> Set-PnPSearchConfiguration -Configuration $config -Scope Subscription
```
Sets the search configuration for the current tenant

### Example 4
```powershell
PS:> Set-PnPSearchConfiguration -Path searchconfig.xml -Scope Subscription
```
Reads the search configuration from the specified XML file and sets it for the current tenant
