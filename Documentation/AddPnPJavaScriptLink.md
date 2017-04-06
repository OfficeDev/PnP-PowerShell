# Add-PnPJavaScriptLink
Adds a link to a JavaScript file to a web or sitecollection
## Syntax
```powershell
Add-PnPJavaScriptLink -Name <String>
                      -Url <String[]>
                      [-Sequence <Int>]
                      [-Scope <CustomActionScope>]
                      [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Name|String|True|Name under which to register the JavaScriptLink|
|Url|String[]|True|URL to the JavaScript file to inject|
|Scope|CustomActionScope|False|Defines if this JavaScript file will be injected to every page within the current site collection or web. All is not allowed in for this command. Default is web.|
|Sequence|Int|False|Sequence of this JavaScript being injected. Use when you have a specific sequence with which to have JavaScript files being added to the page. I.e. jQuery library first and then jQueryUI.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPJavaScriptLink -Name jQuery -Url https://code.jquery.com/jquery.min.js -Sequence 9999 -Scope Site
```
Injects a reference to the latest v1 series jQuery library to all pages within the current site collection under the name jQuery and at order 9999

### Example 2
```powershell
PS:> Add-PnPJavaScriptLink -Name jQuery -Url https://code.jquery.com/jquery.min.js
```
Injects a reference to the latest v1 series jQuery library to all pages within the current web under the name jQuery
