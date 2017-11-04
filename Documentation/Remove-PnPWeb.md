---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPWeb

## SYNOPSIS
Removes a subweb in the current web

## SYNTAX 

### ByUrl
```powershell
Remove-PnPWeb -Url <String>
              [-Force [<SwitchParameter>]]
              [-Web <WebPipeBind>]
              [-Connection <SPOnlineConnection>]
```

### ByIdentity
```powershell
Remove-PnPWeb -Identity <WebPipeBind>
              [-Force [<SwitchParameter>]]
              [-Web <WebPipeBind>]
              [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPWeb -Url projectA
```

Remove a web

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPWeb -Identity 5fecaf67-6b9e-4691-a0ff-518fc9839aa0
```

Remove a web specified by its ID

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPSubWebs | Remove-PnPWeb -Force
```

Remove all subwebs and do not ask for confirmation

## PARAMETERS

### -Force
Do not ask for confirmation to delete the subweb

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
Identity/Id/Web object to delete

```yaml
Type: WebPipeBind
Parameter Sets: ByIdentity

Required: True
Position: Named
Accept pipeline input: True
```

### -Url
The site relative url of the web, e.g. 'Subweb1'

```yaml
Type: String
Parameter Sets: ByUrl

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