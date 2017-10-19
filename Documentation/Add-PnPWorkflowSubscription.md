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
```

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


```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -StartOnChanged


```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -StartOnCreated


```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -TaskListName


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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)