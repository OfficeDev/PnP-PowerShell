# Set-PnPView
Changes one or more properties of a specific view
## Syntax
```powershell
Set-PnPView -Identity <ViewPipeBind>
            -Values <Hashtable>
            [-Web <WebPipeBind>]
            [-List <ListPipeBind>]
```


## Returns
>[Microsoft.SharePoint.Client.Field](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.view.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ViewPipeBind|True|The Id, Title or instance of the view|
|Values|Hashtable|True|Hashtable of properties to update on the view. Use the syntax @{property1="value";property2="value"}.|
|List|ListPipeBind|False|The Id, Title or Url of the list|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Set-PnPView -List "Tasks" -Identity "All Tasks" -Values @{JSLink="hierarchytaskslist.js|customrendering.js";Title="My view"}
```
Updates the "All Tasks" view on list "Tasks" to use hierarchytaskslist.js and customrendering.js for the JSLink and changes the title of the view to "My view"

### Example 2
```powershell
PS:> Get-PnPList -Identity "Tasks" | Get-PnPView | Set-PnPView -Values @{JSLink="hierarchytaskslist.js|customrendering.js"}
```
Updates all views on list "Tasks" to use hierarchytaskslist.js and customrendering.js for the JSLink
