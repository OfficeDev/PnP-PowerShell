---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPSubWebs

## SYNOPSIS
Returns the subwebs of the current web

## SYNTAX 

```powershell
Get-PnPSubWebs [-Recurse [<SwitchParameter>]]
               [-Identity <WebPipeBind>]
               [-Web <WebPipeBind>]
               [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPSubWebs
```

This will return all sub webs for the current web

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPSubWebs -recurse
```

This will return all sub webs for the current web and its sub webs

## PARAMETERS

### -Identity
The guid of the web or web object

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: 0
Accept pipeline input: True
```

### -Recurse
include subweb of the subwebs

```yaml
Type: SwitchParameter
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

### [List<Microsoft.SharePoint.Client.Web>](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)