#Get-PnPRecycleBinItem
Returns the items in the recycle bin from the context
##Syntax
```powershell
Get-PnPRecycleBinItem [-Includes <String[]>]
```


```powershell
Get-PnPRecycleBinItem [-FirstStage [<SwitchParameter>]]
```


```powershell
Get-PnPRecycleBinItem [-Identity <GuidPipeBind>]
```


```powershell
Get-PnPRecycleBinItem [-SecondStage [<SwitchParameter>]]
```


##Returns
>[Microsoft.SharePoint.Client.RecycleBinItem](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.recyclebinitem.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|FirstStage|SwitchParameter|False|Return all items in the first stage recycle bin|
|Identity|GuidPipeBind|False|Returns a recycle bin item with a specific identity|
|Includes|String[]|False|Specify properties to include when retrieving objects from the server.|
|SecondStage|SwitchParameter|False|Return all items in the second stage recycle bin|
##Examples

###Example 1
```powershell
PS:> Get-PnPRecycleBinItem
```
Returns all items in both the first and the second stage recycle bins in the current site collection

###Example 2
```powershell
PS:> Get-PnPRecycleBinItem -Identity f3ef6195-9400-4121-9d1c-c997fb5b86c2
```
Returns all a specific recycle bin item by id

###Example 3
```powershell
PS:> Get-PnPRecycleBinItem -FirstStage
```
Returns all items in only the first stage recycle bin in the current site collection

###Example 4
```powershell
PS:> Get-PnPRecycleBinItem -SecondStage
```
Returns all items in only the second stage recycle bin in the current site collection
