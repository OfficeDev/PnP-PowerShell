#Ensure-SPOFolder
Returns a folder given a site relative path, and will create it does not exist.
##Syntax
```powershell
Ensure-SPOFolder [-Web <WebPipeBind>]
                 -SiteRelativePath <String>
```


##Returns
>[Microsoft.SharePoint.Client.Folder](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.folder.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|SiteRelativePath|String|True|Site Relative Folder Path|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Ensure-SPOFolder -SiteRelativePath "demofolder/subfolder"
```
Creates a folder called subfolder in a folder called demofolder located in the root folder of the site. If the folder hierarchy does not exist, it will be created.
