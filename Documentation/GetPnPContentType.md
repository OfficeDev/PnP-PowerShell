#Get-PnPContentType
Retrieves a content type
##Syntax
```powershell
Get-PnPContentType [-List <ListPipeBind>]
                   [-InSiteHierarchy [<SwitchParameter>]]
                   [-Web <WebPipeBind>]
                   [-Identity <ContentTypePipeBind>]
```


##Returns
>[Microsoft.SharePoint.Client.ContentType](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.contenttype.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ContentTypePipeBind|False|Name or ID of the content type to retrieve|
|InSiteHierarchy|SwitchParameter|False|Search site hierarchy for content types|
|List|ListPipeBind|False|List to query|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-PnPContentType 
```
This will get a listing of all available content types within the current web

###Example 2
```powershell
PS:> Get-PnPContentType -InSiteHierarchy
```
This will get a listing of all available content types within the site collection

###Example 3
```powershell
PS:> Get-PnPContentType -Identity "Project Document"
```
This will get the content type with the name "Project Document" within the current context

###Example 4
```powershell
PS:> Get-PnPContentType -List "Documents"
```
This will get a listing of all available content types within the list "Documents"
