---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPWorkflowDefinition

## SYNOPSIS
Removes a workflow definition

## SYNTAX 

```powershell
Remove-PnPWorkflowDefinition -Identity <WorkflowDefinitionPipeBind>
                             [-Web <WebPipeBind>]
                             [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
Remove-PnPWorkflowDefinition -Identity $wfDef
```

Removes the workflow, retrieved by Get-PnPWorkflowDefinition, from the site.

### ------------------EXAMPLE 2------------------
```powershell
Get-PnPWorkflowDefinition -Name MyWorkflow | Remove-PnPWorkflowDefinition
```

Get the workflow MyWorkFlow and remove from the site.

## PARAMETERS

### -Identity
The definition to remove

```yaml
Type: WorkflowDefinitionPipeBind
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