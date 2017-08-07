# Rename-PnPFile
Renames a file in its current location
## Syntax
```powershell
Rename-PnPFile -ServerRelativeUrl <String>
               -TargetFileName <String>
               [-OverwriteIfAlreadyExists [<SwitchParameter>]]
               [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
```


```powershell
Rename-PnPFile -SiteRelativeUrl <String>
               -TargetFileName <String>
               [-OverwriteIfAlreadyExists [<SwitchParameter>]]
               [-Force [<SwitchParameter>]]
               [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ServerRelativeUrl|String|True|Server relative Url specifying the file to rename. Must include the file name.|
|SiteRelativeUrl|String|True|Site relative Url specifying the file to rename. Must include the file name.|
|TargetFileName|String|True|File name to rename the file to. Should only be the file name and not include the path to its location. Use Move-PnPFile to move the file to another location.|
|Force|SwitchParameter|False|If provided, no confirmation will be requested and the action will be performed|
|OverwriteIfAlreadyExists|SwitchParameter|False|If provided, if a file already exist with the provided TargetFileName, it will be overwritten. If ommitted, the rename operation will be canceled if a file already exists with the TargetFileName file name.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:>Rename-PnPFile -ServerRelativeUrl /sites/project/Documents/company.docx -TargetFileName mycompany.docx
```
Renames a file named company.docx located in the document library called Documents located in the projects sitecollection under the managed path sites to mycompany.docx. If a file named mycompany.aspx already exists, it won't perform the rename.

### Example 2
```powershell
PS:>Rename-PnPFile -SiteRelativeUrl Documents/company.aspx -TargetFileName mycompany.docx
```
Renames a file named company.docx located in the document library called Documents located in the current site to mycompany.aspx. If a file named mycompany.aspx already exists, it won't perform the rename.

### Example 3
```powershell
PS:>Rename-PnPFile -ServerRelativeUrl /sites/project/Documents/company.docx -TargetFileName mycompany.docx -OverwriteIfAlreadyExists
```
Renames a file named company.docx located in the document library called Documents located in the projects sitecollection under the managed path sites to mycompany.aspx. If a file named mycompany.aspx already exists, it will still perform the rename and replace the original mycompany.aspx file.
