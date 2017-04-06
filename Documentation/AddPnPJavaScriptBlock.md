# Add-PnPJavaScriptBlock
Adds a link to a JavaScript snippet/block to a web or site collection
## Syntax
```powershell
Add-PnPJavaScriptBlock -Name <String>
                       -Script <String>
                       [-Sequence <Int>]
                       [-Scope <CustomActionScope>]
                       [-Web <WebPipeBind>]
```


## Detailed Description
Specify a scope as 'Site' to add the custom action to all sites in a site collection.

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Name|String|True|The name of the script block. Can be used to identify the script with other cmdlets or coded solutions|
|Script|String|True|The javascript block to add to the specified scope|
|Scope|CustomActionScope|False|The scope of the script to add to. Either Web or Site, defaults to Web. 'All' is not valid for this command.|
|Sequence|Int|False|A sequence number that defines the order on the page|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPJavaScriptBlock -Name myAction -script '<script>Alert("This is my Script block");</script>' -Sequence 9999 -Scope Site
```
Add a JavaScript code block  to all pages within the current site collection under the name myAction and at order 9999

### Example 2
```powershell
PS:> Add-PnPJavaScriptBlock -Name myAction -script '<script>Alert("This is my Script block");</script>'
```
Add a JavaScript code block  to all pages within the current web under the name myAction
