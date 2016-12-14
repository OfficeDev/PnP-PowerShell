#Uninstall-PnPAppInstance
Removes an app from a site
##Syntax
```powershell
Uninstall-PnPAppInstance -Identity <AppPipeBind>
                         [-Force [<SwitchParameter>]]
                         [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Do not ask for confirmation.|
|Identity|AppPipeBind|True|Appinstance or Id of the addin to remove.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Uninstall-PnPAppInstance -Identity $appinstance
```
Uninstalls the app instance which was retrieved with the command Get-PnPAppInstance

###Example 2
```powershell
PS:> Uninstall-PnPAppInstance -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe
```
Uninstalls the app instance with the ID '99a00f6e-fb81-4dc7-8eac-e09c6f9132fe'

###Example 3
```powershell
PS:> Uninstall-PnPAppInstance -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -force
```
Uninstalls the app instance with the ID '99a00f6e-fb81-4dc7-8eac-e09c6f9132fe' and do not ask for confirmation
