#Add-SPOTaxonomyField
*Topic automatically generated on: 2015-10-13*

Adds a taxonomy field to a list or as a site column.
##Syntax
```powershell
Add-SPOTaxonomyField [-TaxonomyItemId <GuidPipeBind>] [-List <ListPipeBind>] -DisplayName <String> -InternalName <String> [-Group <String>] [-Id <GuidPipeBind>] [-AddToDefaultView [<SwitchParameter>]] [-MultiValue [<SwitchParameter>]] [-Required [<SwitchParameter>]] [-FieldOptions <AddFieldOptions>] [-Web <WebPipeBind>]
```


```powershell
Add-SPOTaxonomyField -TermSetPath <String> [-TermPathDelimiter <String>] [-List <ListPipeBind>] -DisplayName <String> -InternalName <String> [-Group <String>] [-Id <GuidPipeBind>] [-AddToDefaultView [<SwitchParameter>]] [-MultiValue [<SwitchParameter>]] [-Required [<SwitchParameter>]] [-FieldOptions <AddFieldOptions>] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AddToDefaultView|SwitchParameter|False||
|DisplayName|String|True||
|FieldOptions|AddFieldOptions|False||
|Group|String|False||
|Id|GuidPipeBind|False||
|InternalName|String|True||
|List|ListPipeBind|False||
|MultiValue|SwitchParameter|False||
|Required|SwitchParameter|False||
|TaxonomyItemId|GuidPipeBind|False||
|TermPathDelimiter|String|False||
|TermSetPath|String|True||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Add-SPOTaxonomyField -DisplayName "Test" -InternalName "Test" -TermSetPath "TestTermGroup|TestTermSet"
```
Adds a new taxonomy field called "Test" that points to the TestTermSet which is located in the TestTermGroup
