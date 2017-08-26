# Get-PnPRequestAccessEmails
Returns the request access e-mail addresses
>*Only available for SharePoint Online*
## Syntax
```powershell
Get-PnPRequestAccessEmails [-Web <WebPipeBind>]
```


## Returns
>List<System.String>

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPRequestAccessEmails
```
This will return all the request access e-mail addresses for the current web
