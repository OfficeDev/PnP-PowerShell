---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPPropertyBag

## SYNOPSIS
Returns the property bag values.

## SYNTAX 

```powershell
Get-PnPPropertyBag [-Folder <String>]
                   [-Web <WebPipeBind>]
                   [-Key <String>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPPropertyBag
```

This will return all web property bag values

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPPropertyBag -Key MyKey
```

This will return the value of the key MyKey from the web property bag

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPPropertyBag -Folder /MyFolder
```

This will return all property bag values for the folder MyFolder which is located in the root of the current web

### ------------------EXAMPLE 4------------------
```powershell
PS:> Get-PnPPropertyBag -Folder /MyFolder -Key vti_mykey
```

This will return the value of the key vti_mykey from the folder MyFolder which is located in the root of the current web

### ------------------EXAMPLE 5------------------
```powershell
PS:> Get-PnPPropertyBag -Folder / -Key vti_mykey
```

This will return the value of the key vti_mykey from the root folder of the current web

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

### -Key
Key that should be looked up

```yaml
Type: String
Parameter Sets: (All)

Required: False
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

## OUTPUTS

### SharePointPnP.PowerShell.Commands.PropertyBagValue

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)