#Add-PnPTaxonomyField
Adds a taxonomy field to a list or as a site column.
##Syntax
```powershell
Add-PnPTaxonomyField [-TaxonomyItemId <GuidPipeBind>]
                     [-List <ListPipeBind>]
                     -DisplayName <String>
                     -InternalName <String>
                     [-Group <String>]
                     [-Id <GuidPipeBind>]
                     [-AddToDefaultView [<SwitchParameter>]]
                     [-MultiValue [<SwitchParameter>]]
                     [-Required [<SwitchParameter>]]
                     [-FieldOptions <AddFieldOptions>]
                     [-Web <WebPipeBind>]
```


```powershell
Add-PnPTaxonomyField -TermSetPath <String>
                     [-TermPathDelimiter <String>]
                     [-List <ListPipeBind>]
                     -DisplayName <String>
                     -InternalName <String>
                     [-Group <String>]
                     [-Id <GuidPipeBind>]
                     [-AddToDefaultView [<SwitchParameter>]]
                     [-MultiValue [<SwitchParameter>]]
                     [-Required [<SwitchParameter>]]
                     [-FieldOptions <AddFieldOptions>]
                     [-Web <WebPipeBind>]
```


##Returns
>[Microsoft.SharePoint.Client.Field](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AddToDefaultView|SwitchParameter|False|Switch Parameter if this field must be added to the default view|
|DisplayName|String|True|The display name of the field|
|FieldOptions|AddFieldOptions|False|Specifies the control settings while adding a field. See https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.addfieldoptions.aspx for details|
|Group|String|False|The group name to where this field belongs to|
|Id|GuidPipeBind|False|The ID for the field, must be unique|
|InternalName|String|True|The internal name of the field|
|List|ListPipeBind|False|The list object or name where this field needs to be added|
|MultiValue|SwitchParameter|False|Switch Parameter if this Taxonomy field can hold multiple values|
|Required|SwitchParameter|False|Switch Parameter if the field is a required field|
|TaxonomyItemId|GuidPipeBind|False|The ID of the Taxonomy item|
|TermPathDelimiter|String|False|The path delimiter to be used, by default this is '|'|
|TermSetPath|String|True|The path to the term that this needs be be bound|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Add-PnPTaxonomyField -DisplayName "Test" -InternalName "Test" -TermSetPath "TestTermGroup|TestTermSet"
```
Adds a new taxonomy field called "Test" that points to the TestTermSet which is located in the TestTermGroup
