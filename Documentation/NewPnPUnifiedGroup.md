# New-PnPUnifiedGroup
Creates a new Office 365 Group (aka Unified Group)
## Syntax
```powershell
New-PnPUnifiedGroup -DisplayName <String>
                    -Description <String>
                    -MailNickname <String>
                    [-Owners <String[]>]
                    [-Members <String[]>]
                    [-IsPrivate [<SwitchParameter>]]
                    [-GroupLogoPath <String>]
                    [-Force [<SwitchParameter>]]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Description|String|True|The Description of the Office 365 Group.|
|DisplayName|String|True|The Display Name of the Office 365 Group.|
|MailNickname|String|True|The Mail Nickname of the Office 365 Group.|
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question.|
|GroupLogoPath|String|False|The path to the logo file of to set.|
|IsPrivate|SwitchParameter|False|Makes the group private when selected.|
|Members|String[]|False|The array UPN values of the group's members.|
|Owners|String[]|False|The array UPN values of the group's owners.|
## Examples

### Example 1
```powershell
PS:> New-PnPUnifiedGroup -DisplayName $displayName -Description $description -MailNickname $nickname
```
Creates a public Office 365 Group with all the required properties

### Example 2
```powershell
PS:> New-PnPUnifiedGroup -DisplayName $displayName -Description $description -MailNickname $nickname -Owners $arrayOfOwners -Members $arrayOfMembers
```
Creates a public Office 365 Group with all the required properties, and with a custom list of Owners and a custom list of Members

### Example 3
```powershell
PS:> New-PnPUnifiedGroup -DisplayName $displayName -Description $description -MailNickname $nickname -IsPrivate
```
Creates a private Office 365 Group with all the required properties

### Example 4
```powershell
PS:> New-PnPUnifiedGroup -DisplayName $displayName -Description $description -MailNickname $nickname -Owners $arrayOfOwners -Members $arrayOfMembers -IsPrivate
```
Creates a private Office 365 Group with all the required properties, and with a custom list of Owners and a custom list of Members
