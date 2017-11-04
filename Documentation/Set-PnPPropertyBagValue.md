---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Set-PnPPropertyBagValue

## SYNOPSIS
Sets a property bag value

## SYNTAX 

### Folder
```powershell
Set-PnPPropertyBagValue -Key <String>
                        -Value <String>
                        [-Folder <String>]
                        [-Web <WebPipeBind>]
                        [-Connection <SPOnlineConnection>]
```

### Web
```powershell
Set-PnPPropertyBagValue -Key <String>
                        -Value <String>
                        -Indexed [<SwitchParameter>]
                        [-Web <WebPipeBind>]
                        [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPPropertyBagValue -Key MyKey -Value MyValue
```

This sets or adds a value to the current web property bag

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPPropertyBagValue -Key MyKey -Value MyValue -Folder /
```

This sets or adds a value to the root folder of the current web

### ------------------EXAMPLE 3------------------
```powershell
PS:> Set-PnPPropertyBagValue -Key MyKey -Value MyValue -Folder /MyFolder
```

This sets or adds a value to the folder MyFolder which is located in the root folder of the current web

## PARAMETERS

### -Folder
Site relative url of the folder. See examples for use.

```yaml
Type: String
Parameter Sets: Folder

Required: False
Position: Named
Accept pipeline input: False
```

### -Indexed


```yaml
Type: SwitchParameter
Parameter Sets: Web

Required: True
Position: Named
Accept pipeline input: False
```

### -Key


```yaml
Type: String
Parameter Sets: Web

Required: True
Position: Named
Accept pipeline input: False
```

### -Value


```yaml
Type: String
Parameter Sets: Web

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