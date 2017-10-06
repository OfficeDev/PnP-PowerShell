# Add-PnPClientSideText
Adds a Client-Side Page
>*Only available for SharePoint Online*
## Syntax
```powershell
Add-PnPClientSideText -Text <String>
                      -Page <ClientSidePagePipeBind>
                      [-Order <Int>]
                      [-Web <WebPipeBind>]
```


```powershell
Add-PnPClientSideText -Text <String>
                      -Section <Int>
                      -Column <Int>
                      -Page <ClientSidePagePipeBind>
                      [-Order <Int>]
                      [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Column|Int|True|Sets the column where to insert the text control.|
|Page|ClientSidePagePipeBind|True|The name of the page.|
|Section|Int|True|Sets the section where to insert the text control.|
|Text|String|True|Specifies the text to display in the text area.|
|Order|Int|False|Sets the order of the text control. (Default = 1)|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPClientSideText -Page "MyPage" -Text "Hello World!"
```
Adds the text 'Hello World!' to the Client-Side Page 'MyPage'
