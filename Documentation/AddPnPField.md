#Add-PnPField
Adds a field to a list or as a site column
##Syntax
```powershell
Add-PnPField -List <ListPipeBind>
             -Field <FieldPipeBind>
             [-List <ListPipeBind>]
             [-DisplayName <String>]
             [-InternalName <String>]
             [-Type <FieldType>]
             [-Id <GuidPipeBind>]
             [-AddToDefaultView [<SwitchParameter>]]
             [-Required [<SwitchParameter>]]
             [-Group <String>]
             [-Web <WebPipeBind>]
```


```powershell
Add-PnPField [-List <ListPipeBind>]
             -DisplayName <String>
             -InternalName <String>
             -Type <FieldType>
             [-Id <GuidPipeBind>]
             [-AddToDefaultView [<SwitchParameter>]]
             [-Required [<SwitchParameter>]]
             [-Group <String>]
             [-List <ListPipeBind>]
             [-DisplayName <String>]
             [-InternalName <String>]
             [-Type <FieldType>]
             [-Id <GuidPipeBind>]
             [-AddToDefaultView [<SwitchParameter>]]
             [-Required [<SwitchParameter>]]
             [-Group <String>]
             [-Web <WebPipeBind>]
             [-Choices <String[]>]
```


```powershell
Add-PnPField [-AddToDefaultView [<SwitchParameter>]]
             [-Required [<SwitchParameter>]]
             [-Group <String>]
             [-List <ListPipeBind>]
             [-DisplayName <String>]
             [-InternalName <String>]
             [-Type <FieldType>]
             [-Id <GuidPipeBind>]
             [-AddToDefaultView [<SwitchParameter>]]
             [-Required [<SwitchParameter>]]
             [-Group <String>]
             [-Web <WebPipeBind>]
```


```powershell
Add-PnPField -DisplayName <String>
             -InternalName <String>
             -Type <FieldType>
             [-Id <GuidPipeBind>]
             [-List <ListPipeBind>]
             [-DisplayName <String>]
             [-InternalName <String>]
             [-Type <FieldType>]
             [-Id <GuidPipeBind>]
             [-AddToDefaultView [<SwitchParameter>]]
             [-Required [<SwitchParameter>]]
             [-Group <String>]
             [-Web <WebPipeBind>]
             [-Choices <String[]>]
```


##Returns
>[Microsoft.SharePoint.Client.Field](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AddToDefaultView|SwitchParameter|False||
|Choices|String[]|False|Specify choices, only valid if the field type is Choice|
|DisplayName|String|True||
|Field|FieldPipeBind|True|The name of the field, its ID or an actual field object that needs to be added|
|Group|String|False||
|Id|GuidPipeBind|False||
|InternalName|String|True||
|List|ListPipeBind|False||
|Required|SwitchParameter|False||
|Type|FieldType|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Add-PnPField -List "Demo list" -DisplayName "Location" -InternalName "SPSLocation" -Type Choice -Group "Demo Group" -AddToDefaultView -Choices "Stockholm","Helsinki","Oslo"
```
This will add a field of type Choice to the list "Demo List".

###Example 2
```powershell
PS:>Add-PnPField -List "Demo list" -DisplayName "Speakers" -InternalName "SPSSpeakers" -Type MultiChoice -Group "Demo Group" -AddToDefaultView -Choices "Obiwan Kenobi","Darth Vader", "Anakin Skywalker"
```
This will add a field of type Multiple Choice to the list "Demo List". (you can pick several choices for the same item)
