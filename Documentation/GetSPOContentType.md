#Get-SPOContentType
Retrieves a content type
##Syntax
```powershell
Get-SPOContentType [-Web <WebPipeBind>] [-Identity <ContentTypePipeBind>] [-List <ListPipeBind>] [-InSiteHierarchy [<SwitchParameter>]]
```


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
PS:> Get-SPOContentType 
```
This will get a listing of all available content types within the current web

###Example 2
```powershell
PS:> Get-SPOContentType -InSiteHierarchy
```
This will get a listing of all available content types within the site collection

###Example 3
```powershell
PS:> Get-SPOContentType -Identity "Project Document"
```
This will get a listing of content types within the current context

###Example 4
```powershell
PS:> Get-SPOContentType -List "Documents"
```
This will get a listing of all available content types within the list "Documents"
