#Export-SPOTaxonomy
*Topic automatically generated on: 2015-10-14*

Exports a taxonomy to either the output or to a file.
##Syntax
```powershell
Export-SPOTaxonomy [-TermSetId <GuidPipeBind>] [-TermStoreName <String>] [-IncludeID [<SwitchParameter>]] [-Path <String>] [-Force [<SwitchParameter>]] [-Delimiter <String>] [-Encoding <Encoding>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Delimiter|String|False||
|Encoding|Encoding|False||
|Force|SwitchParameter|False|Overwrites the output file if it exists.|
|IncludeID|SwitchParameter|False|If specified will include the ids of the taxonomy items in the output. Format: <label>;#<guid>|
|Path|String|False|File to export the data to.|
|TermSetId|GuidPipeBind|False|If specified, will export the specified termset only|
|TermStoreName|String|False||
##Examples

###Example 1
```powershell
PS:> Export-SPOTaxonomy
```
Exports the full taxonomy to the standard output

###Example 2
```powershell
PS:> Export-SPOTaxonomy -Path c:\output.txt
```
Exports the full taxonomy the file output.txt

###Example 3
```powershell
PS:> Export-SPOTaxonomy -Path c:\output.txt -TermSet f6f43025-7242-4f7a-b739-41fa32847254 
```
Exports the term set with the specified id
Exports a taxonomy to either the output or to a file.
##Syntax
```powershell
Export-SPOTaxonomy [-TermSetId <GuidPipeBind>] [-TermStoreName <String>] [-IncludeID [<SwitchParameter>]] [-Path <String>] [-Force [<SwitchParameter>]] [-Delimiter <String>] [-Encoding <Encoding>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Delimiter|String|False||
|Encoding|Encoding|False||
|Force|SwitchParameter|False|Overwrites the output file if it exists.|
|IncludeID|SwitchParameter|False|If specified will include the ids of the taxonomy items in the output. Format: <label>;#<guid>|
|Path|String|False|File to export the data to.|
|TermSetId|GuidPipeBind|False|If specified, will export the specified termset only|
|TermStoreName|String|False||
##Examples

###Example 1
```powershell
PS:> Export-SPOTaxonomy
```
Exports the full taxonomy to the standard output

###Example 2
```powershell
PS:> Export-SPOTaxonomy -Path c:\output.txt
```
Exports the full taxonomy the file output.txt

###Example 3
```powershell
PS:> Export-SPOTaxonomy -Path c:\output.txt -TermSet f6f43025-7242-4f7a-b739-41fa32847254 
```
Exports the term set with the specified id
