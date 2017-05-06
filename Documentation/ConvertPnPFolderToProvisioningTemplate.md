# Convert-PnPFolderToProvisioningTemplate
Creates a pnp package file of an existing template xml, and includes all files in the current folder
## Syntax
```powershell
Convert-PnPFolderToProvisioningTemplate -Out <String>
                                        [-Force [<SwitchParameter>]]
                                        [-Folder <String>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Out|String|True|Filename to write to, optionally including full path.|
|Folder|String|False|Folder to process. If not specified the current folder will be used.|
|Force|SwitchParameter|False|Overwrites the output file if it exists.|
## Examples

### Example 1
```powershell
PS:> Convert-PnPFolderToProvisioningTemplate -Out template.pnp
```
Creates a pnp package file of an existing template xml, and includes all files in the current folder

### Example 2
```powershell
PS:> Convert-PnPFolderToProvisioningTemplate -Out template.pnp -Folder c:\temp
```
Creates a pnp package file of an existing template xml, and includes all files in the c:\temp folder
