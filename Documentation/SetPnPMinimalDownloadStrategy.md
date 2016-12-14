#Set-PnPMinimalDownloadStrategy
Activates or deactivates the minimal downloading strategy.
##Syntax
```powershell
Set-PnPMinimalDownloadStrategy -Off [<SwitchParameter>]
                               [-Force [<SwitchParameter>]]
                               [-Web <WebPipeBind>]
```


```powershell
Set-PnPMinimalDownloadStrategy -On [<SwitchParameter>]
                               [-Force [<SwitchParameter>]]
                               [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question.|
|Off|SwitchParameter|True|Turn minimal download strategy off|
|On|SwitchParameter|True|Turn minimal download strategy on|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Set-PnPMinimalDownloadStrategy -Off
```
Will deactivate minimal download strategy (MDS) for the current web.

###Example 2
```powershell
PS:> Set-PnPMinimalDownloadStrategy -On
```
Will activate minimal download strategy (MDS) for the current web.
