#Get-SPOView
Returns one or all views from a list
##Syntax
```powershell
Get-SPOView [-Identity <ViewPipeBind>] [-Web <WebPipeBind>] -List <ListPipeBind>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|ViewPipeBind|False||
|List|ListPipeBind|True|The ID or Url of the list.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
Get-SPOView -List "Demo List"
```
Returns all views associated from the specified list

###Example 2
```powershell
Get-SPOView -List "Demo List" -Identity "Demo View"
```
Returns the view called "Demo View" from the specified list

###Example 3
```powershell
Get-SPOView -List "Demo List" -Identity "5275148a-6c6c-43d8-999a-d2186989a661"
```
Returns the view with the specified ID from the specified list
