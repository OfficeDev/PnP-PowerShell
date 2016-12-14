#Get-PnPDocumentSetTemplate
Retrieves a document set template
##Syntax
```powershell
Get-PnPDocumentSetTemplate [-Web <WebPipeBind>]
                           -Identity <DocumentSetPipeBind>
```


##Returns
>[Microsoft.SharePoint.Client.DocumentSet.DocumentSetTemplate](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.documentset.documentsettemplate.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|DocumentSetPipeBind|True|Either specify a name, an id, a document set template object or a content type object|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-PnPDocumentSetTemplate -Identity "Test Document Set"
```
This will get the document set template with the name "Test Document Set"

###Example 2
```powershell
PS:> Get-PnPDocumentSetTemplate -Identity "0x0120D520005DB65D094035A241BAC9AF083F825F3B"
```
This will get the document set template with the id "0x0120D520005DB65D094035A241BAC9AF083F825F3B"
