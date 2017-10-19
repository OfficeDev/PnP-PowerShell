---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Get-PnPWebhookSubscriptions

## SYNOPSIS
Gets all the Webhook subscriptions of the resource

## SYNTAX 

```powershell
Get-PnPWebhookSubscriptions [-List <ListPipeBind>]
                            [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPWebhookSubscriptions -List MyList
```

Gets all Webhook subscriptions of the list MyList

## PARAMETERS

### -List
The list object or name to get the Webhook subscriptions from

```yaml
Type: ListPipeBind
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