#Clear-PnpRecycleBinItem
Permanently deletes all or a specific recycle bin item
##Syntax
```powershell
Clear-PnpRecycleBinItem [-All [<SwitchParameter>]]
                        [-SecondStageOnly [<SwitchParameter>]]
                        [-Force [<SwitchParameter>]]
```


```powershell
Clear-PnpRecycleBinItem -Identity <RecycleBinItemPipeBind>
                        [-Force [<SwitchParameter>]]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|All|SwitchParameter|False|Clears all items|
|Force|SwitchParameter|False|If provided, no confirmation will be asked to permanently delete the recycle bin item|
|Identity|RecycleBinItemPipeBind|True|Id of the recycle bin item or the recycle bin item itself to permanently delete|
|SecondStageOnly|SwitchParameter|False|If provided, only all the items in the second stage recycle bin will be cleared|
##Examples

###Example 1
```powershell
PS:> Get-PnPRecycleBinItems | ? FileLeafName -like "*.docx" | Clear-PnpRecycleBinItem
```
Permanently deletes all the items in the first and second stage recycle bins of which the file names have the .docx extension

###Example 2
```powershell
PS:> Clear-PnpRecycleBinItem -Identity 72e4d749-d750-4989-b727-523d6726e442
```
Permanently deletes the recycle bin item with Id 72e4d749-d750-4989-b727-523d6726e442 from the recycle bin

###Example 3
```powershell
PS:> Clear-PnpRecycleBinItem -Identity $item -Force
```
Permanently deletes the recycle bin item stored under variable $item from the recycle bin without asking for confirmation from the end user first
