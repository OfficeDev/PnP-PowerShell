---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# New-PnPExtensbilityHandlerObject

## SYNOPSIS
Creates an ExtensibilityHandler Object, to be used by the Get-SPOProvisioningTemplate cmdlet

## SYNTAX 

```powershell
New-PnPExtensbilityHandlerObject -Type <String>
                                 -Assembly <String>
                                 [-Configuration <String>]
                                 [-Disabled [<SwitchParameter>]]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell

PS:> $handler = New-PnPExtensbilityHandlerObject -Assembly Contoso.Core.Handlers -Type Contoso.Core.Handlers.MyExtensibilityHandler
PS:> Get-PnPProvisioningTemplate -Out NewTemplate.xml -ExtensibilityHandlers $handler
```

This will create a new ExtensibilityHandler object that is run during extraction of the template

## PARAMETERS

### -Assembly
The full assembly name of the handler

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -Configuration
Any configuration data you want to send to the handler

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Disabled
If set, the handler will be disabled

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Type
The type of the handler

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

## OUTPUTS

### OfficeDevPnP.Core.Framework.Provisioning.Model.ExtensibilityHandler

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)