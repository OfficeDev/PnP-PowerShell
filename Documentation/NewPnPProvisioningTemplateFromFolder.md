#New-PnPProvisioningTemplateFromFolder
Generates a provisioning template from a given folder, including only files that are present in that folder
##Syntax
```powershell
New-PnPProvisioningTemplateFromFolder [-Match <String>]
                                      [-ContentType <ContentTypePipeBind>]
                                      [-Properties <Hashtable>]
                                      [-AsIncludeFile [<SwitchParameter>]]
                                      [-Force [<SwitchParameter>]]
                                      [-Encoding <Encoding>]
                                      [-Web <WebPipeBind>]
                                      [-Out <String>]
                                      [-Folder <String>]
                                      [-TargetFolder <String>]
                                      [-Schema <XMLPnPSchemaVersion>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AsIncludeFile|SwitchParameter|False|If specified, the output will only contain the <pnp:Files> element. This allows the output to be included in another template.|
|ContentType|ContentTypePipeBind|False|An optional content type to use.|
|Encoding|Encoding|False|The encoding type of the XML file, Unicode is default|
|Folder|String|False|Folder to process. If not specified the current folder will be used.|
|Force|SwitchParameter|False|Overwrites the output file if it exists.|
|Match|String|False|Optional wildcard pattern to match filenames against. If empty all files will be included.|
|Out|String|False|Filename to write to, optionally including full path.|
|Properties|Hashtable|False|Additional properties to set for every file entry in the generated template.|
|Schema|XMLPnPSchemaVersion|False|The schema of the output to use, defaults to the latest schema|
|TargetFolder|String|False|Target folder to provision to files to. If not specified, the current folder name will be used.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml
```
Creates an empty provisioning template, and includes all files in the current folder.

###Example 2
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp
```
Creates an empty provisioning template, and includes all files in the c:\temp folder.

###Example 3
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp -Match *.js
```
Creates an empty provisioning template, and includes all files with a JS extension in the c:\temp folder.

###Example 4
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp -Match *.js -TargetFolder "Shared Documents"
```
Creates an empty provisioning template, and includes all files with a JS extension in the c:\temp folder and marks the files in the template to be added to the 'Shared Documents' folder

###Example 5
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp -Match *.js -TargetFolder "Shared Documents" -ContentType "Test Content Type"
```
Creates an empty provisioning template, and includes all files with a JS extension in the c:\temp folder and marks the files in the template to be added to the 'Shared Documents' folder. It will add a property to the item for the content type.

###Example 6
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.xml -Folder c:\temp -Match *.js -TargetFolder "Shared Documents" -Properties @{"Title" = "Test Title"; "Category"="Test Category"}
```
Creates an empty provisioning template, and includes all files with a JS extension in the c:\temp folder and marks the files in the template to be added to the 'Shared Documents' folder. It will add the specified properties to the file entries.

###Example 7
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.pnp
```
Creates an empty provisioning template as a pnp package file, and includes all files in the current folder

###Example 8
```powershell
PS:> New-PnPProvisioningTemplateFromFolder -Out template.pnp -Folder c:\temp
```
Creates an empty provisioning template as a pnp package file, and includes all files in the c:\temp folder
