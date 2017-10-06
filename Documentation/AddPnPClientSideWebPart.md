# Add-PnPClientSideWebPart
Adds a Client-Side Component to a page
>*Only available for SharePoint Online*
## Syntax
```powershell
Add-PnPClientSideWebPart -DefaultWebPartType <DefaultClientSideWebParts>
                         -Page <ClientSidePagePipeBind>
                         [-WebPartProperties <PropertyBagPipeBind>]
                         [-Order <Int>]
                         [-Web <WebPipeBind>]
```


```powershell
Add-PnPClientSideWebPart -Component <ClientSideComponentPipeBind>
                         -Page <ClientSidePagePipeBind>
                         [-WebPartProperties <PropertyBagPipeBind>]
                         [-Order <Int>]
                         [-Web <WebPipeBind>]
```


```powershell
Add-PnPClientSideWebPart -DefaultWebPartType <DefaultClientSideWebParts>
                         -Section <Int>
                         -Column <Int>
                         -Page <ClientSidePagePipeBind>
                         [-WebPartProperties <PropertyBagPipeBind>]
                         [-Order <Int>]
                         [-Web <WebPipeBind>]
```


```powershell
Add-PnPClientSideWebPart -Component <ClientSideComponentPipeBind>
                         -Section <Int>
                         -Column <Int>
                         -Page <ClientSidePagePipeBind>
                         [-WebPartProperties <PropertyBagPipeBind>]
                         [-Order <Int>]
                         [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Column|Int|True|Sets the column where to insert the WebPart control.|
|Component|ClientSideComponentPipeBind|True|Specifies the component instance or Id to add.|
|DefaultWebPartType|DefaultClientSideWebParts|True|Defines a default WebPart type to insert.|
|Page|ClientSidePagePipeBind|True|The name of the page.|
|Section|Int|True|Sets the section where to insert the WebPart control.|
|Order|Int|False|Sets the order of the WebPart control. (Default = 1)|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
|WebPartProperties|PropertyBagPipeBind|False|The properties of the WebPart|
## Examples

### Example 1
```powershell
PS:> Add-PnPClientSideWebPart -Page "MyPage" -DefaultWebPartType BingMap
```
Adds a built-in Client-Side component 'BingMap' to the page called 'MyPage'

### Example 2
```powershell
PS:> Add-PnPClientSideWebPart -Page "MyPage" -Component "HelloWorld"
```
Adds a Client-Side component 'HelloWorld' to the page called 'MyPage'

### Example 3
```powershell
PS:> Add-PnPClientSideWebPart  -Page "MyPage" -Component "HelloWorld" -Section 1 -Column 2
```
Adds a Client-Side component 'HelloWorld' to the page called 'MyPage' in section 1 and column 2
