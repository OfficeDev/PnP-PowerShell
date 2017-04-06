# Get-PnPFile
Downloads a file.
## Syntax
```powershell
Get-PnPFile -Url <String>
            [-Web <WebPipeBind>]
```


```powershell
Get-PnPFile -Url <String>
            [-AsListItem [<SwitchParameter>]]
            [-Web <WebPipeBind>]
```


```powershell
Get-PnPFile -Url <String>
            [-AsString [<SwitchParameter>]]
            [-Web <WebPipeBind>]
```


```powershell
Get-PnPFile -AsFile [<SwitchParameter>]
            -Url <String>
            [-Path <String>]
            [-Filename <String>]
            [-Web <WebPipeBind>]
```


## Returns
>[Microsoft.SharePoint.Client.File](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.file.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AsFile|SwitchParameter|True||
|Url|String|True|The URL (server or site relative) to the file|
|AsListItem|SwitchParameter|False||
|AsString|SwitchParameter|False|Retrieve the file contents as a string|
|Filename|String|False|Name for the local file|
|Path|String|False|Local path where the file should be saved|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPFile -Url /sites/project/_catalogs/themes/15/company.spcolor
```
Retrieves the file and downloads it to the current folder

### Example 2
```powershell
PS:> Get-PnPFile -Url /sites/project/_catalogs/themes/15/company.spcolor -Path c:\temp -FileName company.spcolor
```
Retrieves the file and downloads it to c:\temp\company.spcolor

### Example 3
```powershell
PS:> Get-PnPFile -Url /sites/project/_catalogs/themes/15/company.spcolor -AsString
```
Retrieves the file and outputs its contents to the console

### Example 4
```powershell
PS:> Get-PnPFile -Url /sites/project/_catalogs/themes/15/company.spcolor -AsFile
```
Retrieves the file and returns it as a File object

### Example 5
```powershell
PS:> Get-PnPFile -Url /sites/project/_catalogs/themes/15/company.spcolor -AsListItem
```
Retrieves the file and returns it as a ListItem object

### Example 6
```powershell
PS:> Get-PnPFile -Url _catalogs/themes/15/company.spcolor -Path c:\temp -FileName company.spcolor
```
Retrieves the file by site relative URL and downloads it to c:\temp\company.spcolor
