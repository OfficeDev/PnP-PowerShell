# Import-PnPTaxonomy
Imports a taxonomy from either a string array or a file
## Syntax
```powershell
Import-PnPTaxonomy [-Terms <String[]>]
                   [-Lcid <Int>]
                   [-TermStoreName <String>]
                   [-Delimiter <String>]
                   [-SynchronizeDeletions [<SwitchParameter>]]
```


```powershell
Import-PnPTaxonomy -Path <String>
                   [-Lcid <Int>]
                   [-TermStoreName <String>]
                   [-Delimiter <String>]
                   [-SynchronizeDeletions [<SwitchParameter>]]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Path|String|True|Specifies a file containing terms per line, in the format as required by the Terms parameter.|
|Delimiter|String|False|The path delimiter to be used, by default this is '|'|
|Lcid|Int|False||
|SynchronizeDeletions|SwitchParameter|False|If specified, terms that exist in the termset, but are not in the imported data, will be removed.|
|Terms|String[]|False|An array of strings describing termgroup, termset, term, subterms using a default delimiter of '|'.|
|TermStoreName|String|False|Term store to import to; if not specified the default term store is used.|
## Examples

### Example 1
```powershell
PS:> Import-PnPTaxonomy -Terms 'Company|Locations|Stockholm'
```
Creates a new termgroup, 'Company', a termset 'Locations' and a term 'Stockholm'

### Example 2
```powershell
PS:> Import-PnPTaxonomy -Terms 'Company|Locations|Stockholm|Central','Company|Locations|Stockholm|North'
```
Creates a new termgroup, 'Company', a termset 'Locations', a term 'Stockholm' and two subterms: 'Central', and 'North'
