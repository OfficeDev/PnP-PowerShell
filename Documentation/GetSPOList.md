#Get-SPOList
*Topic automatically generated on: 2015-10-13*

Returns a List object
##Syntax
```powershell
Get-SPOList [-Web <WebPipeBind>] [-Identity <ListPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ListPipeBind|False|The ID or Url of the list.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-SPOList
```
Returns all lists in the current web

###Example 2
```powershell
PS:> Get-SPOList -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe
```
Returns a list with the given id.

###Example 3
```powershell
PS:> Get-SPOList -Identity /Lists/Announcements
```
Returns a list with the given url.
