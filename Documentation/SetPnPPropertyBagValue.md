# Set-PnPPropertyBagValue
Sets a property bag value
## Syntax
```powershell
Set-PnPPropertyBagValue -Key <String>
                        -Value <String>
                        [-Folder <String>]
                        [-Web <WebPipeBind>]
```


```powershell
Set-PnPPropertyBagValue -Key <String>
                        -Value <String>
                        -Indexed [<SwitchParameter>]
                        [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Indexed|SwitchParameter|True||
|Key|String|True||
|Value|String|True||
|Folder|String|False|Site relative url of the folder. See examples for use.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Set-PnPPropertyBagValue -Key MyKey -Value MyValue
```
This sets or adds a value to the current web property bag

### Example 2
```powershell
PS:> Set-PnPPropertyBagValue -Key MyKey -Value MyValue -Folder /
```
This sets or adds a value to the root folder of the current web

### Example 3
```powershell
PS:> Set-PnPPropertyBagValue -Key MyKey -Value MyValue -Folder /MyFolder
```
This sets or adds a value to the folder MyFolder which is located in the root folder of the current web
