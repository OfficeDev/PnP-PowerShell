# Set-PnPDefaultColumnValues
Sets default column values for a document library
## Syntax
```powershell
Set-PnPDefaultColumnValues -Field <FieldPipeBind>
                           -Value <String[]>
                           -List <ListPipeBind>
                           [-Folder <String>]
                           [-Web <WebPipeBind>]
```


## Detailed Description
Sets default column values for a document library, per folder, or for the root folder if the folder parameter has not been specified. Supports both text and taxonomy fields.

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Field|FieldPipeBind|True|The internal name, id or a reference to a field|
|List|ListPipeBind|True|The ID, Name or Url of the list.|
|Value|String[]|True|A list of values. In case of a text field the values will be concatenated, separated by a semi-colon. In case of a taxonomy field multiple values will added. In case of people field multiple values will be added.|
|Folder|String|False|A library relative folder path, if not specified it will set the default column values on the root folder of the library ('/')|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Set-PnPDefaultColumnValues -List Documents -Field TaxKeyword -Value "Company|Locations|Stockholm"
```
Sets a default value for the enterprise keywords field on a library to a term called "Stockholm", located in the "Locations" term set, which is part of the "Company" term group

### Example 2
```powershell
PS:> Set-PnPDefaultColumnValues -List Documents -Field TaxKeyword -Value "15c4c4e4-4b67-4894-a1d8-de5ff811c791"
```
Sets a default value for the enterprise keywords field on a library to a term with the id "15c4c4e4-4b67-4894-a1d8-de5ff811c791". You need to ensure the term is valid for the field.

### Example 3
```powershell
PS:> Set-PnPDefaultColumnValues -List Documents -Field MyTextField -Value "DefaultValue"
```
Sets a default value for the MyTextField text field on a library to a value of "DefaultValue"

### Example 4
```powershell
PS:> Set-PnPDefaultColumnValues -List Documents -Field MyPeopleField -Value "1;#Foo Bar"
```
Sets a default value for the MyPeopleField people field on a library to a value of "Foo Bar" using the id from the user information list.

### Example 5
```powershell
PS:> $user = New-PnPUser -LoginName foobar@contoso.com
PS:> Set-PnPDefaultColumnValues -List Documents -Field MyPeopleField -Value "$($user.Id);#$($user.LoginName)"
```
Sets a default value for the MyPeopleField people field on a library to a value of "Foo Bar" using the id from the user information list.

### Example 6
```powershell
PS:> $user1 = New-PnPUser -LoginName user1@contoso.com
PS:> $user2 = New-PnPUser -LoginName user2@contoso.com
PS:> Set-PnPDefaultColumnValues -List Documents -Field MyMultiPeopleField -Value "$($user1.Id);#$($user1.LoginName)","$($user2.Id);#$($user2.LoginName)"
```
Sets a default value for the MyMultiPeopleField people field on a library to a value of "User 1" and "User 2" using the id from the user information list.
