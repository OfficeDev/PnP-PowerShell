# Get-PnPTermGroup
Returns a taxonomy term group
## Syntax
```powershell
Get-PnPTermGroup [-TermStore <Id, Name or Object>]
                 [-Includes <String[]>]
                 [-Identity <Id, Title or TaxonomyItem>]
```


## Returns
>[Microsoft.SharePoint.Client.Taxonomy.TermGroup](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termgroup.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|Id, Title or TaxonomyItem|False|Name of the taxonomy term group to retrieve.|
|Includes|String[]|False|Specify properties to include when retrieving objects from the server.|
|TermStore|Id, Name or Object|False|Term store to check; if not specified the default term store is used.|
## Examples

### Example 1
```powershell
PS:> Get-PnPTermGroup
```
Returns all Term Groups in the site collection termstore

### Example 2
```powershell
PS:> Get-PnPTermGroup -Identity "Departments"
```
Returns the termgroup named "Departments" from the site collection termstore

### Example 3
```powershell
PS:> Get-PnPTermGroup -Identity ab2af486-e097-4b4a-9444-527b251f1f8d
```
Returns the termgroup with the given ID from the site collection termstore
