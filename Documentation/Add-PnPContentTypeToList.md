---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPContentTypeToList

## SYNOPSIS
Adds a new content type to a list

## SYNTAX 

```powershell
Add-PnPContentTypeToList -List <ListPipeBind>
                         -ContentType <ContentTypePipeBind>
                         [-DefaultContentType [<SwitchParameter>]]
                         [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPContentTypeToList -List "Documents" -ContentType "Project Document" -DefaultContentType
```

This will add an existing content type to a list and sets it as the default content type

## PARAMETERS

### -ContentType
Specifies the content type that needs to be added to the list

```yaml
Type: ContentTypePipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -DefaultContentType
Specify if the content type needs to be the default content type or not

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -List
Specifies the list the content type needs to be added to

```yaml
Type: ListPipeBind
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