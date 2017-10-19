---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPWorkflowSubscription

## SYNOPSIS
Remove workflow subscription

## SYNTAX 

```powershell
Remove-PnPWorkflowSubscription -Identity <WorkflowSubscriptionPipeBind>
                               [-Web <WebPipeBind>]
```

## DESCRIPTION
Removes a previously registered workflow subscription

## PARAMETERS

### -Identity
The subscription to remove

```yaml
Type: WorkflowSubscriptionPipeBind
Parameter Sets: (All)

Required: True
Position: 0
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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)