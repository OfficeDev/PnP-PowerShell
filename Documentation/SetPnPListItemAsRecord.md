# Set-PnPListItemAsRecord
Declares a list item as a record
>*Only available for SharePoint Online*
## Syntax
```powershell
Set-PnPListItemAsRecord -Identity <ListItemPipeBind>
                        -List <ListPipeBind>
                        [-DeclarationDate <DateTime>]
                        [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ListItemPipeBind|True|The ID of the listitem, or actual ListItem object|
|List|ListPipeBind|True|The ID, Title or Url of the list.|
|DeclarationDate|DateTime|False|The declaration date|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Set-PnPListItemAsRecord -List "Documents" -Identity 4
```
Declares the document in the documents library with id 4 as a record

### Example 2
```powershell
PS:> Set-PnPListItemAsRecord -List "Documents" -Identity 4 -DeclarationDate $date
```
Declares the document in the documents library with id as a record
