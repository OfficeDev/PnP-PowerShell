# Get-PnPTerm
Returns a taxonomy term
## Syntax
```powershell
Get-PnPTerm -TermSet <Id, Title or TaxonomyItem>
            -TermGroup <Id, Title or TermGroup>
            [-Identity <Id, Name or Object>]
            [-TermStore <Id, Name or Object>]
            [-Includes <String[]>]
```


## Returns
>[Microsoft.SharePoint.Client.Taxonomy.Term](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.term.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|TermGroup|Id, Title or TermGroup|True|Name of the termgroup to check.|
|TermSet|Id, Title or TaxonomyItem|True|Name of the termset to check.|
|Identity|Id, Name or Object|False|The Id or Name of a Term|
|Includes|String[]|False|Specify properties to include when retrieving objects from the server.|
|TermStore|Id, Name or Object|False|Term store to check; if not specified the default term store is used.|
## Examples

### Example 1
```powershell
PS:> Get-PnPTerm -TermSet "Departments" -TermGroup "Corporate"
```
Returns all term in the termset "Departments" which is in the group "Corporate" from the site collection termstore

### Example 2
```powershell
PS:> Get-PnPTermSet -Identity "Finance" -TermSet "Departments" -TermGroup "Corporate"
```
Returns the term named "Finance" in the termset "Departments" from the termgroup called "Corporate" from the site collection termstore

### Example 3
```powershell
PS:> Get-PnPTermSet -Identity ab2af486-e097-4b4a-9444-527b251f1f8d -TermSet "Departments" -TermGroup "Corporate"
```
Returns the termset named with the given id, from the "Departments" from termgroup called "Corporate" from the site collection termstore
