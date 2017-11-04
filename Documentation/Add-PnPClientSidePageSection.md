---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Add-PnPClientSidePageSection

## SYNOPSIS
Adds a new section to a Client-Side page

## SYNTAX 

```powershell
Add-PnPClientSidePageSection -SectionTemplate <CanvasSectionTemplate>
                             -Page <ClientSidePagePipeBind>
                             [-Order <Int>]
                             [-Web <WebPipeBind>]
                             [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPClientSidePageSection -Page "MyPage" -SectionTemplate OneColumn
```

Adds a new one-column section to the Client-Side page 'MyPage'

### ------------------EXAMPLE 2------------------
```powershell
PS:> Add-PnPClientSidePageSection -Page "MyPage" -SectionTemplate ThreeColumn -Order 10
```

Adds a new Three columns section to the Client-Side page 'MyPage' with an order index of 10

### ------------------EXAMPLE 3------------------
```powershell
PS:> $page = Add-PnPClientSidePage -Name "MyPage"
PS> Add-PnPClientSidePageSection -Page $page -SectionTemplate OneColumn
```

Adds a new one column section to the Client-Side page 'MyPage'

## PARAMETERS

### -Order
Sets the order of the section. (Default = 1)

```yaml
Type: Int
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Page
The name of the page

```yaml
Type: ClientSidePagePipeBind
Parameter Sets: (All)

Required: True
Position: 0
Accept pipeline input: True
```

### -SectionTemplate
Specifies the columns template to use for the section.

```yaml
Type: CanvasSectionTemplate
Parameter Sets: (All)

Required: True
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