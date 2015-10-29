#Add-SPOView
*Topic automatically generated on: 2015-10-13*

Adds a view to a list
##Syntax
```powershell
Add-SPOView -Title <String> [-Query <String>] -Fields <String[]> [-ViewType <ViewType>] [-RowLimit <UInt32>] [-Personal [<SwitchParameter>]] [-SetAsDefault [<SwitchParameter>]] [-Web <WebPipeBind>] -List <ListPipeBind>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Fields|String[]|True|A list of fields to add.|
|List|ListPipeBind|True|The ID or Url of the list.|
|Personal|SwitchParameter|False|If specified, a personal view will be created.|
|Query|String|False|A valid CAML Query.|
|RowLimit|UInt32|False|The row limit for the view. Defaults to 30.|
|SetAsDefault|SwitchParameter|False|If specified the view will be set as the default view for the list.|
|Title|String|True|The title of the view.|
|ViewType|ViewType|False|The type of view to add.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
Add-SPOView -List "Demo List" -Title "Demo View" -Fields "Title","Address"
```

