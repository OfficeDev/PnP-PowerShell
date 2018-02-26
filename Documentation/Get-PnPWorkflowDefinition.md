---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPWorkflowDefinition

## SYNOPSIS
Returns a workflow definition

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
PS:> Get-PnPWorkflowDefinition -Name MyWorkflow
```

Gets a Workflow with the name "MyWorkflow".

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPWorkflowDefinition -Name MyWorkflow -PublishedOnly $false
```

Gets a Workflow with the name "MyWorkflow" that is published.

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
Optional connection to be used by the cmdlet. Retrieve the value for this parameter by either specifying -ReturnConnection on Connect-PnPOnline or by executing Get-PnPConnection.

```yaml
Type: SPOnlineConnection
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Web
This parameter allows you to optionally apply the cmdlet action to a subweb within the current web. In most situations this parameter is not required and you can connect to the subweb using Connect-PnPOnline instead. Specify the GUID, server relative url (i.e. /sites/team1) or web instance of the web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

## OUTPUTS

### [Microsoft.SharePoint.Client.WorkflowServices.WorkflowDefinition](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.workflowservices.workflowdefinition.aspx)

## RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)
