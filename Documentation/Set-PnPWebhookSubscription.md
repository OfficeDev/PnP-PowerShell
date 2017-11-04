---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Set-PnPWebhookSubscription

## SYNOPSIS
Updates a Webhook subscription

## SYNTAX 

```powershell
Set-PnPWebhookSubscription -Subscription <WebhookSubscriptionPipeBind>
                           [-List <ListPipeBind>]
                           [-NotificationUrl <String>]
                           [-ExpirationDate <DateTime>]
                           [-Web <WebPipeBind>]
                           [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPWebhookSubscription -List MyList -Subscription ea1533a8-ff03-415b-a7b6-517ee50db8b6 -NotificationUrl https://my-func.azurewebsites.net/webhook
```

Updates an existing Webhook subscription with the specified id on the list MyList with a new Notification Url

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPWebhookSubscription -List MyList -Subscription ea1533a8-ff03-415b-a7b6-517ee50db8b6 -NotificationUrl https://my-func.azurewebsites.net/webhook -ExpirationDate "2017-09-01"
```

Updates an existing Webhook subscription with the specified id on the list MyList with a new Notification Url and a new expiration date

### ------------------EXAMPLE 3------------------
```powershell
PS:> $subscriptions = Get-PnPWebhookSubscriptions -List MyList
PS:> $updated = $subscriptions[0]
PS:> $updated.ExpirationDate = "2017-10-01"
PS:> Set-PnPWebhookSubscription -List MyList -Subscription $updated
```

Updates the Webhook subscription from the list MyList with a modified subscription object.
Note: The date will be converted to Universal Time

## PARAMETERS

### -ExpirationDate
The date at which the Webhook subscription will expire. (Default: 6 months from today)

```yaml
Type: DateTime
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -List
The list object or name from which the Webhook subscription will be modified

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -NotificationUrl
The URL of the Webhook endpoint that will be notified of the change

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Subscription
The identity of the Webhook subscription to update

```yaml
Type: WebhookSubscriptionPipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -Connection
Optional connection to be used by cmdlet. Retrieve the value for this parameter by eiter specifying -ReturnConnection on Connect-PnPOnline or by executing Get-PnPConnection.

```yaml
Type: SPOnlineConnection
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Web
The GUID, server relative url (i.e. /sites/team1) or web instance of the web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

## OUTPUTS

### OfficeDevPnP.Core.Entities.WebhookSubscription

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)