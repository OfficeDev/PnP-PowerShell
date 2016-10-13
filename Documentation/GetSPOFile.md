#Get-SPOFile
Downloads a file.
##Syntax
```powershell
Get-SPOFile [-Path <String>]
            [-Filename <String>]
            [-Web <WebPipeBind>]
            -ServerRelativeUrl <String>
```


```powershell
Get-SPOFile [-AsFile [<SwitchParameter>]]
            [-Web <WebPipeBind>]
            -ServerRelativeUrl <String>
```


```powershell
Get-SPOFile [-AsListItem [<SwitchParameter>]]
            [-Web <WebPipeBind>]
            -ServerRelativeUrl <String>
```


```powershell
Get-SPOFile [-AsString [<SwitchParameter>]]
            [-Web <WebPipeBind>]
            -ServerRelativeUrl <String>
```


```powershell
Get-SPOFile [-Path <String>]
            [-Filename <String>]
            [-Web <WebPipeBind>]
            -SiteRelativeUrl <String>
```


```powershell
Get-SPOFile [-AsFile [<SwitchParameter>]]
            [-Web <WebPipeBind>]
            -SiteRelativeUrl <String>
```


```powershell
Get-SPOFile [-AsListItem [<SwitchParameter>]]
            [-Web <WebPipeBind>]
            -SiteRelativeUrl <String>
```


```powershell
Get-SPOFile [-AsString [<SwitchParameter>]]
            [-Web <WebPipeBind>]
            -SiteRelativeUrl <String>
```


##Returns
>[Microsoft.SharePoint.Client.File](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.file.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AsFile|SwitchParameter|False||
|AsListItem|SwitchParameter|False||
|AsString|SwitchParameter|False||
|Filename|String|False||
|Path|String|False||
|ServerRelativeUrl|String|True||
|SiteRelativeUrl|String|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-SPOFile -ServerRelativeUrl /sites/project/_catalogs/themes/15/company.spcolor
```
Retrieves the file and downloads it to the current folder

###Example 2
```powershell
PS:> Get-SPOFile -ServerRelativeUrl /sites/project/_catalogs/themes/15/company.spcolor -Path c:\temp -FileName company.spcolor
```
Retrieves the file and downloads it to c:\temp\company.spcolor

###Example 3
```powershell
PS:> Get-SPOFile -ServerRelativeUrl /sites/project/_catalogs/themes/15/company.spcolor -AsString
```
Retrieves the file and outputs its contents to the console

###Example 4
```powershell
PS:> Get-SPOFile -ServerRelativeUrl /sites/project/_catalogs/themes/15/company.spcolor -AsFile
```
Retrieves the file and returns it as a File object

###Example 5
```powershell
PS:> Get-SPOFile -ServerRelativeUrl /sites/project/_catalogs/themes/15/company.spcolor -AsListItem
```
Retrieves the file and returns it as a ListItem object

###Example 6
```powershell
PS:> Get-SPOFile -SiteRelativeUrl _catalogs/themes/15/company.spcolor -Path c:\temp -FileName company.spcolor
```
Retrieves the file by site relative URL and downloads it to c:\temp\company.spcolor
