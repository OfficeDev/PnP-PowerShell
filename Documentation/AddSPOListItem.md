#Add-SPOListItem
Adds an item to a list
##Syntax
```powershell
Add-SPOListItem [-ContentType <ContentTypePipeBind>]
                [-Values <Hashtable>]
                [-Folder <String>]
                [-Web <WebPipeBind>]
                -List <ListPipeBind>
```


##Returns
>[Microsoft.SharePoint.Client.ListItem](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.listitem.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ContentType|ContentTypePipeBind|False|Specify either the name, ID or an actual content type.|
|Folder|String|False|The list relative URL of a folder. E.g. "MyFolder" for a folder located in the root of the list, or "MyFolder/SubFolder" for a folder located in the MyFolder folder which is located in the root of the list.|
|List|ListPipeBind|True|The ID, Title or Url of the list.|
|Values|Hashtable|False|Use the internal names of the fields when specifying field names.

Single line of text: -Values @{"Title" = "Title New"}

Multiple lines of text: -Values @{"MultiText" = "New text\n\nMore text"}

Rich text: -Values @{"MultiText" = "<strong>New</strong> text"}

Choice: -Values @{"Choice" = "Value 1"}

Number: -Values @{"Number" = "10"}

Currency: -Values @{"Number" = "10"}

Currency: -Values @{"Currency" = "10"}

Date and Time: -Values @{"DateAndTime" = "03/10/2015 14:16"}

Lookup (id of lookup value): -Values @{"Lookup" = "2"}

Yes/No: -Values @{"YesNo" = "False"}

Person/Group (id of user/group in Site User Info List or email of the user, seperate multiple values with a comma): -Values @{"Person" = "user1@domain.com","21"}

Hyperlink or Picture: -Values @{"Hyperlink" = "https://github.com/OfficeDev/, OfficePnp"}|
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

###Example 3
```powershell
Add-SPOListItem -List "Demo List" -Values @{"MultiUserField"="user1@domain.com","user2@domain.com"}
```
Adds a new list item to the "Demo List" and sets the user field called MultiUserField to 2 users. Separate multiple users with a comma.

###Example 4
```powershell
Add-SPOListItem -List "Demo List" -Values @{"Title"="Sales Report"} -Folder "projects/europe"
```
Adds a new list item to the "Demo List". It will add the list item to the europe folder which is located in the projects folder. Folders will be created if needed.
