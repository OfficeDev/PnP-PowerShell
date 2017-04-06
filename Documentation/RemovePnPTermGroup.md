# Remove-PnPTermGroup
Removes a taxonomy term group and all its containing termsets
## Syntax
```powershell
Remove-PnPTermGroup -GroupName <String>
                    [-TermStoreName <String>]
                    [-Force [<SwitchParameter>]]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|GroupName|String|True|Name of the taxonomy term group to delete.|
|Force|SwitchParameter|False||
|TermStoreName|String|False|Term store to use; if not specified the default term store is used.|
