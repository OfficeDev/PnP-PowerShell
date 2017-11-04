---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPContentType

## SYNOPSIS
Retrieves a content type

## SYNTAX 

```powershell
Get-PnPContentType [-List <ListPipeBind>]
                   [-InSiteHierarchy [<SwitchParameter>]]
                   [-Identity <ContentTypePipeBind>]
                   [-Web <WebPipeBind>]
                   [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPContentType 
```

This will get a listing of all available content types within the current web

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPContentType -InSiteHierarchy
```

This will get a listing of all available content types within the site collection

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPContentType -Identity "Project Document"
```

This will get the content type with the name "Project Document" within the current context

### ------------------EXAMPLE 4------------------
```powershell
PS:> Get-PnPContentType -List "Documents"
```

This will get a listing of all available content types within the list "Documents"

## PARAMETERS

### -Identity
Name or ID of the content type to retrieve

```yaml
Type: ContentTypePipeBind
Parameter Sets: (All)

Required: False
Position: 0
Accept pipeline input: True
```

### -InSiteHierarchy
Search site hierarchy for content types

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -List
List to query

```yaml
Type: ListPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: True
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

### [Microsoft.SharePoint.Client.ContentType](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.contenttype.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)