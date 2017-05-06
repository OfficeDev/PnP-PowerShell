# New-PnPTerm
Creates a taxonomy term
## Syntax
```powershell
New-PnPTerm -Name <String>
            -TermGroup <Id, Title or TermGroup>
            -TermSet <Id, Title or TaxonomyItem>
            [-Id <Guid>]
            [-Lcid <Int>]
            [-Description <String>]
            [-CustomProperties <Hashtable>]
            [-LocalCustomProperties <Hashtable>]
            [-TermStore <Id, Name or Object>]
```


## Returns
>[Microsoft.SharePoint.Client.Taxonomy.Term](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.term.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Name|String|True|The name of the term.|
|TermGroup|Id, Title or TermGroup|True|The termgroup to create the term in.|
|TermSet|Id, Title or TaxonomyItem|True|The termset to add the term to.|
|CustomProperties|Hashtable|False|Custom Properties|
|Description|String|False|Descriptive text to help users understand the intended use of this term.|
|Id|Guid|False|The Id to use for the term; if not specified, or the empty GUID, a random GUID is generated and used.|
|Lcid|Int|False|The locale id to use for the term. Defaults to the current locale id.|
|LocalCustomProperties|Hashtable|False|Custom Properties|
|TermStore|Id, Name or Object|False|Term store to check; if not specified the default term store is used.|
## Examples

### Example 1
```powershell
PS:> New-PnPTerm -TermSet "Departments" -TermGroup "Corporate" -Name "Finance"
```
Creates a new taxonomy term named "Finance" in the termset Departments which is located in the "Corporate" termgroup
