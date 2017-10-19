---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Execute-PnPQuery

## SYNOPSIS
Execute the current queued actions

## SYNTAX 

```powershell
Execute-PnPQuery [-RetryCount <Int>]
                 [-RetryWait <Int>]
```

## DESCRIPTION
Executes any queued actions / changes on the SharePoint Client Side Object Model Context

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Execute-PnPQuery -RetryCount 5
```

This will execute any queued actions / changes on the SharePoint Client Side Object Model Context and will retry 5 times in case of throttling.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Execute-PnPQuery -RetryWait 10
```

This will execute any queued actions / changes on the SharePoint Client Side Object Model Context and delay the execution for 10 seconds before it retries the execution.

## PARAMETERS

### -RetryCount
Number of times to retry in case of throttling. Defaults to 10.

```yaml
Type: Int
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -RetryWait
Delay in seconds. Defaults to 1.

```yaml
Type: Int
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)