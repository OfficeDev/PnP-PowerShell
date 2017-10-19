---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPPropertyBagValue

## SYNOPSIS
Removes a value from the property bag

## SYNTAX 

```powershell
Remove-PnPPropertyBagValue -Key <String>
                           [-Folder <String>]
                           [-Force [<SwitchParameter>]]
                           [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPPropertyBagValue -Key MyKey
```

This will remove the value with key MyKey from the current web property bag

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPPropertyBagValue -Key MyKey -Folder /MyFolder
```

This will remove the value with key MyKey from the folder MyFolder which is located in the root folder of the current web

### ------------------EXAMPLE 3------------------
```powershell
PS:> Remove-PnPPropertyBagValue -Key MyKey -Folder /
```

This will remove the value with key MyKey from the root folder of the current web

## PARAMETERS

### -Folder
Site relative url of the folder. See examples for use.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Force


```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Key
Key of the property bag value to be removed

```yaml
Type: String
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