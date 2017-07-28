# Set-PnPField
Changes a property of a field from a list or site
## Syntax
```powershell
Set-PnPField -Values <Hashtable>
             -Identity <FieldPipeBind>
             [-List <ListPipeBind>]
             [-Web <WebPipeBind>]
```


## Returns
>[Microsoft.SharePoint.Client.Field](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|FieldPipeBind|True|The field object or name to get|
|Values|Hashtable|True|Hashtable of properties to update on the field. Use the internal names of the fields when specifying field names or use the field id.|
|List|ListPipeBind|False|The list object or name where to get the field from|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Set-PnPField
```
Gets all the fields from the current site

### Example 2
```powershell
PS:> Get-PnPField -List "Demo list" -Identity "Speakers"
```
Gets the speakers field from the list Demo list
