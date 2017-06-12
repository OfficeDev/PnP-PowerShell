# Get-PnPListItem
Retrieves list items
## Syntax
```powershell
Get-PnPListItem -List <ListPipeBind>
                [-Id <Int>]
                [-Fields <String[]>]
                [-Web <WebPipeBind>]
```


```powershell
Get-PnPListItem -List <ListPipeBind>
                [-UniqueId <GuidPipeBind>]
                [-Fields <String[]>]
                [-Web <WebPipeBind>]
```


```powershell
Get-PnPListItem -List <ListPipeBind>
                [-Query <String>]
                [-PageSize <Int>]
                [-ScriptBlock <ScriptBlock>]
                [-LimitResults [<SwitchParameter>]]
                [-Web <WebPipeBind>]
```


```powershell
Get-PnPListItem -List <ListPipeBind>
                [-Fields <String[]>]
                [-PageSize <Int>]
                [-ScriptBlock <ScriptBlock>]
                [-LimitResults [<SwitchParameter>]]
                [-Web <WebPipeBind>]
```


## Returns
>[Microsoft.SharePoint.Client.ListItem](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.listitem.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|List|ListPipeBind|True|The list to query|
|Fields|String[]|False|The fields to retrieve. If not specified all fields will be loaded in the returned list object.|
|Id|Int|False|The ID of the item to retrieve|
|LimitResults|SwitchParameter|False|Limit the number of items to the set PageSize, otherwise it will return all results paged.|
|PageSize|Int|False|The number of items to retrieve per page request.|
|Query|String|False|The CAML query to execute against the list|
|ScriptBlock|ScriptBlock|False|The script block to run after every page request.|
|UniqueId|GuidPipeBind|False|The unique id (GUID) of the item to retrieve|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPListItem -List Tasks
```
Retrieves all list items from the Tasks list

### Example 2
```powershell
PS:> Get-PnPListItem -List Tasks -Id 1
```
Retrieves the list item with ID 1 from from the Tasks list. This parameter is ignored if the Query parameter is specified.

### Example 3
```powershell
PS:> Get-PnPListItem -List Tasks -UniqueId bd6c5b3b-d960-4ee7-a02c-85dc6cd78cc3
```
Retrieves the list item with unique id bd6c5b3b-d960-4ee7-a02c-85dc6cd78cc3 from from the tasks lists. This parameter is ignored if the Query parameter is specified.

### Example 4
```powershell
PS:> Get-PnPListItem -List Tasks -Fields "Title","GUID"
```
Retrieves all list items, but only includes the values of the Title and GUID fields in the list item object. This parameter is ignored if the Query parameter is specified.

### Example 5
```powershell
PS:> Get-PnPListItem -List Tasks -Query "<View><Query><Where><Eq><FieldRef Name='GUID'/><Value Type='Guid'>bd6c5b3b-d960-4ee7-a02c-85dc6cd78cc3</Value></Eq></Where></Query></View>"
```
Retrieves all list items based on the CAML query specified.

### Example 6
```powershell
PS:> Get-PnPListItem -List Tasks -PageSize 1000
```
Retrieves all list items from the Tasks list in pages of 1000 items. This parameter is ignored if the Query parameter is specified.

### Example 7
```powershell
PS:> Get-PnPListItem -List Tasks -PageSize 1000 -ScriptBlock { Param($items) $items.Context.ExecuteQuery() } | % { $_.BreakRoleInheritance($true, $true) }
```
Retrieves all list items from the Tasks list in pages of 1000 items and breaks permission inheritance on each item.
