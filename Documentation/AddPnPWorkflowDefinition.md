# Add-PnPWorkflowDefinition
Adds a workflow definition
## Syntax
```powershell
Add-PnPWorkflowDefinition -Definition <WorkflowDefinition>
                          [-DoNotPublish [<SwitchParameter>]]
                          [-Web <WebPipeBind>]
```


## Returns
>System.Guid

Returns the Id of the workflow definition

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Definition|WorkflowDefinition|True|The workflow definition to add.|
|DoNotPublish|SwitchParameter|False|Overrides the default behaviour, which is to publish workflow definitions.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
Add-PnPWorkflowDefinition -Definition $wfdef
```
Adds an existing workflow definition, retrieved by Get-PnPWorkflowDefinition, to a site.
