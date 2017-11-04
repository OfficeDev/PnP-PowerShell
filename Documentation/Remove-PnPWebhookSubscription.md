---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Remove-PnPWebhookSubscription

## SYNOPSIS
Removes a Webhook subscription from the resource

## SYNTAX 

```powershell
Remove-PnPWebhookSubscription -Identity <WebhookSubscriptionPipeBind>
                              [-List <ListPipeBind>]
                              [-Force [<SwitchParameter>]]
                              [-Web <WebPipeBind>]
                              [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPWebhookSubscription -List MyList -Identity ea1533a8-ff03-415b-a7b6-517ee50db8b6
```

Removes the Webhook subscription with the specified id from the list MyList

### ------------------EXAMPLE 2------------------
```powershell
PS:> $subscriptions = Get-PnPWebhookSubscriptions -List MyList
PS:> Remove-PnPWebhookSubscription -Identity $subscriptions[0] -List MyList
```

Removes the first Webhook subscription from the list MyList

### ------------------EXAMPLE 3------------------
```powershell
PS:> $subscriptions = Get-PnPWebhookSubscriptions -List MyList
PS:> $subscriptions[0] | Remove-PnPWebhookSubscription -List MyList
```

Removes the first Webhook subscription from the list MyList

## PARAMETERS

### -Force
Specifying the Force parameter will skip the confirmation question.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
The identity of the Webhook subscription to remove

```yaml
Type: WebhookSubscriptionPipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -List
The list object or name which the Webhook subscription will be removed from

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
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