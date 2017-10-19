---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPFieldFromContentType

## SYNOPSIS
Removes a site column from a content type

## SYNTAX 

```powershell
Remove-PnPFieldFromContentType -Field <FieldPipeBind>
                               -ContentType <ContentTypePipeBind>
                               [-DoNotUpdateChildren [<SwitchParameter>]]
                               [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPFieldFromContentType -Field "Project_Name" -ContentType "Project Document"
```

This will remove the site column with an internal name of "Project_Name" from a content type called "Project Document"

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPFieldFromContentType -Field "Project_Name" -ContentType "Project Document" -DoNotUpdateChildren
```

This will remove the site column with an internal name of "Project_Name" from a content type called "Project Document". It will not update content types that inherit from the "Project Document" content type.

## PARAMETERS

### -ContentType
The content type where the field is to be removed from.

```yaml
Type: ContentTypePipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -DoNotUpdateChildren
If specified, inherited content types will not be updated.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Field
The field to remove.

```yaml
Type: FieldPipeBind
Parameter Sets: (All)

Required: True
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

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)