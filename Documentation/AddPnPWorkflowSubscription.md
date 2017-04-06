# Add-PnPWorkflowSubscription
Adds a workflow subscription to a list
## Syntax
```powershell
Add-PnPWorkflowSubscription -Name <String>
                            -DefinitionName <String>
                            -List <ListPipeBind>
                            -HistoryListName <String>
                            -TaskListName <String>
                            [-StartManually [<SwitchParameter>]]
                            [-StartOnCreated [<SwitchParameter>]]
                            [-StartOnChanged [<SwitchParameter>]]
                            [-AssociationValues <Dictionary`2>]
                            [-Web <WebPipeBind>]
```

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|DefinitionName|String|True|The name of the workflow definition|
|HistoryListName|String|True||
|List|ListPipeBind|True|The list to add the workflow to|
|Name|String|True|The name of the subscription|
|TaskListName|String|True||
|AssociationValues|Dictionary`2|False||
|StartManually|SwitchParameter|False||
|StartOnChanged|SwitchParameter|False||
|StartOnCreated|SwitchParameter|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
