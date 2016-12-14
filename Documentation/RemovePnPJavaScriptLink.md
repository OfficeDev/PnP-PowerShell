#Remove-PnPJavaScriptLink
Removes a JavaScript link or block from a web or sitecollection
##Syntax
```powershell
Remove-PnPJavaScriptLink [-Force [<SwitchParameter>]]
                         [-Scope <CustomActionScope>]
                         [-Web <WebPipeBind>]
                         [-Name <String>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Use the -Force flag to bypass the confirmation question|
|Name|String|False|Name of the JavaScriptLink to remove. Omit if you want to remove all JavaScript Links.|
|Scope|CustomActionScope|False|Define if the JavaScriptLink is to be found at the web or site collection scope. Specify All to allow deletion from either web or site collection.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Remove-PnPJavaScriptLink -Name jQuery
```
Removes the injected JavaScript file with the name jQuery from the current web after confirmation

###Example 2
```powershell
PS:> Remove-PnPJavaScriptLink -Name jQuery -Scope Site
```
Removes the injected JavaScript file with the name jQuery from the current site collection after confirmation

###Example 3
```powershell
PS:> Remove-PnPJavaScriptLink -Name jQuery -Scope Site -Force
```
Removes the injected JavaScript file with the name jQuery from the current site collection and will not ask for confirmation

###Example 4
```powershell
PS:> Remove-PnPJavaScriptLink -Scope Site
```
Removes all the injected JavaScript files with from the current site collection after confirmation for each of them
