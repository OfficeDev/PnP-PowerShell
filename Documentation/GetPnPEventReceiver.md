#Get-PnPEventReceiver
Returns all or a specific event receiver
##Syntax
```powershell
Get-PnPEventReceiver [-List <ListPipeBind>]
                     [-Identity <GuidPipeBind>]
                     [-Web <WebPipeBind>]
```


##Returns
>[Microsoft.SharePoint.Client.EventReceiverDefinition](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.eventreceiverdefinition.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|GuidPipeBind|False|The Guid of the event receiver on the list|
|List|ListPipeBind|False|The list object from which to get the event receiver object|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-PnPEventReceiver
```
This will return all registered event receivers on the current web

###Example 2
```powershell
PS:> Get-PnPEventReceiver -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22
```
This will return a specific registered event receiver from the current web

###Example 3
```powershell
PS:> Get-PnPEventReceiver -List "ProjectList"
```
This will return all registered event receivers in the list with the name ProjectList

###Example 4
```powershell
PS:> Get-PnPEventReceiver -List "ProjectList" -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22
```
This will return a specific registered event receiver in the list with the name ProjectList
