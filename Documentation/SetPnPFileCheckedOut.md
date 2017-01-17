#Set-PnPFileCheckedOut
Checks out a file
##Syntax
```powershell
Set-PnPFileCheckedOut [-Web <WebPipeBind>]
                      -Url <String>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Url|String|True|The server relative url of the file to check out|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:>Set-PnPFileCheckedOut -Url "/sites/testsite/subsite/Documents/Contract.docx"
```
Checks out the file "Contract.docx" in the "Documents" library.
