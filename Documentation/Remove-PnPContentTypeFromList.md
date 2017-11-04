---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPContentTypeFromList

## SYNOPSIS
Removes a content type from a list

## SYNTAX 

```powershell
Remove-PnPContentTypeFromList -List <ListPipeBind>
                              -ContentType <ContentTypePipeBind>
                              [-Web <WebPipeBind>]
                              [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPContentTypeFromList -List "Documents" -ContentType "Project Document"
```

This will remove a content type called "Project Document" from the "Documents" list

## PARAMETERS

### -ContentType
The name of a content type, its ID or an actual content type object that needs to be removed from the specified list.

```yaml
Type: ContentTypePipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -List
The name of the list, its ID or an actual list object from where the content type needs to be removed from

```yaml
Type: ListPipeBind
Parameter Sets: (All)

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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)