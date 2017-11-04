---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPPublishingImageRendition

## SYNOPSIS
Removes an existing image rendition

## SYNTAX 

```powershell
Remove-PnPPublishingImageRendition -Identity <ImageRenditionPipeBind>
                                   [-Force [<SwitchParameter>]]
                                   [-Web <WebPipeBind>]
                                   [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPPublishingImageRendition -Name "MyImageRendition" -Width 800 -Height 600
```



## PARAMETERS

### -Force
If provided, no confirmation will be asked to remove the Image Rendition.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
The display name or id of the Image Rendition.

```yaml
Type: ImageRenditionPipeBind
Parameter Sets: (All)

Required: True
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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)