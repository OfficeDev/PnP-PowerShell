---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPView

## SYNOPSIS
Adds a view to a list

## SYNTAX 

```powershell
Add-PnPView -Title <String>
            -Fields <String[]>
            -List <ListPipeBind>
            [-Query <String>]
            [-ViewType <ViewType>]
            [-RowLimit <UInt32>]
            [-Personal [<SwitchParameter>]]
            [-SetAsDefault [<SwitchParameter>]]
            [-Paged [<SwitchParameter>]]
            [-Web <WebPipeBind>]
            [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
Add-PnPView -List "Demo List" -Title "Demo View" -Fields "Title","Address"
```

Adds a view named "Demo view" to the "Demo List" list.

### ------------------EXAMPLE 2------------------
```powershell
Add-PnPView -List "Demo List" -Title "Demo View" -Fields "Title","Address" -Paged
```

Adds a view named "Demo view" to the "Demo List" list and makes sure there's paging on this view.

## PARAMETERS

### -Fields
A list of fields to add.

```yaml
Type: String[]
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -List
The ID or Url of the list.

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -Paged
If specified, the view will have paging.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Personal
If specified, a personal view will be created.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Query
A valid CAML Query.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -RowLimit
The row limit for the view. Defaults to 30.

```yaml
Type: UInt32
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -SetAsDefault
If specified, the view will be set as the default view for the list.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Title
The title of the view.

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -ViewType
The type of view to add.

```yaml
Type: ViewType
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

### [Microsoft.SharePoint.Client.View](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.view.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)