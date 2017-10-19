---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPDocumentSet

## SYNOPSIS
Creates a new document set in a library.

## SYNTAX 

```powershell
Add-PnPDocumentSet -List <ListPipeBind>
                   -Name <String>
                   -ContentType <ContentTypePipeBind>
                   [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPDocumentSet -List "Documents" -ContentType "Test Document Set" -Name "Test"
```

This will add a new document set based upon the 'Test Document Set' content type to a list called 'Documents'. The document set will be named 'Test'

## PARAMETERS

### -ContentType
The name of the content type, its ID or an actual content object referencing to the document set.

```yaml
Type: ContentTypePipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -List
The name of the list, its ID or an actual list object from where the document set needs to be added

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Name
The name of the document set

```yaml
Type: String
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

## OUTPUTS

### System.String

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)