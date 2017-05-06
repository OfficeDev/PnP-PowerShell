# Get-PnPWorkflowDefinition
Returns a workflow definition
## Syntax
```powershell
Get-PnPWorkflowDefinition [-PublishedOnly [<SwitchParameter>]]
                          [-Web <WebPipeBind>]
                          [-Name <String>]
```


## Returns
>[Microsoft.SharePoint.Client.WorkflowServices.WorkflowDefinition](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.workflowservices.workflowdefinition.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Name|String|False|The name of the workflow|
|PublishedOnly|SwitchParameter|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
