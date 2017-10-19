---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Get-PnPCustomAction

## SYNOPSIS
Return user custom actions

## SYNTAX 

### 
```powershell
Get-PnPCustomAction [-Identity <GuidPipeBind>]
                    [-Scope <CustomActionScope>]
                    [-ThrowExceptionIfCustomActionNotFound [<SwitchParameter>]]
                    [-Web <WebPipeBind>]
                    [-Includes <String[]>]
```

## DESCRIPTION
Returns all or a specific user custom action

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPCustomAction
```

Returns all custom actions of the current site.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPCustomAction -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2
```

Returns the custom action with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2'.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Get-PnPCustomAction -Scope web
```

Returns all custom actions for the current web object.

## PARAMETERS

### -Identity
Identity of the CustomAction to return. Omit to return all CustomActions.

```yaml
Type: GuidPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Includes
Specify properties to include when retrieving objects from the server.

```yaml
Type: String[]
Parameter Sets: 

Required: False
Position: 0
Accept pipeline input: False
```

### -Scope
Scope of the CustomAction, either Web, Site or All to return both

```yaml
Type: CustomActionScope
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ThrowExceptionIfCustomActionNotFound
Switch parameter if an exception should be thrown if the requested CustomAction does not exist (true) or if omitted, nothing will be returned in case the CustomAction does not exist

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Web
The web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

## OUTPUTS

### [List<Microsoft.SharePoint.Client.UserCustomAction>](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.usercustomaction.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)