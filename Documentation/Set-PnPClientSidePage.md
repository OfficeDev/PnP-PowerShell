---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Set-PnPClientSidePage

## SYNOPSIS
Sets parameters of a Client-Side Page

## SYNTAX 

```powershell
Set-PnPClientSidePage -Identity <ClientSidePagePipeBind>
                      [-Name <String>]
                      [-LayoutType <ClientSidePageLayoutType>]
                      [-PromoteAs <ClientSidePagePromoteType>]
                      [-CommentsEnabled [<SwitchParameter>]]
                      [-Publish [<SwitchParameter>]]
                      [-PublishMessage <String>]
                      [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Set-PnPClientSidePage -Identity "MyPage" -LayoutType Home
```

Updates the properties of the Client-Side page named 'MyPage'

### ------------------EXAMPLE 2------------------
```powershell
PS:> Set-PnPClientSidePage -Identity "MyPage" -CommentsEnabled
```

Enables the comments on the Client-Side page named 'MyPage'

### ------------------EXAMPLE 3------------------
```powershell
PS:> Set-PnPClientSidePage -Identity "MyPage" -CommentsEnabled $false
```

Disables the comments on the Client-Side page named 'MyPage'

## PARAMETERS

### -CommentsEnabled
Enables or Disables the comments on the page

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Identity
The name/identity of the page

```yaml
Type: ClientSidePagePipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -LayoutType
Sets the layout type of the page. (Default = Article)

```yaml
Type: ClientSidePageLayoutType
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Name
Sets the name of the page.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -PromoteAs
Allows to promote the page for a specific purpose (HomePage | NewsPage)

```yaml
Type: ClientSidePagePromoteType
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Publish
Publishes the page once it is saved.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -PublishMessage
Sets the message for publishing the page.

```yaml
Type: String
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