---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPListItem

## SYNOPSIS
Adds an item to a list

## SYNTAX 

```powershell
Add-PnPListItem -List <ListPipeBind>
                [-ContentType <ContentTypePipeBind>]
                [-Values <Hashtable>]
                [-Folder <String>]
                [-Web <WebPipeBind>]
                [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
Add-PnPListItem -List "Demo List" -Values @{"Title" = "Test Title"; "Category"="Test Category"}
```

Adds a new list item to the "Demo List", and sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.

### ------------------EXAMPLE 2------------------
```powershell
Add-PnPListItem -List "Demo List" -ContentType "Company" -Values @{"Title" = "Test Title"; "Category"="Test Category"}
```

Adds a new list item to the "Demo List", sets the content type to "Company" and sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.

### ------------------EXAMPLE 3------------------
```powershell
Add-PnPListItem -List "Demo List" -Values @{"MultiUserField"="user1@domain.com","user2@domain.com"}
```

Adds a new list item to the "Demo List" and sets the user field called MultiUserField to 2 users. Separate multiple users with a comma.

### ------------------EXAMPLE 4------------------
```powershell
Add-PnPListItem -List "Demo List" -Values @{"Title"="Sales Report"} -Folder "projects/europe"
```

Adds a new list item to the "Demo List". It will add the list item to the europe folder which is located in the projects folder. Folders will be created if needed.

## PARAMETERS

### -ContentType
Specify either the name, ID or an actual content type.

```yaml
Type: ContentTypePipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Folder
The list relative URL of a folder. E.g. "MyFolder" for a folder located in the root of the list, or "MyFolder/SubFolder" for a folder located in the MyFolder folder which is located in the root of the list.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -List
The ID, Title or Url of the list.

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -Values
Use the internal names of the fields when specifying field names.

Single line of text: -Values @{"Title" = "Title New"}

Multiple lines of text: -Values @{"MultiText" = "New text\n\nMore text"}

Rich text: -Values @{"MultiText" = "<strong>New</strong> text"}

Choice: -Values @{"Choice" = "Value 1"}

Number: -Values @{"Number" = "10"}

Currency: -Values @{"Number" = "10"}

Currency: -Values @{"Currency" = "10"}

Date and Time: -Values @{"DateAndTime" = "03/10/2015 14:16"}

Lookup (id of lookup value): -Values @{"Lookup" = "2"}

Multi value lookup (id of lookup values as array 1): -Values @{"MultiLookupField" = "1","2"}

Multi value lookup (id of lookup values as array 2): -Values @{"MultiLookupField" = 1,2}

Multi value lookup (id of lookup values as string): -Values @{"MultiLookupField" = "1,2"}

Yes/No: -Values @{"YesNo" = $false}

Person/Group (id of user/group in Site User Info List or email of the user, seperate multiple values with a comma): -Values @{"Person" = "user1@domain.com","21"}

Managed Metadata (single value with path to term): -Values @{"MetadataField" = "CORPORATE|DEPARTMENTS|FINANCE"}

Managed Metadata (single value with id of term): -Values @{"MetadataField" = "fe40a95b-2144-4fa2-b82a-0b3d0299d818"} with Id of term

Managed Metadata (multiple values with paths to terms): -Values @{"MetadataField" = "CORPORATE|DEPARTMENTS|FINANCE","CORPORATE|DEPARTMENTS|HR"}

Managed Metadata (multiple values with ids of terms): -Values @{"MetadataField" = "fe40a95b-2144-4fa2-b82a-0b3d0299d818","52d88107-c2a8-4bf0-adfa-04bc2305b593"}

Hyperlink or Picture: -Values @{"Hyperlink" = "https://github.com/OfficeDev/, OfficePnp"}

```yaml
Type: Hashtable
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Connection
Optional connection to be used by cmdlet. Retrieve the value for this parameter by eiter specifying -ReturnConnection on Connect-PnPOnline or by executing Get-PnPConnection.

```yaml
Type: SPOnlineConnection
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Web
The GUID, server relative url (i.e. /sites/team1) or web instance of the web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

## OUTPUTS

### [Microsoft.SharePoint.Client.ListItem](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.listitem.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)