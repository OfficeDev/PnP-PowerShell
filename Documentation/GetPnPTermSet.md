# Get-PnPTermSet
Returns a taxonomy term set
## Syntax
```powershell
Get-PnPTermSet -TermGroup <Id, Title or TermGroup>
               [-Identity <Id, Name or Object>]
               [-TermStore <Id, Name or Object>]
               [-Includes <String[]>]
```


## Returns
>[Microsoft.SharePoint.Client.Taxonomy.TermSet](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termset.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|TermGroup|Id, Title or TermGroup|True|Name of the term group to check.|
|Identity|Id, Name or Object|False|The Id or Name of a termset|
|Includes|String[]|False|Specify properties to include when retrieving objects from the server.|
|TermStore|Id, Name or Object|False|Term store to check; if not specified the default term store is used.|
## Examples

### Example 1
```powershell
PS:> Get-PnPTermSet -TermGroup "Corporate"
```
Returns all termsets in the group "Corporate" from the site collection termstore

### Example 2
```powershell
PS:> Get-PnPTermSet -Identity "Departments" -TermGroup "Corporate"
```
Returns the termset named "Departments" from the termgroup called "Corporate" from the site collection termstore

### Example 3
```powershell
PS:> Get-PnPTermSet -Identity ab2af486-e097-4b4a-9444-527b251f1f8d -TermGroup "Corporate
```
Returns the termset with the given id from the termgroup called "Corporate" from the site collection termstore
