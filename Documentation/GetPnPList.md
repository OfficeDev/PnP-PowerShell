#Get-PnPList
Returns a List object
##Syntax
```powershell
Get-PnPList [-Web <WebPipeBind>]
            [-Includes <String[]>]
            [-Identity <ListPipeBind>]
```


##Returns
>[Microsoft.SharePoint.Client.List](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.list.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ListPipeBind|False|The ID, name or Url (Lists/MyList) of the list.|
|Includes|String[]|False|Specify properties to include when retrieving objects from the server.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-PnPList
```
Returns all lists in the current web

###Example 2
```powershell
PS:> Get-PnPList -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe
```
Returns a list with the given id.

###Example 3
```powershell
PS:> Get-PnPList -Identity Lists/Announcements
```
Returns a list with the given url.
