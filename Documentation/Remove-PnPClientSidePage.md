---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Remove-PnPClientSidePage

## SYNOPSIS
Removes a Client-Side Page

## SYNTAX 

```powershell
Remove-PnPClientSidePage -Identity <ClientSidePagePipeBind>
                         [-Force [<SwitchParameter>]]
                         [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Remove-PnPClientSidePage -Identity "MyPage"
```

Removes the Client-Side page named 'MyPage.aspx'

### ------------------EXAMPLE 2------------------
```powershell
PS:> Remove-PnPClientSidePage $page
```

Removes the specified Client-Side page which is contained in the $page variable.

## PARAMETERS

### -Force
Specifying the Force parameter will skip the confirmation question.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
The name of the page

```yaml
Type: ClientSidePagePipeBind
Parameter Sets: (All)

Required: True
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

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)