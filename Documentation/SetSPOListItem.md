#Set-SPOListItem
Updates a list item
##Syntax
```powershell
Set-SPOListItem -Identity <ListItemPipeBind> [-ContentType <ContentTypePipeBind>] [-Values <Hashtable>] [-Web <WebPipeBind>] -List <ListPipeBind>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ContentType|ContentTypePipeBind|False|Specify either the name, ID or an actual content type|
|Identity|ListItemPipeBind|True|The ID of the listitem, or actual ListItem object|
|List|ListPipeBind|True|The ID, Title or Url of the list.|
|Values|Hashtable|False|Use the internal names of the fields when specifying field names|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
Set-SPOListItem -List "Demo List" -Identity 1 -Values @{"Title" = "Test Title"; "Category"="Test Category"}
```
Sets fields value in the list item with ID 1 in the "Demo List". It sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.

###Example 2
```powershell
Set-SPOListItem -List "Demo List" -Identity 1 -ContentType "Company" -Values @{"Title" = "Test Title"; "Category"="Test Category"}
```
Sets fields value in the list item with ID 1 in the "Demo List". It sets the content type of the item to "Company" and it sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.

###Example 3
```powershell
Set-SPOListItem -List "Demo List" -Identity $item -Values @{"Title" = "Test Title"; "Category"="Test Category"}
```
Sets fields value in the list item which has been retrieved by for instance Get-SPOListItem.. It sets the content type of the item to "Company" and it sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.
