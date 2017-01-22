#Get-PnPContentTypePublishingHubUrl
Returns the url to Content Type Publishing Hub
##Syntax
##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
##Examples

###Example 1
```powershell
PS:> $url = Get-PnPContentTypePublishingHubUrl
PS:> Connect-PnPOnline -Url $url
PS:> Get-PnPContentType

```
This will retrieve the url to the content type hub, connect to it, and then retrieve the content types form that site
