# Get-PnPClientSidePage
Gets a Client-Side Page
>*Only available for SharePoint Online*
## Syntax
```powershell
Get-PnPClientSidePage -Identity <ClientSidePagePipeBind>
                      [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ClientSidePagePipeBind|True|The name of the page|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPClientSidePage -Identity "MyPage.aspx"
```
Gets the Modern Page (Client-Side) named 'MyPage.aspx' in the current SharePoint site

### Example 2
```powershell
PS:> Get-PnPClientSidePage "MyPage"
```
Gets the Modern Page (Client-Side) named 'MyPage.aspx' in the current SharePoint site
