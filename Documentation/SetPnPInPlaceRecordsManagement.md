# Set-PnPInPlaceRecordsManagement
Activates or deactivates in place records management
>*Only available for SharePoint Online*
## Syntax
```powershell
Set-PnPInPlaceRecordsManagement -On [<SwitchParameter>]
                                [-Web <WebPipeBind>]
```


```powershell
Set-PnPInPlaceRecordsManagement -Off [<SwitchParameter>]
                                [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Off|SwitchParameter|True|Turn records management off|
|On|SwitchParameter|True|Turn records management on|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Set-PnPInPlaceRecordsManagement -On
```
Activates In Place Records Management
