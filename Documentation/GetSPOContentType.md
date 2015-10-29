#Get-SPOContentType
*Topic automatically generated on: 2015-10-13*

Retrieves a content type
##Syntax
```powershell
Get-SPOContentType [-Web <WebPipeBind>] [-Identity <ContentTypePipeBind>] [-InSiteHierarchy [<SwitchParameter>]]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ContentTypePipeBind|False|Name or ID of the content type to retrieve|
|InSiteHierarchy|SwitchParameter|False|Search site hierarchy for content types|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-SPOContentType -Identity "Project Document"
```
This will get a listing of content types within the current context
