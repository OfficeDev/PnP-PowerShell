# Set-PnPFileCheckedIn
Checks in a file
## Syntax
```powershell
Set-PnPFileCheckedIn -Url <String>
                     [-CheckinType <CheckinType>]
                     [-Comment <String>]
                     [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Url|String|True|The server relative url of the file to check in|
|CheckinType|CheckinType|False|The check in type to use. Defaults to Major|
|Comment|String|False|The check in comment|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:>Set-PnPFileCheckedIn -Url "/Documents/Contract.docx"
```
Checks in the file "Contract.docx" in the "Documents" library

### Example 2
```powershell
PS:>Set-PnPFileCheckedIn -Url "/Documents/Contract.docx" -CheckinType MinorCheckin -Comment "Smaller changes"
```
Checks in the file "Contract.docx" in the "Documents" library as a minor version and adds the check in comment "Smaller changes"
