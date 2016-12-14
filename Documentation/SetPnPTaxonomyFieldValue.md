#Set-PnPTaxonomyFieldValue
Sets a taxonomy term value in a listitem field
##Syntax
```powershell
Set-PnPTaxonomyFieldValue -TermId <GuidPipeBind>
                          [-Label <String>]
                          -ListItem <ListItem>
                          -InternalFieldName <String>
```


```powershell
Set-PnPTaxonomyFieldValue [-Terms <Hashtable>]
                          -ListItem <ListItem>
                          -InternalFieldName <String>
```


```powershell
Set-PnPTaxonomyFieldValue -TermPath <String>
                          -ListItem <ListItem>
                          -InternalFieldName <String>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|InternalFieldName|String|True|The internal name of the field|
|Label|String|False|The Label value of the term|
|ListItem|ListItem|True|The list item to set the field value to|
|TermId|GuidPipeBind|True|The Id of the Term|
|TermPath|String|True|A path in the form of GROUPLABEL|TERMSETLABEL|TERMLABEL|
|Terms|Hashtable|False|Allows you to specify terms with key value pairs that can be referred to in the template by means of the {id:label} token. See examples on how to use this parameter.|
##Examples

###Example 1
```powershell
PS:> Set-PnPTaxonomyFieldValue -ListItem $item -InternalFieldName 'Department' -TermId 863b832b-6818-4e6a-966d-2d3ee057931c
```
Sets the field called 'Department' to the value of the term with the ID specified

###Example 2
```powershell
PS:> Set-PnPTaxonomyFieldValue -ListItem $item -InternalFieldName 'Department' -TermPath 'CORPORATE|DEPARTMENTS|HR'
```
Sets the field called 'Department' to the term called HR which is located in the DEPARTMENTS termset, which in turn is located in the CORPORATE termgroup.

###Example 3
```powershell
PS:> Set-PnPTaxonomyFieldValue -ListItem $item -InternalFieldName 'Department' -Terms @{"TermId1"="Label1";"TermId2"="Label2"}
```
Sets the field called 'Department' with multiple terms by ID and label. You can refer to those terms with the {ID:label} token.
