# Set-PnPListItem
Updates a list item
## Syntax
```powershell
Set-PnPListItem -Identity <ListItemPipeBind>
                -List <ListPipeBind>
                [-ContentType <ContentTypePipeBind>]
                [-Values <Hashtable>]
                [-SystemUpdate [<SwitchParameter>]]
                [-Web <WebPipeBind>]
```


## Returns
>[Microsoft.SharePoint.Client.ListItem](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.listitem.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ListItemPipeBind|True|The ID of the listitem, or actual ListItem object|
|List|ListPipeBind|True|The ID, Title or Url of the list.|
|ContentType|ContentTypePipeBind|False|Specify either the name, ID or an actual content type|
|SystemUpdate|SwitchParameter|False|Updating item without updating the modified and modified by fields|
|Values|Hashtable|False|Use the internal names of the fields when specifying field names.

Single line of text: -Values @{"TextField" = "Title New"}

Multiple lines of text: -Values @{"MultiTextField" = "New text\n\nMore text"}

Rich text: -Values @{"MultiTextField" = "<strong>New</strong> text"}

Choice: -Values @{"ChoiceField" = "Value 1"}

Number: -Values @{"NumberField" = "10"}

Currency: -Values @{"NumberField" = "10"}

Currency: -Values @{"CurrencyField" = "10"}

Date and Time: -Values @{"DateAndTimeField" = "03/10/2015 14:16"}

Lookup (id of lookup value): -Values @{"LookupField" = "2"}

Multi value lookup (id of lookup values as array 1): -Values @{"MultiLookupField" = "1","2"}

Multi value lookup (id of lookup values as array 2): -Values @{"MultiLookupField" = 1,2}

Multi value lookup (id of lookup values as string): -Values @{"MultiLookupField" = "1,2"}

Yes/No: -Values @{"YesNoField" = $false}

Person/Group (id of user/group in Site User Info List or email of the user, seperate multiple values with a comma): -Values @{"PersonField" = "user1@domain.com","21"}

Managed Metadata (single value with path to term): -Values @{"MetadataField" = "CORPORATE|DEPARTMENTS|FINANCE"}

Managed Metadata (single value with id of term): -Values @{"MetadataField" = "fe40a95b-2144-4fa2-b82a-0b3d0299d818"} with Id of term

Managed Metadata (multiple values with paths to terms): -Values @{"MetadataField" = "CORPORATE|DEPARTMENTS|FINANCE","CORPORATE|DEPARTMENTS|HR"}

Managed Metadata (multiple values with ids of terms): -Values @{"MetadataField" = "fe40a95b-2144-4fa2-b82a-0b3d0299d818","52d88107-c2a8-4bf0-adfa-04bc2305b593"}

Hyperlink or Picture: -Values @{"HyperlinkField" = "https://github.com/OfficeDev/, OfficePnp"}|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
Set-PnPListItem -List "Demo List" -Identity 1 -Values @{"Title" = "Test Title"; "Category"="Test Category"}
```
Sets fields value in the list item with ID 1 in the "Demo List". It sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.

### Example 2
```powershell
Set-PnPListItem -List "Demo List" -Identity 1 -ContentType "Company" -Values @{"Title" = "Test Title"; "Category"="Test Category"}
```
Sets fields value in the list item with ID 1 in the "Demo List". It sets the content type of the item to "Company" and it sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.

### Example 3
```powershell
Set-PnPListItem -List "Demo List" -Identity $item -Values @{"Title" = "Test Title"; "Category"="Test Category"}
```
Sets fields value in the list item which has been retrieved by for instance Get-PnPListItem. It sets the content type of the item to "Company" and it sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.
