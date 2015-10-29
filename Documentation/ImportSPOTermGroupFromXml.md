#Import-SPOTermGroupFromXml
*Topic automatically generated on: 2015-10-13*

Imports a taxonomy TermGroup from either the input or from an XML file.
##Syntax
```powershell
Import-SPOTermGroupFromXml [-Path <String>]
```


```powershell
Import-SPOTermGroupFromXml [-Xml <String>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Path|String|False|The XML File to import the data from|
|Xml|String|False|The XML to process|
##Examples

###Example 1
```powershell
PS:> Import-SPOTermGroupFromXml -Xml $xml
```
Imports the XML based termgroup information located in the $xml variable

###Example 2
```powershell
PS:> Import-SPOTermGroupFromXml -Path input.xml
```
Imports the XML file specified by the path.
