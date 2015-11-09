#Add-SPOCustomAction
*Topic automatically generated on: 2015-10-29*

Adds a custom action to a web
##Syntax
```powershell
Add-SPOCustomAction -Name <String> -Title <String> -Description <String> -Group <String> -Location <String> [-Sequence <Int32>] [-Url <String>] [-ImageUrl <String>] [-CommandUIExtension <String>] [-RegistrationId <String>] [-Rights <PermissionKind[]>] [-RegistrationType <UserCustomActionRegistrationType>] [-Scope <CustomActionScope>] [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|CommandUIExtension|String|False||
|Description|String|True||
|Group|String|True||
|ImageUrl|String|False||
|Location|String|True||
|Name|String|True||
|RegistrationId|String|False||
|RegistrationType|UserCustomActionRegistrationType|False||
|Rights|PermissionKind[]|False||
|Scope|CustomActionScope|False||
|Sequence|Int32|False||
|Title|String|True||
|Url|String|False||
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
$cUIExtn = "<CommandUIExtension><CommandUIDefinitions><CommandUIDefinition Location=""Ribbon.List.Share.Controls._children""><Button Id=""Ribbon.List.Share.GetItemsCountButton"" Alt=""Get list items count"" Sequence=""11"" Command=""Invoke_GetItemsCountButtonRequest"" LabelText=""Get Items Count"" TemplateAlias=""o1"" Image32by32=""_layouts/15/images/placeholder32x32.png"" Image16by16=""_layouts/15/images/placeholder16x16.png"" /></CommandUIDefinition></CommandUIDefinitions><CommandUIHandlers><CommandUIHandler Command=""Invoke_GetItemsCountButtonRequest"" CommandAction=""javascript: alert('Total items in this list: '+ ctx.TotalListItems);"" EnabledScript=""javascript: function checkEnable() { return (true);} checkEnable();""/></CommandUIHandlers></CommandUIExtension>"
Add-SPOCustomAction -Name 'ff1591d2-8613-4a1c-b465-d647a34b2555-GetItemsCount' -Title 'Invoke GetItemsCount Action' -Description 'Adds custom action to custom list ribbon' -Group 'Microsoft.SharePoint.Client.UserCustomAction.group' -Location 'CommandUI.Ribbon' -CommandUIExtension $cUIExtn
```
Adds a new custom action to the custom list template, and sets the Title, Name and other fields with the specified values. On click it shows the number of items in that list. Notice, escape quotes in CommandUIExtension.
