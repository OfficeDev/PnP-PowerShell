---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPWorkflowSubscription

## SYNOPSIS
Adds a workflow subscription to a list

## SYNTAX 

```powershell
Add-PnPWorkflowSubscription -Name <String>
                            -DefinitionName <String>
                            -List <ListPipeBind>
                            -HistoryListName <String>
                            -TaskListName <String>
                            [-StartManually [<SwitchParameter>]]
                            [-StartOnCreated [<SwitchParameter>]]
                            [-StartOnChanged [<SwitchParameter>]]
                            [-AssociationValues <Dictionary`2>]
                            [-Web <WebPipeBind>]
                            [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
Add-PnPWorkflowSubscription -Name MyWorkflow -DefinitionName SendMessageWf -list $list
```

Adds an Workflow with the name 'SendMessageWf' to the list $list.

### ------------------EXAMPLE 2------------------
```powershell
$list | Add-PnPWorkflowSubscription -Name MyWorkflow -DefinitionName SendMessageWf
```

Adds an Workflow with the name "SendMessageWf" to the list $list.

### ------------------EXAMPLE 3------------------
```powershell
Get-PnPList -Identity "MyCustomList" | Add-PnPWorkflowSubscription -Name MyWorkflow -DefinitionName SendMessageWf
```

Adds an Workflow with the name "SendMessageWf" to the list "MyCustomList".

## PARAMETERS

### -AssociationValues


```yaml
Type: Dictionary`2
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -DefinitionName
The name of the workflow definition

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -HistoryListName
The name of the History list

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -List
The list to add the workflow to

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Name
The name of the subscription

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -StartManually
Switch if the workflow should be started manually, default value is 'true'

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -StartOnChanged
Should the workflow run when an item is changed

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -StartOnCreated
Should the workflow run when an new item is created

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -TaskListName
The name of the task list

```yaml
Type: String
Parameter Sets: (All)

Required: True
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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)