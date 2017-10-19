---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Remove-PnPJavaScriptLink

## SYNOPSIS
Removes a JavaScript link or block from a web or sitecollection

## SYNTAX 

```powershell
Remove-PnPJavaScriptLink [-Force [<SwitchParameter>]]
                         [-Scope <CustomActionScope>]
                         [-Web <WebPipeBind>]
                         [-Name <String>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPJavaScriptLink -Name jQuery
```

Removes the injected JavaScript file with the name jQuery from the current web after confirmation

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPJavaScriptLink -Name jQuery -Scope Site
```

Removes the injected JavaScript file with the name jQuery from the current site collection after confirmation

### ------------------EXAMPLE 3------------------
```powershell
PS:> Remove-PnPJavaScriptLink -Name jQuery -Scope Site -Force
```

Removes the injected JavaScript file with the name jQuery from the current site collection and will not ask for confirmation

### ------------------EXAMPLE 4------------------
```powershell
PS:> Remove-PnPJavaScriptLink -Scope Site
```

Removes all the injected JavaScript files with from the current site collection after confirmation for each of them

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

### -Name
Name of the JavaScriptLink to remove. Omit if you want to remove all JavaScript Links.

```yaml
Type: String
Parameter Sets: (All)
Aliases: Key

Required: False
Position: 0
Accept pipeline input: True
```

### -Scope
Define if the JavaScriptLink is to be found at the web or site collection scope. Specify All to allow deletion from either web or site collection.

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