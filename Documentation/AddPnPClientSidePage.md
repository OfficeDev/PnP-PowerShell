# Add-PnPClientSidePage
Adds a Client-Side Page
>*Only available for SharePoint Online*
## Syntax
```powershell
Add-PnPClientSidePage -Name <String>
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
|Name|String|True|Specifies the name of the page.|
|CommentsEnabled|SwitchParameter|False|Enables or Disables the comments on the page|
|LayoutType|ClientSidePageLayoutType|False|Specifies the layout type of the page.|
|PromoteAs|ClientSidePagePromoteType|False|Allows to promote the page for a specific purpose (HomePage | NewsPage)|
|Publish|SwitchParameter|False|Publishes the page once it is saved. Applicable to libraries set to create major and minor versions.|
|PublishMessage|String|False|Sets the message for publishing the page.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPClientSidePage -Name "NewPage"
```
Creates a new Client-Side page named 'NewPage'

### Example 2
```powershell
PS:> Add-PnPClientSidePage "NewPage"
```
Creates a new Client-Side page named 'NewPage'
