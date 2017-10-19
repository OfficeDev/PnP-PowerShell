---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPContentType

## SYNOPSIS
Removes a content type from a web

## SYNTAX 

```powershell
Remove-PnPContentType -Identity <ContentTypePipeBind>
                      [-Force [<SwitchParameter>]]
                      [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPContentType -Identity "Project Document"
```

This will remove a content type called "Project Document" from the current web

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPContentType -Identity "Project Document" -Force
```

This will remove a content type called "Project Document" from the current web with force

## PARAMETERS

### -Force
Specifying the Force parameter will skip the confirmation question.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
The name or ID of the content type to remove

```yaml
Type: ContentTypePipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
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