---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPView

## SYNOPSIS
Returns one or all views from a list

## SYNTAX 

### 
```powershell
Get-PnPView -List <ListPipeBind>
            [-Identity <ViewPipeBind>]
            [-Web <WebPipeBind>]
            [-Includes <String[]>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
Get-PnPView -List "Demo List"
```

Returns all views associated from the specified list

### ------------------EXAMPLE 2------------------
```powershell
Get-PnPView -List "Demo List" -Identity "Demo View"
```

Returns the view called "Demo View" from the specified list

### ------------------EXAMPLE 3------------------
```powershell
Get-PnPView -List "Demo List" -Identity "5275148a-6c6c-43d8-999a-d2186989a661"
```

Returns the view with the specified ID from the specified list

## PARAMETERS

### -Identity
The ID or name of the view

```yaml
Type: ViewPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Includes
Specify properties to include when retrieving objects from the server.

```yaml
Type: String[]
Parameter Sets: 

Required: False
Position: 0
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

### -Web
The web to apply the command to. Omit this parameter to use the current web.

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