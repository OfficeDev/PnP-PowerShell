#Move-PnPFile
Moves a file to a different location
##Syntax
```powershell
Move-PnPFile [-OverwriteIfAlreadyExists [<SwitchParameter>]]
             [-Force [<SwitchParameter>]]
             [-Web <WebPipeBind>]
             -ServerRelativeUrl <String>
             -TargetUrl <String>
```


```powershell
Move-PnPFile [-OverwriteIfAlreadyExists [<SwitchParameter>]]
             [-Force [<SwitchParameter>]]
             [-Web <WebPipeBind>]
             -SiteRelativeUrl <String>
             -TargetUrl <String>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|If provided, no confirmation will be requested and the action will be performed|
|OverwriteIfAlreadyExists|SwitchParameter|False|If provided, if a file already exists at the TargetUrl, it will be overwritten. If ommitted, the move operation will be canceled if the file already exists at the TargetUrl location.|
|ServerRelativeUrl|String|True|Server relative Url specifying the file to move. Must include the file name.|
|SiteRelativeUrl|String|True|Site relative Url specifying the file to move. Must include the file name.|
|TargetUrl|String|True|Server relative Url where to move the file to. Must include the file name.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:>Move-PnPFile -ServerRelativeUrl /sites/project/Documents/company.docx -TargetUrl /sites/otherproject/Documents/company.docx
```
Moves a file named company.docx located in the document library called Documents located in the projects sitecollection under the managed path sites to the site collection otherproject located in the managed path sites. If a file named company.aspx already exists, it won't perform the move.

###Example 2
```powershell
PS:>Move-PnPFile -SiteRelativeUrl Documents/company.aspx -TargetUrl /sites/otherproject/Documents/company.docx
```
Moves a file named company.docx located in the document library called Documents located in the current site to the Documents library in the site collection otherproject located in the managed path sites. If a file named company.aspx already exists, it won't perform the move.

###Example 3
```powershell
PS:>Move-PnPFile -ServerRelativeUrl /sites/project/Documents/company.docx -TargetUrl /sites/otherproject/Documents/company.docx -OverwriteIfAlreadyExists
```
Moves a file named company.docx located in the document library called Documents located in the projects sitecollection under the managed path sites to the site collection otherproject located in the managed path sites. If a file named company.aspx already exists, it will still perform the move and replace the original company.aspx file.
