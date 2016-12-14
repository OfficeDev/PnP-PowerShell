#Get-PnPView
Returns one or all views from a list
##Syntax
```powershell
Get-PnPView [-Identity <ViewPipeBind>]
            [-Web <WebPipeBind>]
            -List <ListPipeBind>
```


##Returns
>[Microsoft.SharePoint.Client.View](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.view.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ViewPipeBind|False|The ID or name of the view|
|List|ListPipeBind|True|The ID or Url of the list.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
Get-PnPView -List "Demo List"
```
Returns all views associated from the specified list

###Example 2
```powershell
Get-PnPView -List "Demo List" -Identity "Demo View"
```
Returns the view called "Demo View" from the specified list

###Example 3
```powershell
Get-PnPView -List "Demo List" -Identity "5275148a-6c6c-43d8-999a-d2186989a661"
```
Returns the view with the specified ID from the specified list
