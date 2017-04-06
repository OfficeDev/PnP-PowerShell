# Add-PnPMasterPage
Adds a Masterpage
## Syntax
```powershell
Add-PnPMasterPage -SourceFilePath <String>
                  -Title <String>
                  -Description <String>
                  [-DestinationFolderHierarchy <String>]
                  [-UIVersion <String>]
                  [-DefaultCssFile <String>]
                  [-Web <WebPipeBind>]
```


## Returns
>[Microsoft.SharePoint.Client.File](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.file.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Description|String|True|Description for the Masterpage|
|SourceFilePath|String|True|Path to the file which will be uploaded|
|Title|String|True|Title for the Masterpage|
|DefaultCssFile|String|False|Default CSS file for the MasterPage, this Url is SiteRelative|
|DestinationFolderHierarchy|String|False|Folder hierarchy where the MasterPage will be deployed|
|UIVersion|String|False|UIVersion of the Masterpage. Default = 15|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPMasterPage -SourceFilePath "page.master" -Title "MasterPage" -Description "MasterPage for Web" -DestinationFolderHierarchy "SubFolder"
```
Adds a MasterPage from the local file "page.master" to the folder "SubFolder" in the Masterpage gallery.
