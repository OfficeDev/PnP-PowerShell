# Get-PnPWebhookSubscriptions
Gets all the Webhook subscriptions of the resource
## Syntax
```powershell
Get-PnPWebhookSubscriptions [-List <ListPipeBind>]
                            [-Web <WebPipeBind>]
```


## Returns
>OfficeDevPnP.Core.Entities.WebhookSubscription

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|List|ListPipeBind|False|The list object or name to get the Webhook subscriptions from|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Get-PnPWebhookSubscriptions -List MyList
```
Gets all Webhook subscriptions of the list MyList
