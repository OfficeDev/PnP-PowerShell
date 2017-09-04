# Move-PnpRecycleBinItem
Moves all items or a specific item in the first stage recycle bin of the current site collection to the second stage recycle bin
>*Only available for SharePoint Online*
## Syntax
```powershell
Move-PnpRecycleBinItem [-Identity <RecycleBinItemPipeBind>]
                       [-Force [<SwitchParameter>]]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|If provided, no confirmation will be asked to move the first stage recycle bin items to the second stage|
|Identity|RecycleBinItemPipeBind|False|If provided, moves the item with the specific ID to the second stage recycle bin|
## Examples

### Example 1
```powershell
PS:> Move-PnpRecycleBinItem
```
Moves all the items in the first stage recycle bin of the current site collection to the second stage recycle bin

### Example 2
```powershell
PS:> Move-PnpRecycleBinItem -Identity 26ffff29-b526-4451-9b6f-7f0e56ba7125
```
Moves the item with the provided ID in the first stage recycle bin of the current site collection to the second stage recycle bin without asking for confirmation first

### Example 3
```powershell
PS:> Move-PnpRecycleBinItem -Force
```
Moves all the items in the first stage recycle bin of the current context to the second stage recycle bin without asking for confirmation first
