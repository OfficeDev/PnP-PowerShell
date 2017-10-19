---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPView

## SYNOPSIS
Change view properties

## SYNTAX 

```powershell
Set-PnPView -Identity <ViewPipeBind>
            -Values <Hashtable>
            [-Web <WebPipeBind>]
            [-List <ListPipeBind>]
```

## DESCRIPTION
Sets one or more properties of an existing view.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPView -List "Tasks" -Identity "All Tasks" -Values @{JSLink="hierarchytaskslist.js|customrendering.js";Title="My view"}
```

Updates the "All Tasks" view on list "Tasks" to use hierarchytaskslist.js and customrendering.js for the JSLink and changes the title of the view to "My view"

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPList -Identity "Tasks" | Get-PnPView | Set-PnPView -Values @{JSLink="hierarchytaskslist.js|customrendering.js"}
```

Updates all views on list "Tasks" to use hierarchytaskslist.js and customrendering.js for the JSLink

## PARAMETERS

### -Identity
The Id, Title or instance of the view

```yaml
Type: ViewPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: True
```

### -List
The Id, Title or Url of the list

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: False
Position: 0
Accept pipeline input: False
```

### -Values
Hashtable of properties to update on the view. Use the syntax @{property1="value";property2="value"}.

```yaml
Type: Hashtable
Parameter Sets: (All)

Required: True
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

### [Microsoft.SharePoint.Client.Field](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.view.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)