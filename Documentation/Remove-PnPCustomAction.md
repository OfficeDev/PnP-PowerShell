---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPCustomAction

## SYNOPSIS
Removes a custom action

## SYNTAX 

```powershell
Remove-PnPCustomAction -Identity <GuidPipeBind>
                       [-Scope <CustomActionScope>]
                       [-Force [<SwitchParameter>]]
                       [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPCustomAction -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2
```

Removes the custom action with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2'.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPCustomAction -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2 -scope web
```

Removes the custom action with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2' from the current web.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Remove-PnPCustomAction -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2 -force
```

Removes the custom action with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2' without asking for confirmation.

## PARAMETERS

### -Force
Use the -Force flag to bypass the confirmation question

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
The identifier of the CustomAction that needs to be removed

```yaml
Type: GuidPipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -Scope
Define if the CustomAction is to be found at the web or site collection scope. Specify All to allow deletion from either web or site collection.

```yaml
Type: CustomActionScope
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