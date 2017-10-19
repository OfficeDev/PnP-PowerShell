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
                          [-Web <WebPipeBind>]
                          [-Name <String>]
```

## DESCRIPTION
Returns a workflow definition

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


```yaml
Type: SwitchParameter
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