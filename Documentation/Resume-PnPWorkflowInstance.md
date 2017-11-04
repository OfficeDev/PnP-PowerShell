---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Resume-PnPWorkflowInstance

## SYNOPSIS
Resume a workflow

## SYNTAX 

```powershell
Resume-PnPWorkflowInstance -Identity <WorkflowInstancePipeBind>
                           [-Web <WebPipeBind>]
                           [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Resumes a previously stopped workflow instance

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
Resume-PnPWorkflowInstance -identity $wfInstance
```

Resumes the workflow instance, this can be the Guid of the instance or the instance itself.

## PARAMETERS

### -Identity
The instance to resume

```yaml
Type: WorkflowInstancePipeBind
Parameter Sets: (All)

Required: True
Position: 0
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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)