---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Add-PnPClientSideText

## SYNOPSIS
Adds a text element to a client-side page.

## SYNTAX 

### Default
```powershell
Add-PnPClientSideText -Text <String>
                      -Page <ClientSidePagePipeBind>
                      [-Order <Int>]
                      [-Web <WebPipeBind>]
                      [-Connection <SPOnlineConnection>]
```

### Positioned
```powershell
Add-PnPClientSideText -Text <String>
                      -Section <Int>
                      -Column <Int>
                      -Page <ClientSidePagePipeBind>
                      [-Order <Int>]
                      [-Web <WebPipeBind>]
                      [-Connection <SPOnlineConnection>]
```

## DESCRIPTION
Adds a new text element to a section on a client-side page.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPClientSideText -Page "MyPage" -Text "Hello World!"
```

Adds the text 'Hello World!' to the Client-Side Page 'MyPage'

## PARAMETERS

### -Column
Sets the column where to insert the text control.

```yaml
Type: Int
Parameter Sets: Positioned

Required: True
Position: Named
Accept pipeline input: False
```

### -Order
Sets the order of the text control. (Default = 1)

```yaml
Type: Int
Parameter Sets: Default

Required: False
Position: Named
Accept pipeline input: False
```

### -Page
The name of the page.

```yaml
Type: ClientSidePagePipeBind
Parameter Sets: Default

Required: True
Position: 0
Accept pipeline input: True
```

### -Section
Sets the section where to insert the text control.

```yaml
Type: Int
Parameter Sets: Positioned

Required: True
Position: Named
Accept pipeline input: False
```

### -Text
Specifies the text to display in the text area.

```yaml
Type: String
Parameter Sets: Default

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