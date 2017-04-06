# Set-PnPUnifiedGroup
Sets Office 365 Group (aka Unified Group) properties
## Syntax
```powershell
Set-PnPUnifiedGroup -Identity <UnifiedGroupPipeBind>
                    [-DisplayName <String>]
                    [-Description <String>]
                    [-Owners <String[]>]
                    [-Members <String[]>]
                    [-IsPrivate [<SwitchParameter>]]
                    [-GroupLogoPath <String>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|UnifiedGroupPipeBind|True|The Identity of the Office 365 Group.|
|Description|String|False|The Description of the group to set.|
|DisplayName|String|False|The DisplayName of the group to set.|
|GroupLogoPath|String|False|The path to the logo file of to set.|
|IsPrivate|SwitchParameter|False|Makes the group private when selected.|
|Members|String[]|False|The array UPN values of members to add to the group.|
|Owners|String[]|False|The array UPN values of owners to add to the group.|
## Examples

### Example 1
```powershell
PS:> Set-PnPUnifiedGroup -Identity $group -DisplayName "My Displayname"
```
Sets the display name of the group where $group is a Group entity

### Example 2
```powershell
PS:> Set-PnPUnifiedGroup -Identity $groupId -Descriptions "My Description" -DisplayName "My DisplayName"
```
Sets the display name and description of a group based upon its ID

### Example 3
```powershell
PS:> Set-PnPUnifiedGroup -Identity $group -GroupLogoPath ".\MyLogo.png"
```
Sets a specific Office 365 Group logo.

### Example 4
```powershell
PS:> Set-PnPUnifiedGroup -Identity $group -IsPrivate:$false
```
Sets a group to be Public if previously Private.

### Example 5
```powershell
PS:> Set-PnPUnifiedGroup -Identity $group -Owners demo@contoso.com
```
Adds demo@contoso.com as an additional owner to the group.
