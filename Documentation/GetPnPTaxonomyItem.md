#Get-PnPTaxonomyItem
Returns a taxonomy item
##Syntax
```powershell
Get-PnPTaxonomyItem -TermPath <String>
```


##Returns
>[Microsoft.SharePoint.Client.Taxonomy.TaxonomyItem](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.taxonomyitem.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|TermPath|String|True|The path, delimited by | of the taxonomy item to retrieve, alike GROUPLABEL|TERMSETLABEL|TERMLABEL|
