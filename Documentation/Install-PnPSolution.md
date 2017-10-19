---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Install-PnPSolution

## SYNOPSIS
Installs a sandboxed solution to a site collection. WARNING! This method can delete your composed look gallery due to the method used to activate the solution. We recommend you to only to use this cmdlet if you are okay with that.

## SYNTAX 

```powershell
Install-PnPSolution -PackageId <GuidPipeBind>
                    -SourceFilePath <String>
                    [-MajorVersion <Int>]
                    [-MinorVersion <Int>]
```

## PARAMETERS

### -MajorVersion
Optional major version of the solution, defaults to 1

```yaml
Type: Int
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -MinorVersion
Optional minor version of the solution, defaults to 0

```yaml
Type: Int
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -PackageId
ID of the solution, from the solution manifest

```yaml
Type: GuidPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -SourceFilePath
Path to the sandbox solution package (.WSP) file

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)