---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPTaxonomyField

## SYNOPSIS
Add a taxonomy field

## SYNTAX 

### Id
```powershell
Add-PnPTaxonomyField -DisplayName <String>
                     -InternalName <String>
                     [-TaxonomyItemId <GuidPipeBind>]
                     [-List <ListPipeBind>]
                     [-Group <String>]
                     [-Id <GuidPipeBind>]
                     [-AddToDefaultView [<SwitchParameter>]]
                     [-MultiValue [<SwitchParameter>]]
                     [-Required [<SwitchParameter>]]
                     [-FieldOptions <AddFieldOptions>]
                     [-Web <WebPipeBind>]
                     [-Connection <SPOnlineConnection>]
```

### Path
```powershell
Add-PnPTaxonomyField -TermSetPath <String>
                     -DisplayName <String>
                     -InternalName <String>
                     [-TermPathDelimiter <String>]
                     [-List <ListPipeBind>]
                     [-Group <String>]
                     [-Id <GuidPipeBind>]
                     [-AddToDefaultView [<SwitchParameter>]]
                     [-MultiValue [<SwitchParameter>]]
                     [-Required [<SwitchParameter>]]
                     [-FieldOptions <AddFieldOptions>]
                     [-Web <WebPipeBind>]
                     [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Adds a taxonomy/managed metadata field to a list or as a site column.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPTaxonomyField -DisplayName "Test" -InternalName "Test" -TermSetPath "TestTermGroup|TestTermSet"
```

Adds a new taxonomy field called "Test" that points to the TestTermSet which is located in the TestTermGroup

## PARAMETERS

### -AddToDefaultView
Switch Parameter if this field must be added to the default view

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -DisplayName
The display name of the field

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -FieldOptions
Specifies the control settings while adding a field. See https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.addfieldoptions.aspx for details

```yaml
Type: AddFieldOptions
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Group
The group name to where this field belongs to

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Id
The ID for the field, must be unique

```yaml
Type: GuidPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -InternalName
The internal name of the field

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -List
The list object or name where this field needs to be added

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -MultiValue
Switch Parameter if this Taxonomy field can hold multiple values

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Required
Switch Parameter if the field is a required field

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -TaxonomyItemId
The ID of the Taxonomy item

```yaml
Type: GuidPipeBind
Parameter Sets: Id

Required: False
Position: Named
Accept pipeline input: False
```

### -TermPathDelimiter
The path delimiter to be used, by default this is '|'

```yaml
Type: String
Parameter Sets: Path

Required: False
Position: Named
Accept pipeline input: False
```

### -TermSetPath
The path to the term that this needs be be bound

```yaml
Type: String
Parameter Sets: Path

Required: True
Position: Named
Accept pipeline input: False
```

### -Connection
Optional connection to be used by cmdlet. Retrieve the value for this parameter by eiter specifying -ReturnConnection on Connect-PnPOnline or by executing Get-PnPConnection.

```yaml
Type: SPOnlineConnection
Parameter Sets: (All)

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

## OUTPUTS

### [Microsoft.SharePoint.Client.Field](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)