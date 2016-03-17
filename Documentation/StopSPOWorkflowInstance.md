#Stop-SPOWorkflowInstance
Stops a workflow instance
##Syntax
```powershell
Stop-SPOWorkflowInstance [-Web <WebPipeBind>] -Identity <WorkflowInstancePipeBind>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|WorkflowInstancePipeBind|True|The instance to stop|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
