---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Invoke-PnPWebAction

## SYNOPSIS
Executes operations on web, lists and list items.

## SYNTAX 

```powershell
Invoke-PnPWebAction [-Webs <Web[]>]
                    [-WebAction <Action`1>]
                    [-ShouldProcessWebAction <Func`2>]
                    [-PostWebAction <Action`1>]
                    [-ShouldProcessPostWebAction <Func`2>]
                    [-WebProperties <String[]>]
                    [-ListAction <Action`1>]
                    [-ShouldProcessListAction <Func`2>]
                    [-PostListAction <Action`1>]
                    [-ShouldProcessPostListAction <Func`2>]
                    [-ListProperties <String[]>]
                    [-ListItemAction <Action`1>]
                    [-ShouldProcessListItemAction <Func`2>]
                    [-ListItemProperties <String[]>]
                    [-SubWebs [<SwitchParameter>]]
                    [-DisableStatisticsOutput [<SwitchParameter>]]
                    [-SkipCounting [<SwitchParameter>]]
                    [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Invoke-PnPWebAction -ListAction ${function:ListAction}
```

This will call the function ListAction on all the lists located on the current web.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Invoke-PnPWebAction -ShouldProcessListAction ${function:ShouldProcessList} -ListAction ${function:ListAction}
```

This will call the function ShouldProcessList, if it returns true the function ListAction will then be called. This will occur on all lists located on the current web

## PARAMETERS

### -DisableStatisticsOutput
Will not output statistics after the operation

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ListAction
Function to be executed on the list. There is one input parameter of type List

```yaml
Type: Action`1
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ListItemAction
Function to be executed on the list item. There is one input parameter of type ListItem

```yaml
Type: Action`1
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ListItemProperties
The properties to load for list items.

```yaml
Type: String[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ListProperties
The properties to load for list.

```yaml
Type: String[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -PostListAction
Function to be executed on the list, this will trigger after list items have been processed. There is one input parameter of type List

```yaml
Type: Action`1
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -PostWebAction
Function to be executed on the web, this will trigger after lists and list items have been processed. There is one input parameter of type Web

```yaml
Type: Action`1
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ShouldProcessListAction
Function to be executed on the web that would determine if ListAction should be invoked, There is one input parameter of type List and the function should return a boolean value

```yaml
Type: Func`2
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ShouldProcessListItemAction
Function to be executed on the web that would determine if ListItemAction should be invoked, There is one input parameter of type ListItem and the function should return a boolean value

```yaml
Type: Func`2
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ShouldProcessPostListAction
Function to be executed on the web that would determine if PostListAction should be invoked, There is one input parameter of type List and the function should return a boolean value

```yaml
Type: Func`2
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ShouldProcessPostWebAction
Function to be executed on the web that would determine if PostWebAction should be invoked, There is one input parameter of type Web and the function should return a boolean value

```yaml
Type: Func`2
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ShouldProcessWebAction
Function to be executed on the web that would determine if WebAction should be invoked, There is one input parameter of type Web and the function should return a boolean value

```yaml
Type: Func`2
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -SkipCounting
Will skip the counting process; by doing this you will not get an estimated time remaining

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -SubWebs
Specify if sub webs will be processed

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

### -WebAction
Function to be executed on the web. There is one input parameter of type Web

```yaml
Type: Action`1
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -WebProperties
The properties to load for web.

```yaml
Type: String[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Webs
Webs you want to process (for example different site collections), will use Web parameter if not specified

```yaml
Type: Web[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)