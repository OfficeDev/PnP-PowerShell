# Set-PnPClientSidePage
Sets parameters of a Client-Side Page
>*Only available for SharePoint Online*
## Syntax
```powershell
Set-PnPClientSidePage -Identity <ClientSidePagePipeBind>
                      [-Name <String>]
                      [-LayoutType <ClientSidePageLayoutType>]
                      [-PromoteAs <ClientSidePagePromoteType>]
                      [-CommentsEnabled [<SwitchParameter>]]
                      [-Publish [<SwitchParameter>]]
                      [-PublishMessage <String>]
                      [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ClientSidePagePipeBind|True|The name/identity of the page|
|CommentsEnabled|SwitchParameter|False|Enables or Disables the comments on the page|
|LayoutType|ClientSidePageLayoutType|False|Sets the layout type of the page. (Default = Article)|
|Name|String|False|Sets the name of the page.|
|PromoteAs|ClientSidePagePromoteType|False|Allows to promote the page for a specific purpose (HomePage | NewsPage)|
|Publish|SwitchParameter|False|Publishes the page once it is saved.|
|PublishMessage|String|False|Sets the message for publishing the page.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Set-PnPClientSidePage -Identity "MyPage" -LayoutType Home
```
Updates the properties of the Client-Side page named 'MyPage'

### Example 2
```powershell
PS:> Set-PnPClientSidePage -Identity "MyPage" -CommentsEnabled
```
Enables the comments on the Client-Side page named 'MyPage'

### Example 3
```powershell
PS:> Set-PnPClientSidePage -Identity "MyPage" -CommentsEnabled $false
```
Disables the comments on the Client-Side page named 'MyPage'
