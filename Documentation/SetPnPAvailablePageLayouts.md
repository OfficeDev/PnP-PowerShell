# Set-PnPAvailablePageLayouts
Sets the available page layouts for the current site
## Syntax
```powershell
Set-PnPAvailablePageLayouts -PageLayouts <String[]>
                            [-Web <WebPipeBind>]
```


```powershell
Set-PnPAvailablePageLayouts -AllowAllPageLayouts [<SwitchParameter>]
                            [-Web <WebPipeBind>]
```


```powershell
Set-PnPAvailablePageLayouts -InheritPageLayouts [<SwitchParameter>]
                            [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AllowAllPageLayouts|SwitchParameter|True|An array of page layout files to set as available page layouts for the site.|
|InheritPageLayouts|SwitchParameter|True|Set the available page layouts to inherit from the parent site.|
|PageLayouts|String[]|True|An array of page layout files to set as available page layouts for the site.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
