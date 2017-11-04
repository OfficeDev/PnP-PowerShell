---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPListItem

## SYNOPSIS
Retrieves list items

## SYNTAX 

### By Id
```powershell
Get-PnPListItem -List <ListPipeBind>
                [-Id <Int>]
                [-Fields <String[]>]
                [-Web <WebPipeBind>]
                [-Connection <SPOnlineConnection>]
```

### By Unique Id
```powershell
Get-PnPListItem -List <ListPipeBind>
                [-UniqueId <GuidPipeBind>]
                [-Fields <String[]>]
                [-Web <WebPipeBind>]
                [-Connection <SPOnlineConnection>]
```

### By Query
```powershell
Get-PnPListItem -List <ListPipeBind>
                [-Query <String>]
                [-PageSize <Int>]
                [-ScriptBlock <ScriptBlock>]
                [-Web <WebPipeBind>]
                [-Connection <SPOnlineConnection>]
```

### All Items
```powershell
Get-PnPListItem -List <ListPipeBind>
                [-Fields <String[]>]
                [-PageSize <Int>]
                [-ScriptBlock <ScriptBlock>]
                [-Web <WebPipeBind>]
                [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPListItem -List Tasks
```

Retrieves all list items from the Tasks list

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPListItem -List Tasks -Id 1
```

Retrieves the list item with ID 1 from from the Tasks list

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPListItem -List Tasks -UniqueId bd6c5b3b-d960-4ee7-a02c-85dc6cd78cc3
```

Retrieves the list item with unique id bd6c5b3b-d960-4ee7-a02c-85dc6cd78cc3 from from the tasks lists

### ------------------EXAMPLE 4------------------
```powershell
PS:> (Get-PnPListItem -List Tasks -Fields "Title","GUID").FieldValues
```

Retrieves all list items, but only includes the values of the Title and GUID fields in the list item object

### ------------------EXAMPLE 5------------------
```powershell
PS:> Get-PnPListItem -List Tasks -Query "<View><Query><Where><Eq><FieldRef Name='GUID'/><Value Type='Guid'>bd6c5b3b-d960-4ee7-a02c-85dc6cd78cc3</Value></Eq></Where></Query></View>"
```

Retrieves all list items based on the CAML query specified

### ------------------EXAMPLE 6------------------
```powershell
PS:> Get-PnPListItem -List Tasks -PageSize 1000
```

Retrieves all list items from the Tasks list in pages of 1000 items

### ------------------EXAMPLE 7------------------
```powershell
PS:> Get-PnPListItem -List Tasks -PageSize 1000 -ScriptBlock { Param($items) $items.Context.ExecuteQuery() } | % { $_.BreakRoleInheritance($true, $true) }
```

Retrieves all list items from the Tasks list in pages of 1000 items and breaks permission inheritance on each item

## PARAMETERS

### -Fields
The fields to retrieve. If not specified all fields will be loaded in the returned list object.

```yaml
Type: String[]
Parameter Sets: All Items

Required: False
Position: Named
Accept pipeline input: False
```

### -Id
The ID of the item to retrieve

```yaml
Type: Int
Parameter Sets: By Id

Required: False
Position: Named
Accept pipeline input: False
```

### -List
The list to query

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: False
```

### -PageSize
The number of items to retrieve per page request.

```yaml
Type: Int
Parameter Sets: All Items

Required: False
Position: Named
Accept pipeline input: False
```

### -Query
The CAML query to execute against the list

```yaml
Type: String
Parameter Sets: By Query

Required: False
Position: Named
Accept pipeline input: False
```

### -ScriptBlock
The script block to run after every page request.

```yaml
Type: ScriptBlock
Parameter Sets: All Items

Required: False
Position: Named
Accept pipeline input: False
```

### -UniqueId
The unique id (GUID) of the item to retrieve

```yaml
Type: GuidPipeBind
Parameter Sets: By Unique Id

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