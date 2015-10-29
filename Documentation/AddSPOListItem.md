#Add-SPOListItem
*Topic automatically generated on: 2015-10-13*

Adds an item to a list
##Syntax
```powershell
Add-SPOListItem [-ContentType <ContentTypePipeBind>] [-Values <Hashtable>] [-Web <WebPipeBind>] -List <ListPipeBind>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ContentType|ContentTypePipeBind|False|Specify either the name, ID or an actual content type.|
|List|ListPipeBind|True|The ID, Title or Url of the list.|
|Values|Hashtable|False|Use the internal names of the fields when specifying field names|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
Add-SPOListItem -List "Demo List" -Values @{"Title" = "Test Title"; "Category"="Test Category"}
```
Adds a new list item to the "Demo List", and sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.

###Example 2
```powershell
Add-SPOListItem -List "Demo List" -ContentType "Company" -Values @{"Title" = "Test Title"; "Category"="Test Category"}
```
Adds a new list item to the "Demo List", sets the content type to "Company" and sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.
