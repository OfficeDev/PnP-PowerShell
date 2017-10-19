---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPTraceLog

## SYNOPSIS
Turn log tracing on or off

## SYNTAX 

### On
```powershell
Set-PnPTraceLog -On [<SwitchParameter>]
                [-LogFile <String>]
                [-Level <LogLevel>]
                [-Delimiter <String>]
                [-IndentSize <Int>]
                [-AutoFlush <Boolean>]
```

### Off
```powershell
Set-PnPTraceLog -Off [<SwitchParameter>]
```

## DESCRIPTION
Defines if tracing should be turned on. PnP Core, which is the foundation of these cmdlets, uses the standard Trace functionality of .NET. With this cmdlet you can turn capturing of this trace to a log file on or off.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPTraceLog -On -LogFile traceoutput.txt
```

This turns on trace logging to the file 'traceoutput.txt' and will capture events of at least 'Information' level.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPTraceLog -On -LogFile traceoutput.txt -Level Debug
```

This turns on trace logging to the file 'traceoutput.txt' and will capture debug events.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Set-PnPTraceLog -On -LogFile traceoutput.txt -Level Debug -Delimiter ","
```

This turns on trace logging to the file 'traceoutput.txt' and will write the entries as comma separated. Debug events are captured.

### ------------------EXAMPLE 4------------------
```powershell
PS:> Set-PnPTraceLog -Off
```

This turns off trace logging. It will flush any remaining messages to the log file.

## PARAMETERS

### -AutoFlush
Auto flush the trace log. Defaults to true.

```yaml
Type: Boolean
Parameter Sets: On

Required: False
Position: Named
Accept pipeline input: False
```

### -Delimiter
If specified the trace log entries will be delimited with this value.

```yaml
Type: String
Parameter Sets: On

Required: False
Position: Named
Accept pipeline input: False
```

### -IndentSize
Indents in the tracelog will be with this amount of characters. Defaults to 4.

```yaml
Type: Int
Parameter Sets: On

Required: False
Position: Named
Accept pipeline input: False
```

### -Level
The level of events to capture. Possible values are 'Debug', 'Error', 'Warning', 'Information'. Defaults to 'Information'.

```yaml
Type: LogLevel
Parameter Sets: On

Required: False
Position: Named
Accept pipeline input: False
```

### -LogFile
The path and filename of the file to write the trace log to.

```yaml
Type: String
Parameter Sets: On

Required: False
Position: Named
Accept pipeline input: False
```

### -Off
Turn off tracing to log file.

```yaml
Type: SwitchParameter
Parameter Sets: Off

Required: True
Position: Named
Accept pipeline input: False
```

### -On
Turn on tracing to log file

```yaml
Type: SwitchParameter
Parameter Sets: On

Required: True
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)