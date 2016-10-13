#Find-SPOFile
Finds a file in the virtual file system of the web.
##Syntax
```powershell
Find-SPOFile -Folder <FolderPipeBind>
             [-Web <WebPipeBind>]
             -Match <String>
```


```powershell
Find-SPOFile -List <ListPipeBind>
             [-Web <WebPipeBind>]
             -Match <String>
```


```powershell
Find-SPOFile [-Web <WebPipeBind>]
             -Match <String>
```


##Returns
>[Microsoft.SharePoint.Client.File](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.file.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Folder|FolderPipeBind|True|Folder object or relative url of a folder to query|
|List|ListPipeBind|True|List title, url or an actual List object to query|
|Match|String|True|Wildcard query|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Find-SPOFile -Match *.master
```
Will return all masterpages located in the current web.

###Example 2
```powershell
PS:> Find-SPOFile -List "Documents" -Match *.pdf
```
Will return all pdf files located in given list.

###Example 3
```powershell
PS:> Find-SPOFile -Folder "Shared Documents/Sub Folder" -Match *.docx
```
Will return all docx files located in given folder.
