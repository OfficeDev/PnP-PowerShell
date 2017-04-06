# Remove-PnPPropertyBagValue
Removes a value from the property bag
## Syntax
```powershell
Remove-PnPPropertyBagValue -Key <String>
                           [-Folder <String>]
                           [-Force [<SwitchParameter>]]
                           [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Key|String|True|Key of the property bag value to be removed|
|Folder|String|False|Site relative url of the folder. See examples for use.|
|Force|SwitchParameter|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPPropertyBagValue -Key MyKey
```
This will remove the value with key MyKey from the current web property bag

### Example 2
```powershell
PS:> Remove-PnPPropertyBagValue -Key MyKey -Folder /MyFolder
```
This will remove the value with key MyKey from the folder MyFolder which is located in the root folder of the current web

### Example 3
```powershell
PS:> Remove-PnPPropertyBagValue -Key MyKey -Folder /
```
This will remove the value with key MyKey from the root folder of the current web
