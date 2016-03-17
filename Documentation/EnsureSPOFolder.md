#Ensure-SPOFolder
Returns a folder given a site relative path, and will create it if it not exists.
##Syntax
```powershell
Ensure-SPOFolder -SiteRelativePath <String> [-Web <WebPipeBind>]
```


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
Creates a folder called subfolder in a folder called demofolder with located in the root folder of the site. If the folder hierarchy does not exist, it will be created.
