---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Get-PnPApp

## SYNOPSIS
Returns the available apps from the app catalog

## SYNTAX 

```powershell
Get-PnPApp [-Identity <GuidPipeBind>]
           [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPAvailableApp
```

This will return all available app metadata from the tenant app catalog. It will list the installed version in the current site.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPAvailableApp -Identity 2646ccc3-6a2b-46ef-9273-81411cbbb60f
```

This will the specific app metadata from the app catalog.

## PARAMETERS

### -Identity
Specifies the Id of an app which is available in the app catalog

```yaml
Type: GuidPipeBind
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

## OUTPUTS

### List<OfficeDevPnP.Core.ALM.AppMetadata>

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)