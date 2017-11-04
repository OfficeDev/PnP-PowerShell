---
external help file:
applicable: SharePoint Online
schema: 2.0.0
---
# Get-PnPClientSidePage

## SYNOPSIS
Gets a Client-Side Page

## SYNTAX 

```powershell
Get-PnPClientSidePage -Identity <ClientSidePagePipeBind>
                      [-Web <WebPipeBind>]
                      [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Get-PnPClientSidePage -Identity "MyPage.aspx"
```

Gets the Modern Page (Client-Side) named 'MyPage.aspx' in the current SharePoint site

### ------------------EXAMPLE 2------------------
```powershell
PS:> Get-PnPClientSidePage "MyPage"
```

Gets the Modern Page (Client-Side) named 'MyPage.aspx' in the current SharePoint site

## PARAMETERS

### -Identity
The name of the page

```yaml
Type: ClientSidePagePipeBind
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