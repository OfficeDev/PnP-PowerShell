---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPDocumentSetTemplate

## SYNOPSIS
Retrieves a document set template

## SYNTAX 

### 
```powershell
Get-PnPDocumentSetTemplate -Identity <DocumentSetPipeBind>
                           [-Web <WebPipeBind>]
                           [-Includes <String[]>]
                           [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPDocumentSetTemplate -Identity "Test Document Set"
```

This will get the document set template with the name "Test Document Set"

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPDocumentSetTemplate -Identity "0x0120D520005DB65D094035A241BAC9AF083F825F3B"
```

This will get the document set template with the id "0x0120D520005DB65D094035A241BAC9AF083F825F3B"

## PARAMETERS

### -Identity
Either specify a name, an id, a document set template object or a content type object

```yaml
Type: DocumentSetPipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -Includes
Specify properties to include when retrieving objects from the server.

```yaml
Type: String[]
Parameter Sets: 

Required: False
Position: 0
Accept pipeline input: False
```

### -Web
The web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
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

## OUTPUTS

### [Microsoft.SharePoint.Client.DocumentSet.DocumentSetTemplate](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.documentset.documentsettemplate.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)