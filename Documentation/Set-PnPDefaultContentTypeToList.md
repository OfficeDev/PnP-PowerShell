---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPDefaultContentTypeToList

## SYNOPSIS
Sets the default content type for a list

## SYNTAX 

```powershell
Set-PnPDefaultContentTypeToList -List <ListPipeBind>
                                -ContentType <ContentTypePipeBind>
                                [-Web <WebPipeBind>]
                                [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPDefaultContentTypeToList -List "Project Documents" -ContentType "Project"
```

This will set the Project content type (which has already been added to a list) as the default content type

## PARAMETERS

### -ContentType
The content type object that needs to be added to the list

```yaml
Type: ContentTypePipeBind
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -List
The name of a content type, its ID or an actual content type object that needs to be removed from the specified list.

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