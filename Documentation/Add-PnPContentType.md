---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPContentType

## SYNOPSIS
Adds a new content type

## SYNTAX 

```powershell
Add-PnPContentType -Name <String>
                   [-ContentTypeId <String>]
                   [-Description <String>]
                   [-Group <String>]
                   [-ParentContentType <ContentType>]
                   [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPContentType -Name "Project Document" -Description "Use for Contoso projects" -Group "Contoso Content Types" -ParentContentType $ct
```

This will add a new content type based on the parent content type stored in the $ct variable.

## PARAMETERS

### -ContentTypeId
If specified, in the format of 0x0100233af432334r434343f32f3, will create a content type with the specific ID

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Description
Specifies the description of the new content type

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Group
Specifies the group of the new content type

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Name
Specify the name of the new content type

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -ParentContentType
Specifies the parent of the new content type

```yaml
Type: ContentType
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

### [Microsoft.SharePoint.Client.ContentType](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.contenttype.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)