# New-PnPUser
Adds a user to the built-in Site User Info List and returns a user object
## Syntax
```powershell
New-PnPUser -LoginName <String>
            [-Web <WebPipeBind>]
```


## Returns
>[Microsoft.SharePoint.Client.User](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.user.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|LoginName|String|True|The users login name (user@company.com)|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> New-PnPUser -LoginName user@company.com
```
Adds a new user with the login user@company.com to the current site
