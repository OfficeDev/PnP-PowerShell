#Export-PnPTaxonomy
Exports a taxonomy to either the output or to a file.
## Syntax
```powershell
Export-PnPTaxonomy [-TermSetId <GuidPipeBind>]
                   [-TermStoreName <String>]
                   [-IncludeID [<SwitchParameter>]]
                   [-Path <String>]
                   [-Force [<SwitchParameter>]]
                   [-Delimiter <String>]
                   [-Encoding <Encoding>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Delimiter|String|False|The path delimiter to be used, by default this is '|'|
|Encoding|Encoding|False|Defaults to Unicode|
|Force|SwitchParameter|False|Overwrites the output file if it exists.|
|IncludeID|SwitchParameter|False|If specified will include the ids of the taxonomy items in the output. Format: <label>;#<guid>|
|Path|String|False|File to export the data to.|
|TermSetId|GuidPipeBind|False|If specified, will export the specified termset only|
|TermStoreName|String|False|Term store to export; if not specified the default term store is used.|
## Examples

### Example 1
```powershell
PS:> Export-PnPTaxonomy
```
Exports the full taxonomy to the standard output

### Example 2
```powershell
PS:> Export-PnPTaxonomy -Path c:\output.txt
```
Exports the full taxonomy the file output.txt

### Example 3
```powershell
PS:> Export-PnPTaxonomy -Path c:\output.txt -TermSet f6f43025-7242-4f7a-b739-41fa32847254 
```
Exports the term set with the specified id
