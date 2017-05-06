# Restore-PnpRecycleBinItem
Restores the provided recycle bin item to its original location
## Syntax
```powershell
Restore-PnpRecycleBinItem -Identity <RecycleBinItemPipeBind>
                          [-Force [<SwitchParameter>]]
```


```powershell
Restore-PnpRecycleBinItem -All [<SwitchParameter>]
                          [-Force [<SwitchParameter>]]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|All|SwitchParameter|True|If provided all items will be stored |
|Identity|RecycleBinItemPipeBind|True|Id of the recycle bin item or the recycle bin item object itself to restore|
|Force|SwitchParameter|False|If provided, no confirmation will be asked to restore the recycle bin item|
## Examples

### Example 1
```powershell
PS:> Restore-PnpRecycleBinItem -Identity 72e4d749-d750-4989-b727-523d6726e442
```
Restores the recycle bin item with Id 72e4d749-d750-4989-b727-523d6726e442 to its original location

### Example 2
```powershell
PS:> Get-PnPRecycleBinItems | ? FileLeafName -like "*.docx" | Restore-PnpRecycleBinItem
```
Restores all the items in the first and second stage recycle bins to their original location of which the filename ends with the .docx extension
