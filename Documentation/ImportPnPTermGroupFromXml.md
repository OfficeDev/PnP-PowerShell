# Import-PnPTermGroupFromXml
Imports a taxonomy TermGroup from either the input or from an XML file.
## Syntax
```powershell
Import-PnPTermGroupFromXml [-Xml <String>]
```


```powershell
Import-PnPTermGroupFromXml [-Path <String>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Path|String|False|The XML File to import the data from|
|Xml|String|False|The XML to process|
## Examples

### Example 1
```powershell
PS:> Import-PnPTermGroupFromXml -Xml $xml
```
Imports the XML based termgroup information located in the $xml variable

### Example 2
```powershell
PS:> Import-PnPTermGroupFromXml -Path input.xml
```
Imports the XML file specified by the path.
