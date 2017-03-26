# Add-PnPField
Adds a field to a list or as a site column
## Syntax
```powershell
Add-PnPField [-AddToDefaultView [<SwitchParameter>]]
             [-Required [<SwitchParameter>]]
             [-Group <String>]
             [-Web <WebPipeBind>]
```


```powershell
Add-PnPField -List <ListPipeBind>
             -Field <FieldPipeBind>
             [-Web <WebPipeBind>]
```


```powershell
Add-PnPField -DisplayName <String>
             -InternalName <String>
             -Type <FieldType>
             [-List <ListPipeBind>]
             [-Id <GuidPipeBind>]
             [-AddToDefaultView [<SwitchParameter>]]
             [-Required [<SwitchParameter>]]
             [-Group <String>]
             [-Web <WebPipeBind>]
             [-Choices <String[]>]
```


```powershell
Add-PnPField -DisplayName <String>
             -InternalName <String>
             -Type <FieldType>
             [-Id <GuidPipeBind>]
             [-Web <WebPipeBind>]
             [-Choices <String[]>]
```


## Returns
>[Microsoft.SharePoint.Client.Field](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|DisplayName|String|True|The display name of the field|
|Field|FieldPipeBind|True|The name of the field, its ID or an actual field object that needs to be added|
|InternalName|String|True|The internal name of the field|
|Type|FieldType|True|The type of the field like Choice, Note, MultiChoice|
|AddToDefaultView|SwitchParameter|False|Switch Parameter if this field must be added to the default view|
|Choices|String[]|False|Specify choices, only valid if the field type is Choice|
|Group|String|False|The group name to where this field belongs to|
|Id|GuidPipeBind|False|The ID of the field, must be unique|
|List|ListPipeBind|False|The name of the list, its ID or an actual list object where this field needs to be added|
|Required|SwitchParameter|False|Switch Parameter if the field is a required field|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPField -List "Demo list" -DisplayName "Location" -InternalName "SPSLocation" -Type Choice -Group "Demo Group" -AddToDefaultView -Choices "Stockholm","Helsinki","Oslo"
```
This will add a field of type Choice to the list "Demo List".

### Example 2
```powershell
PS:>Add-PnPField -List "Demo list" -DisplayName "Speakers" -InternalName "SPSSpeakers" -Type MultiChoice -Group "Demo Group" -AddToDefaultView -Choices "Obiwan Kenobi","Darth Vader", "Anakin Skywalker"
```
This will add a field of type Multiple Choice to the list "Demo List". (you can pick several choices for the same item)
