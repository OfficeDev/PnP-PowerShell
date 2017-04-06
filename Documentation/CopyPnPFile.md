# Copy-PnPFile
Copies a file or folder to a different location
## Syntax
```powershell
Copy-PnPFile -SourceUrl <String>
             -TargetUrl <String>
             [-OverwriteIfAlreadyExists [<SwitchParameter>]]
             [-Force [<SwitchParameter>]]
             [-SkipSourceFolderName [<SwitchParameter>]]
             [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|SourceUrl|String|True|Site relative Url specifying the file or folder to copy.|
|TargetUrl|String|True|Server relative Url where to copy the file or folder to.|
|Force|SwitchParameter|False|If provided, no confirmation will be requested and the action will be performed|
|OverwriteIfAlreadyExists|SwitchParameter|False|If provided, if a file already exists at the TargetUrl, it will be overwritten. If ommitted, the copy operation will be canceled if the file already exists at the TargetUrl location.|
|SkipSourceFolderName|SwitchParameter|False|If the source is a folder, the source folder name will not be created, only the contents within it.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl /sites/otherproject/Documents/company.docx
```
Copies a file named company.docx located in a document library called Documents in the current site to the site collection otherproject. If a file named company.docx already exists, it won't perform the copy.

### Example 2
```powershell
PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl Documents/company2.docx
```
Copies a file named company.docx located in a document library called Documents to a new document named company2.docx in the same library.

### Example 3
```powershell
PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl Documents2/company.docx
```
Copies a file named company.docx located in a document library called Documents to a document library called Documents2 in the same site. 

### Example 4
```powershell
PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl Subsite/Documents/company2.docx
```
Copies a file named company.docx located in a document library called Documents to the document library named Document in a subsite named Subsite as a new document named company2.docx.

### Example 5
```powershell
PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl Subsite/Documents
```
Copies a file named company.docx located in a document library called Documents to the document library named Document in a subsite named Subsite keeping the file name.

### Example 6
```powershell
PS:>Copy-PnPFile -SourceUrl Documents/company.docx -TargetUrl /sites/otherproject/Documents/company.docx -OverwriteIfAlreadyExists
```
Copies a file named company.docx located in a document library called Documents in the current site to the site collection otherproject. If a file named company.docx already exists, it will still perform the copy and replace the original company.docx file.

### Example 7
```powershell
PS:>Copy-PnPFile -SourceUrl Documents/MyDocs -TargetUrl /sites/otherproject/Documents -OverwriteIfAlreadyExists
```
Copies a folder named MyDocs in the document library called Documents located in the current site to the site collection otherproject. If the MyDocs folder exist it will copy into it, if not it will be created.

### Example 8
```powershell
PS:>Copy-PnPFile -SourceUrl Documents/MyDocs -TargetUrl /sites/otherproject/Documents -SkipSourceFolderName -OverwriteIfAlreadyExists
```
Copies a folder named MyDocs in the document library called Documents located in the current site to the root folder of the library named Documents in the site collection otherproject.

### Example 9
```powershell
PS:>Copy-PnPFile -SourceUrl Documents/MyDocs -TargetUrl /sites/otherproject/Documents/MyDocs -SkipSourceFolderName -OverwriteIfAlreadyExists
```
Copies a folder named MyDocs in the MyDocs folder of the library named Documents. If the MyDocs folder does not exists, it will be created.

### Example 10
```powershell
PS:>Copy-PnPFile -SourceUrl Documents/MyDocs -TargetUrl /sites/otherproject/Documents/MyDocs -OverwriteIfAlreadyExists
```
Copies a folder named MyDocs in the root of the library named Documents. If the MyDocs folder exists in the target, a subfolder also named MyDocs is created.

### Example 11
```powershell
PS:>Copy-PnPFile -SourceUrl SubSite1/Documents/company.docx -TargetUrl SubSite2/Documents
```
Copies a file named company.docx in the library named Documents in SubSite1 to the library named Documents in SubSite2.
