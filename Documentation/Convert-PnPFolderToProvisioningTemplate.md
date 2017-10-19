---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Convert-PnPFolderToProvisioningTemplate

## SYNOPSIS
Creates a pnp package file of an existing template xml, and includes all files in the current folder

## SYNTAX 

```powershell
Convert-PnPFolderToProvisioningTemplate -Out <String>
                                        [-Force [<SwitchParameter>]]
                                        [-Folder <String>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Convert-PnPFolderToProvisioningTemplate -Out template.pnp
```

Creates a pnp package file of an existing template xml, and includes all files in the current folder

### ------------------EXAMPLE 2------------------
```powershell
PS:> Convert-PnPFolderToProvisioningTemplate -Out template.pnp -Folder c:\temp
```

Creates a pnp package file of an existing template xml, and includes all files in the c:\temp folder

## PARAMETERS

### -Folder
Folder to process. If not specified the current folder will be used.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: 1
Accept pipeline input: False
```

### -Force
Overwrites the output file if it exists.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Out
Filename to write to, optionally including full path.

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)