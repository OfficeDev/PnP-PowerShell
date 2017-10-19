---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPCustomAction

## SYNOPSIS
Adds a custom action

## SYNTAX 

### Client Side Component Id
```powershell
Add-PnPCustomAction -Name <String>
                    -Title <String>
                    -Location <String>
                    -ClientSideComponentId <GuidPipeBind>
                    [-RegistrationId <String>]
                    [-RegistrationType <UserCustomActionRegistrationType>]
                    [-Scope <CustomActionScope>]
                    [-ClientSideComponentProperties <String>]
                    [-Web <WebPipeBind>]
```

### Default
```powershell
Add-PnPCustomAction -Name <String>
                    -Title <String>
                    -Description <String>
                    -Group <String>
                    -Location <String>
                    [-Sequence <Int>]
                    [-Url <String>]
                    [-ImageUrl <String>]
                    [-CommandUIExtension <String>]
                    [-RegistrationId <String>]
                    [-Rights <PermissionKind[]>]
                    [-RegistrationType <UserCustomActionRegistrationType>]
                    [-Scope <CustomActionScope>]
                    [-Web <WebPipeBind>]
```

## DESCRIPTION
Adds a user custom action to a web or sitecollection.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
$cUIExtn = "<CommandUIExtension><CommandUIDefinitions><CommandUIDefinition Location=""Ribbon.List.Share.Controls._children""><Button Id=""Ribbon.List.Share.GetItemsCountButton"" Alt=""Get list items count"" Sequence=""11"" Command=""Invoke_GetItemsCountButtonRequest"" LabelText=""Get Items Count"" TemplateAlias=""o1"" Image32by32=""_layouts/15/images/placeholder32x32.png"" Image16by16=""_layouts/15/images/placeholder16x16.png"" /></CommandUIDefinition></CommandUIDefinitions><CommandUIHandlers><CommandUIHandler Command=""Invoke_GetItemsCountButtonRequest"" CommandAction=""javascript: alert('Total items in this list: '+ ctx.TotalListItems);"" EnabledScript=""javascript: function checkEnable() { return (true);} checkEnable();""/></CommandUIHandlers></CommandUIExtension>"

Add-PnPCustomAction -Name 'GetItemsCount' -Title 'Invoke GetItemsCount Action' -Description 'Adds custom action to custom list ribbon' -Group 'SiteActions' -Location 'CommandUI.Ribbon' -CommandUIExtension $cUIExtn
```

Adds a new custom action to the custom list template, and sets the Title, Name and other fields with the specified values. On click it shows the number of items in that list. Notice: escape quotes in CommandUIExtension.

## PARAMETERS

### -ClientSideComponentId
The Client Side Component Id of the custom action

```yaml
Type: GuidPipeBind
Parameter Sets: Client Side Component Id

Required: True
Position: Named
Accept pipeline input: False
```

### -ClientSideComponentProperties
The Client Side Component Properties of the custom action. Specify values as a json string : "{Property1 : 'Value1', Property2: 'Value2'}"

```yaml
Type: String
Parameter Sets: Client Side Component Id

Required: False
Position: Named
Accept pipeline input: False
```

### -CommandUIExtension
XML fragment that determines user interface properties of the custom action

```yaml
Type: String
Parameter Sets: Default

Required: False
Position: Named
Accept pipeline input: False
```

### -Description
The description of the custom action

```yaml
Type: String
Parameter Sets: Default

Required: True
Position: Named
Accept pipeline input: False
```

### -Group
The group where this custom action needs to be added like 'SiteActions'

```yaml
Type: String
Parameter Sets: Default

Required: True
Position: Named
Accept pipeline input: False
```

### -ImageUrl
The URL of the image associated with the custom action

```yaml
Type: String
Parameter Sets: Default

Required: False
Position: Named
Accept pipeline input: False
```

### -Location
The actual location where this custom action need to be added like 'CommandUI.Ribbon'

```yaml
Type: String
Parameter Sets: Default

Required: True
Position: Named
Accept pipeline input: False
```

### -Name
The name of the custom action

```yaml
Type: String
Parameter Sets: Default

Required: True
Position: Named
Accept pipeline input: False
```

### -RegistrationId
The identifier of the object associated with the custom action.

```yaml
Type: String
Parameter Sets: Default

Required: False
Position: Named
Accept pipeline input: False
```

### -RegistrationType
Specifies the type of object associated with the custom action

```yaml
Type: UserCustomActionRegistrationType
Parameter Sets: Default

Required: False
Position: Named
Accept pipeline input: False
```

### -Rights
A string array that contain the permissions needed for the custom action

```yaml
Type: PermissionKind[]
Parameter Sets: Default

Required: False
Position: Named
Accept pipeline input: False
```

### -Scope
The scope of the CustomAction to add to. Either Web or Site; defaults to Web. 'All' is not valid for this command.

```yaml
Type: CustomActionScope
Parameter Sets: Default

Required: False
Position: Named
Accept pipeline input: False
```

### -Sequence
Sequence of this CustomAction being injected. Use when you have a specific sequence with which to have multiple CustomActions being added to the page.

```yaml
Type: Int
Parameter Sets: Default

Required: False
Position: Named
Accept pipeline input: False
```

### -Title
The title of the custom action

```yaml
Type: String
Parameter Sets: Default

Required: True
Position: Named
Accept pipeline input: False
```

### -Url
The URL, URI or ECMAScript (JScript, JavaScript) function associated with the action

```yaml
Type: String
Parameter Sets: Default

Required: False
Position: Named
Accept pipeline input: False
```

### -Web
The GUID, server relative url (i.e. /sites/team1) or web instance of the web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)[UserCustomAction](https://msdn.microsoft.com/en-us/library/office/microsoft.sharepoint.client.usercustomaction.aspx)[BasePermissions](https://msdn.microsoft.com/en-us/library/office/microsoft.sharepoint.client.basepermissions.aspx)