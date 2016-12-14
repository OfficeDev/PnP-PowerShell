#Get-PnPSearchConfiguration
Returns the search configuration
##Syntax
```powershell
Get-PnPSearchConfiguration [-Scope <SearchConfigurationScope>]
                           [-Path <String>]
                           [-Web <WebPipeBind>]
```


##Returns
>System.String

Does not return a string when the -Path parameter has been specified.

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Path|String|False|Local path where the search configuration will be saved|
|Scope|SearchConfigurationScope|False|Scope to use. Either Web, Site, or Subscription. Defaults to Web|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-PnPSearchConfiguration
```
Returns the search configuration for the current web

###Example 2
```powershell
PS:> Get-PnPSearchConfiguration -Scope Site
```
Returns the search configuration for the current site collection

###Example 3
```powershell
PS:> Get-PnPSearchConfiguration -Scope Subscription
```
Returns the search configuration for the current tenant

###Example 4
```powershell
PS:> Get-PnPSearchConfiguration -Path searchconfig.xml -Scope Subscription
```
Returns the search configuration for the current tenant and saves it to the specified file
