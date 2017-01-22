#Get-PnPTermGroup
Returns a taxonomy term group
##Syntax
```powershell
Get-PnPTermGroup [-TermStoreName <String>]
                 [-Includes <String[]>]
                 -GroupName <String>
```


##Returns
>[Microsoft.SharePoint.Client.Taxonomy.TermGroup](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termgroup.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|GroupName|String|True|Name of the taxonomy term group to retrieve.|
|Includes|String[]|False|Specify properties to include when retrieving objects from the server.|
|TermStoreName|String|False|Term store to check; if not specified the default term store is used.|
