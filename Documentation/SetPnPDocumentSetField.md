#Set-PnPDocumentSetField
Sets a site column from the available content types to a document set
##Syntax
```powershell
Set-PnPDocumentSetField -DocumentSet <DocumentSetPipeBind>
                        -Field <FieldPipeBind>
                        [-SetSharedField [<SwitchParameter>]]
                        [-SetWelcomePageField [<SwitchParameter>]]
                        [-RemoveSharedField [<SwitchParameter>]]
                        [-RemoveWelcomePageField [<SwitchParameter>]]
                        [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|DocumentSet|DocumentSetPipeBind|True|The document set in which to set the field. Either specify a name, a document set template object, an id, or a content type object|
|Field|FieldPipeBind|True|The field to set. The field needs to be available in one of the available content types. Either specify a name, an id or a field object|
|RemoveSharedField|SwitchParameter|False|Removes the field as a Shared Field|
|RemoveWelcomePageField|SwitchParameter|False|Removes the field as a Welcome Page Field|
|SetSharedField|SwitchParameter|False|Set the field as a Shared Field|
|SetWelcomePageField|SwitchParameter|False|Set the field as a Welcome Page field|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Set-PnPDocumentSetField -Field "Test Field" -DocumentSet "Test Document Set" -SetSharedField -SetWelcomePageField
```
This will set the field, available in one of the available content types, as a Shared Field and as a Welcome Page Field.

###Example 2
```powershell
PS:> Set-PnPDocumentSetField -Field "Test Field" -DocumentSet "Test Document Set" -RemoveSharedField -RemoveWelcomePageField
```
This will remove the field, available in one of the available content types, as a Shared Field and as a Welcome Page Field.
