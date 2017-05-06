# New-PnPTermGroup
Creates a taxonomy term group
## Syntax
```powershell
New-PnPTermGroup -Name <String>
                 [-Id <Guid>]
                 [-Description <String>]
                 [-TermStore <Id, Name or Object>]
```


## Returns
>[Microsoft.SharePoint.Client.Taxonomy.TermGroup](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termgroup.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Name|String|True|Name of the taxonomy term group to create.|
|Description|String|False|Description to use for the term group.|
|Id|Guid|False|GUID to use for the term group; if not specified, or the empty GUID, a random GUID is generated and used.|
|TermStore|Id, Name or Object|False|Term store to add the group to; if not specified the default term store is used.|
## Examples

### Example 1
```powershell
PS:> New-PnPTermGroup -GroupName "Countries"
```
Creates a new taxonomy term group named "Countries"
