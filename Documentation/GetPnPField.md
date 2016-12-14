#Get-PnPField
Returns a field from a list or site
##Syntax
```powershell
Get-PnPField [-List <ListPipeBind>]
             [-Web <WebPipeBind>]
             [-Identity <FieldPipeBind>]
```


##Returns
>[Microsoft.SharePoint.Client.Field](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|FieldPipeBind|False|The field object or name to get|
|List|ListPipeBind|False|The list object or name where to get the field from|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-PnPField
```
Gets all the fields from the current site

###Example 2
```powershell
PS:> Get-PnPField -List "Demo list" -Identity "Speakers"
```
Gets the speakers field from the list Demo list
