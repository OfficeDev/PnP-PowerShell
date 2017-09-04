# Test-PnPListItemIsRecord
Checks if a list item is a record
>*Only available for SharePoint Online*
## Syntax
```powershell
Test-PnPListItemIsRecord -Identity <ListItemPipeBind>
                         -List <ListPipeBind>
                         [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ListItemPipeBind|True|The ID of the listitem, or actual ListItem object|
|List|ListPipeBind|True|The ID, Title or Url of the list.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Test-PnPListItemAsRecord -List "Documents" -Identity 4
```
Returns true if the document in the documents library with id 4 is a record
