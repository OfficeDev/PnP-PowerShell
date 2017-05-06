# Get-PnPCustomAction
Returns all or a specific custom action(s)
## Syntax
```powershell
Get-PnPCustomAction [-Identity <GuidPipeBind>]
                    [-Scope <CustomActionScope>]
                    [-Web <WebPipeBind>]
                    [-Includes <String[]>]
```


## Returns
>[List<Microsoft.SharePoint.Client.UserCustomAction>](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.usercustomaction.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|GuidPipeBind|False|Identity of the CustomAction to return. Omit to return all CustomActions.|
|Includes|String[]|False|Specify properties to include when retrieving objects from the server.|
|Scope|CustomActionScope|False|Scope of the CustomAction, either Web, Site or All to return both|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPCustomAction
```
Returns all custom actions of the current site.

### Example 2
```powershell
PS:> Get-PnPCustomAction -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2
```
Returns the custom action with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2'.

### Example 3
```powershell
PS:> Get-PnPCustomAction -Scope web
```
Returns all custom actions for the current web object.
