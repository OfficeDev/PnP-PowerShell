---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Add-PnPWebhookSubscription

## SYNOPSIS
Adds a new Webhook subscription

## SYNTAX 

```powershell
Add-PnPWebhookSubscription -NotificationUrl <String>
                           [-List <ListPipeBind>]
                           [-ExpirationDate <DateTime>]
                           [-ClientState <String>]
                           [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPWebhookSubscription -List MyList -NotificationUrl https://my-func.azurewebsites.net/webhook
```

Add a Webhook subscription for the specified notification Url on the list MyList

### ------------------EXAMPLE 2------------------
```powershell
PS:> Add-PnPWebhookSubscription -List MyList -NotificationUrl https://my-func.azurewebsites.net/webhook -ExpirationDate "2017-09-01"
```

Add a Webhook subscription for the specified notification Url on the list MyList with an expiration date set on September 1st, 2017

### ------------------EXAMPLE 3------------------
```powershell
PS:> Add-PnPWebhookSubscription -List MyList -NotificationUrl https://my-func.azurewebsites.net/webhook -ExpirationDate "2017-09-01" -ClientState "Hello State!"
```

Add a Webhook subscription for the specified notification Url on the list MyList with an expiration date set on September 1st, 2017 with a specific client state

## PARAMETERS

### -ClientState
A client state information that will be passed through notifications

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

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
The list object or name where the Webhook subscription will be added to

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

Required: True
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