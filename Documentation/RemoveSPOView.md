#Remove-SPOView
Deletes a view from a list
##Syntax
```powershell
Remove-SPOView [-Force [<SwitchParameter>]] [-Web <WebPipeBind>] -Identity <ViewPipeBind> -List <ListPipeBind>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False||
|Identity|ViewPipeBind|True|The ID or Title of the view.|
|List|ListPipeBind|True|The ID or Url of the list.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
