# Set-PnPField
Changes one or more properties of a field in a specific list or for the whole web
## Syntax
```powershell
Set-PnPField -Values <Hashtable>
             -Identity <FieldPipeBind>
             [-List <ListPipeBind>]
             [-UpdateExistingLists [<SwitchParameter>]]
             [-Web <WebPipeBind>]
```


## Returns
>[Microsoft.SharePoint.Client.Field](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|FieldPipeBind|True|The field object, internal field name or field id to update|
|Values|Hashtable|True|Hashtable of properties to update on the field. Use the syntax @{property1="value";property2="value"}.|
|List|ListPipeBind|False|The list object, name or id where to update the field. If omited the field will be updated on the web.|
|UpdateExistingLists|SwitchParameter|False|If provided, the field will be updated on existing lists that use it as well. If not provided or set to $false, existing lists using the field will remain unchanged but new lists will get the updated field.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Set-PnPField -Identity AssignedTo -Values @{JSLink="customrendering.js";Group="My fields"}
```
Updates the AssignedTo field on the current web to use customrendering.js for the JSLink and sets the group name the field is categorized in to "My Fields". Lists that are already using the AssignedTo field will not be updated.

### Example 2
```powershell
PS:> Set-PnPField -Identity AssignedTo -Values @{JSLink="customrendering.js";Group="My fields"} -UpdateExistingLists
```
Updates the AssignedTo field on the current web to use customrendering.js for the JSLink and sets the group name the field is categorized in to "My Fields". Lists that are already using the AssignedTo field will also be updated.

### Example 3
```powershell
PS:> Set-PnPField -List "Tasks" -Identity "AssignedTo" -Values @{JSLink="customrendering.js"}
```
Updates the AssignedTo field on the Tasks list to use customrendering.js for the JSLink
