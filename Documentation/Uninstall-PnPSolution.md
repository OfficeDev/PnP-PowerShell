---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Uninstall-PnPSolution

## SYNOPSIS
Uninstalls a sandboxed solution from a site collection

## SYNTAX 

```powershell
Uninstall-PnPSolution -PackageId <GuidPipeBind>
                      -PackageName <String>
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

### -PackageName
Filename of the WSP file to uninstall

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)