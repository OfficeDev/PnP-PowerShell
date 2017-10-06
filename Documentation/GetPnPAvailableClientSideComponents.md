# Get-PnPAvailableClientSideComponents
Gets the available client side components on a particular page
>*Only available for SharePoint Online*
## Syntax
```powershell
Get-PnPAvailableClientSideComponents -Page <ClientSidePagePipeBind>
                                     [-Component <ClientSideComponentPipeBind>]
                                     [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Page|ClientSidePagePipeBind|True|The name of the page.|
|Component|ClientSideComponentPipeBind|False|Specifies the component instance or Id to look for.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPAvailableClientSideComponents -Page "MyPage.aspx"
```
Gets the list of available client side components on the page 'MyPage.aspx'

### Example 2
```powershell
PS:> Get-PnPAvailableClientSideComponents $page
```
Gets the list of available client side components on the page contained in the $page variable

### Example 3
```powershell
PS:> Get-PnPAvailableClientSideComponents -Page "MyPage.aspx" -ComponentName "HelloWorld"
```
Gets the client side component 'HelloWorld' if available on the page 'MyPage.aspx'
