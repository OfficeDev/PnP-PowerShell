# Get-PnPSubWebs
Returns the subsites of a specific web
## Syntax
```powershell
Get-PnPSubWebs [-Recurse [<SwitchParameter>]]
               [-Web <WebPipeBind>]
               [-Identity <WebPipeBind>]
```


## Detailed Description
This command allows returning all subsites located under the current web

## Returns
>[List<Microsoft.SharePoint.Client.Web>](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|WebPipeBind|False|Relative Url, Id or instance of a pecific web to list the subsites under|
|Recurse|SwitchParameter|False|If provided, all subsites underneath the subsites will be added recursively|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPSubWebs
```
Returns all the subsites directly located under the current web

### Example 2
```powershell
PS:> Get-PnPSubWebs -Recurse
```
Returns all the subsites directly located under the current web and all the subsites underneath those recursively

### Example 3
```powershell
PS:> Get-PnPSubWebs -Identity Project1 -Recurse
```
Returns all the subsites located under the Project1 subsite and all the subsites underneath it recursively
