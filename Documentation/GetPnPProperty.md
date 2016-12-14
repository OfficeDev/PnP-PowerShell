#Get-PnPProperty
Will populate properties of an object and optionally, if needed, load the value from the server. If one property is specified its value will be returned to the output.
##Syntax
```powershell
Get-PnPProperty -ClientObject <ClientObject>
                -Property <String[]>
```


##Returns
>[Microsoft.SharePoint.Client.ClientObject](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.clientobject.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ClientObject|ClientObject|True|Specifies the object where the properties of should be retrieved|
|Property|String[]|True|The properties to load. If one property is specified its value will be returned to the output.|
##Examples

###Example 1
```powershell

PS:> $web = Get-PnPWeb
PS:> Get-PnPProperty -ClientObject $web -Property Id, Lists
PS:> $web.Lists
```
Will load both the Id and Lists properties of the specified Web object.

###Example 2
```powershell

PS:> $list = Get-PnPList -Identity 'Site Assets'
PS:> Get-PnPProperty -ClientObject $list -Property Views
```
Will load the views object of the specified list object and return its value to the output.
