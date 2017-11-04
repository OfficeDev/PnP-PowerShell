---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPPublishingImageRendition

## SYNOPSIS
Returns all image renditions or if Identity is specified a specific one

## SYNTAX 

```powershell
Get-PnPPublishingImageRendition [-Identity <ImageRenditionPipeBind>]
                                [-Web <WebPipeBind>]
                                [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPPublishingImageRendition
```

Returns all Image Renditions

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPPublishingImageRendition -Identity "Test"
```

Returns the image rendition named "Test"

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPPublishingImageRendition -Identity 2
```

Returns the image rendition where its id equals 2

## PARAMETERS

### -Identity
Id or name of an existing image rendition

```yaml
Type: ImageRenditionPipeBind
Parameter Sets: (All)

Required: False
Position: 0
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

### [Microsoft.SharePoint.Client.Publishing.ImageRendition](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.publishing.imagerendition.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)