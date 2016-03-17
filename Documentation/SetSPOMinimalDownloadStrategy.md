#Set-SPOMinimalDownloadStrategy
Activates or deactivates the minimal downloading strategy.
##Syntax
```powershell
Set-SPOMinimalDownloadStrategy -Off [<SwitchParameter>] [-Force [<SwitchParameter>]] [-Web <WebPipeBind>]
```


```powershell
Set-SPOMinimalDownloadStrategy -On [<SwitchParameter>] [-Force [<SwitchParameter>]] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False||
|Off|SwitchParameter|True||
|On|SwitchParameter|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Set-SPOMinimalDownloadStrategy -Off
```
Will deactivate minimal download strategy (MDS) for the current web.
