# Add-PnPWebhookSubscription
Adds a new Webhook subscription
## Syntax
```powershell
Add-PnPWebhookSubscription -NotificationUrl <String>
                           [-List <ListPipeBind>]
                           [-ExpirationDate <DateTime>]
                           [-ClientState <String>]
                           [-Web <WebPipeBind>]
```


## Returns
>OfficeDevPnP.Core.Entities.WebhookSubscription

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|NotificationUrl|String|True|The URL of the Webhook endpoint that will be notified of the change|
|ClientState|String|False|A client state information that will be passed through notifications|
|ExpirationDate|DateTime|False|The date at which the Webhook subscription will expire. (Default: 6 months from today)|
|List|ListPipeBind|False|The list object or name where the Webhook subscription will be added to|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPWebhookSubscription -List MyList -NotificationUrl https://my-func.azurewebsites.net/webhook
```
Add a Webhook subscription for the specified notification Url on the list MyList

### Example 2
```powershell
PS:> Add-PnPWebhookSubscription -List MyList -NotificationUrl https://my-func.azurewebsites.net/webhook -ExpirationDate "2017-09-01"
```
Add a Webhook subscription for the specified notification Url on the list MyList with an expiration date set on September 1st, 2017

### Example 3
```powershell
PS:> Add-PnPWebhookSubscription -List MyList -NotificationUrl https://my-func.azurewebsites.net/webhook -ExpirationDate "2017-09-01" -ClientState "Hello State!"
```
Add a Webhook subscription for the specified notification Url on the list MyList with an expiration date set on September 1st, 2017 with a specific client state
