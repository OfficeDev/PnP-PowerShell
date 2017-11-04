---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPWorkflowDefinition

## SYNOPSIS
Return a workflow definition

## SYNTAX 

```powershell
Get-PnPWorkflowDefinition [-PublishedOnly [<SwitchParameter>]]
                          [-Name <String>]
                          [-Web <WebPipeBind>]
                          [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Returns a workflow definition

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
Get-PnPWorkflowDefinition -Name MyWorkflow
```

Gets an Workflow with the name "MyWorkflow".

### ------------------EXAMPLE 2------------------
```powershell
Get-PnPWorkflowDefinition -Name MyWorkflow -PublishedOnly $false
```

Gets an Workflow with the name "MyWorkflow" that is published.

## PARAMETERS

### -Name
The name of the workflow

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: 0
Accept pipeline input: False
```

### -PublishedOnly
Return only the published workflows

```yaml
Type: SwitchParameter
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

### [Microsoft.SharePoint.Client.WorkflowServices.WorkflowDefinition](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.workflowservices.workflowdefinition.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)