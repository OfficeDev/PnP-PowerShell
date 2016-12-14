#Get-PnPWorkflowSubscription
Returns a workflow subscriptions from a list
##Syntax
```powershell
Get-PnPWorkflowSubscription [-Web <WebPipeBind>]
                            [-Name <String>]
                            [-List <ListPipeBind>]
```


##Returns
>[Microsoft.SharePoint.Client.WorkflowServices.WorkflowSubscription](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.workflowservices.workflowsubscription.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|List|ListPipeBind|False|A list to search the association for|
|Name|String|False|The name of the workflow|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
