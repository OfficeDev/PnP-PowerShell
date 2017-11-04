---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Enable-PnPFeature

## SYNOPSIS
Enables a feature

## SYNTAX 

```powershell
Enable-PnPFeature -Identity <GuidPipeBind>
                  [-Force [<SwitchParameter>]]
                  [-Scope <FeatureScope>]
                  [-Sandboxed [<SwitchParameter>]]
                  [-Web <WebPipeBind>]
                  [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Enable-PnPFeature -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe
```

This will enable the feature with the id "99a00f6e-fb81-4dc7-8eac-e09c6f9132fe"

### ------------------EXAMPLE 2------------------
```powershell
PS:> Enable-PnPFeature -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -Force
```

This will enable the feature with the id "99a00f6e-fb81-4dc7-8eac-e09c6f9132fe" with force.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Enable-PnPFeature -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -Scope Web
```

This will enable the feature with the id "99a00f6e-fb81-4dc7-8eac-e09c6f9132fe" with the web scope.

## PARAMETERS

### -Force
Forcibly enable the feature.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
The id of the feature to enable.

```yaml
Type: GuidPipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -Sandboxed
Specify this parameter if the feature you're trying to activate is part of a sandboxed solution.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Scope
Specify the scope of the feature to activate, either Web or Site. Defaults to Web.

```yaml
Type: FeatureScope
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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)