#Set-PnPUnifiedGroup
Sets Office 365 Group (aka Unified Group) properties
##Syntax
```powershell
Set-PnPUnifiedGroup -Identity <UnifiedGroupPipeBind>
                    [-DisplayName <String>]
                    [-Description <String>]
                    [-GroupLogoPath <String>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Description|String|False|The Description of the group to set.|
|DisplayName|String|False|The DisplayName of the group to set.|
|GroupLogoPath|String|False|The path to the logo file of to set.|
|Identity|UnifiedGroupPipeBind|True|The Identity of the Office 365 Group.|
##Examples

###Example 1
```powershell
PS:> Set-PnPUnifiedGroup -Identity $group -DisplayName "My Displayname"
```
Sets the display name of the group where $group is a Group entity

###Example 2
```powershell
PS:> Set-PnPUnifiedGroup -Identity $groupId -Descriptions "My Description" -DisplayName "My DisplayName"
```
Sets the display name and description of a group based upon its ID

###Example 3
```powershell
PS:> Set-PnPUnifiedGroup -Identity $group -GroupLogoPath ".\MyLogo.png"
```
Sets a specific Office 365 Group logo.
