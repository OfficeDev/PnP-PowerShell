#New-PnPTermGroup
Creates a taxonomy term group
##Syntax
```powershell
New-PnPTermGroup -GroupName <String>
                 [-GroupId <Guid>]
                 [-Description <String>]
                 [-TermStoreName <String>]
```


##Returns
>[Microsoft.SharePoint.Client.Taxonomy.TermGroup](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termgroup.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Description|String|False|Description to use for the term group.|
|GroupId|Guid|False|GUID to use for the term group; if not specified, or the empty GUID, a random GUID is generated and used.|
|GroupName|String|True|Name of the taxonomy term group to create.|
|TermStoreName|String|False|Term store to check; if not specified the default term store is used.|
##Examples

###Example 1
```powershell
PS:> New-PnPTermGroup -GroupName "Countries"
```
Creates a new taxonomy term group named "Countries"
