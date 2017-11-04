---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Unpublish-PnPApp

## SYNOPSIS
Unpublishes/retracts an available add-in from the app catalog

## SYNTAX 

```powershell
Unpublish-PnPApp -Identity <AppMetadataPipeBind>
                 [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Unpublish-PnPApp -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe
```

This will retract, but not remove, the specified app from the app catalog

## PARAMETERS

### -Identity
Specifies the Id of the Addin Instance

```yaml
Type: AppMetadataPipeBind
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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)