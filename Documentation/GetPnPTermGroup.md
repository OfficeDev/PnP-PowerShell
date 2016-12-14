#Get-PnPTermGroup
Returns a taxonomy term group
##Syntax
```powershell
Get-PnPTermGroup [-TermStoreName <String>]
                 -GroupName <String>
```


##Returns
>[Microsoft.SharePoint.Client.Taxonomy.TermGroup](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termgroup.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|GroupName|String|True|Name of the taxonomy term group to retrieve.|
|TermStoreName|String|False|Term store to check; if not specified the default term store is used.|
