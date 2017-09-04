# Set-PnPWebhookSubscription
Removes a Webhook subscription from the resource
>*Only available for SharePoint Online*
## Syntax
```powershell
Set-PnPWebhookSubscription -Subscription <WebhookSubscriptionPipeBind>
                           [-List <ListPipeBind>]
                           [-NotificationUrl <String>]
                           [-ExpirationDate <DateTime>]
                           [-Web <WebPipeBind>]
```


## Returns
>OfficeDevPnP.Core.Entities.WebhookSubscription

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Subscription|WebhookSubscriptionPipeBind|True|The identity of the Webhook subscription to update|
|ExpirationDate|DateTime|False|The date at which the Webhook subscription will expire. (Default: 6 months from today)|
|List|ListPipeBind|False|The list object or name from which the Webhook subscription will be modified|
|NotificationUrl|String|False|The URL of the Webhook endpoint that will be notified of the change|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Set-PnPWebhookSubscription -List MyList -Subscription ea1533a8-ff03-415b-a7b6-517ee50db8b6 -NotificationUrl https://my-func.azurewebsites.net/webhook
```
Updates an existing Webhook subscription with the specified id on the list MyList with a new Notification Url

### Example 2
```powershell
PS:> Set-PnPWebhookSubscription -List MyList -Subscription ea1533a8-ff03-415b-a7b6-517ee50db8b6 -NotificationUrl https://my-func.azurewebsites.net/webhook -ExpirationDate "2017-09-01"
```
Updates an existing Webhook subscription with the specified id on the list MyList with a new Notification Url and a new expiration date

### Example 3
```powershell
PS:> $subscriptions = Get-PnPWebhookSubscriptions -List MyList
PS:> $updated = $subscriptions[0]
PS:> $updated.ExpirationDate = "2017-10-01"
PS:> Set-PnPWebhookSubscription -List MyList -Subscription $updated
```
Updates the Webhook subscription from the list MyList with a modified subscription object.
Note: The date will be converted to Universal Time
