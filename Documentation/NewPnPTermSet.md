# New-PnPTermSet
Creates a taxonomy term set
## Syntax
```powershell
New-PnPTermSet -Name <String>
               -TermGroup <Id, Title or TermGroup>
               [-Id <Guid>]
               [-Lcid <Int>]
               [-Contact <String>]
               [-Description <String>]
               [-IsOpenForTermCreation [<SwitchParameter>]]
               [-IsNotAvailableForTagging [<SwitchParameter>]]
               [-Owner <String>]
               [-StakeHolders <String[]>]
               [-CustomProperties <Hashtable>]
               [-TermStore <Id, Name or Object>]
```


## Returns
>[Microsoft.SharePoint.Client.Taxonomy.TermSet](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.taxonomy.termset.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Name|String|True|The name of the termset.|
|TermGroup|Id, Title or TermGroup|True|Name, id or actualy termgroup to create the termset in.|
|Contact|String|False|An e-mail address for term suggestion and feedback. If left blank the suggestion feature will be disabled.|
|CustomProperties|Hashtable|False||
|Description|String|False|Descriptive text to help users understand the intended use of this term set.|
|Id|Guid|False|The Id to use for the term set; if not specified, or the empty GUID, a random GUID is generated and used.|
|IsNotAvailableForTagging|SwitchParameter|False|By default a term set is available to be used by end users and content editors of sites consuming this term set. Specify this switch to turn this off|
|IsOpenForTermCreation|SwitchParameter|False|When a term set is closed, only metadata managers can add terms to this term set. When it is open, users can add terms from a tagging application. Not specifying this switch will make the term set closed.|
|Lcid|Int|False|The locale id to use for the term set. Defaults to the current locale id.|
|Owner|String|False|The primary user or group of this term set.|
|StakeHolders|String[]|False|People and groups in the organization that should be notified before major changes are made to the term set. You can enter multiple users or groups.|
|TermStore|Id, Name or Object|False|Term store to check; if not specified the default term store is used.|
## Examples

### Example 1
```powershell
PS:> New-PnPTermSet -Name "Department" -TermGroup "Corporate"
```
Creates a new termset named "Department" in the group named "Corporate"
