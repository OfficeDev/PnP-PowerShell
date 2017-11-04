---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Send-PnPMail

## SYNOPSIS
Sends an email using the Office 365 SMTP Service or SharePoint, depending on the parameters specified. See detailed help for more information.

## SYNTAX 

```powershell
Send-PnPMail -To <String[]>
             -Subject <String>
             -Body <String>
             [-Server <String>]
             [-From <String>]
             [-Password <String>]
             [-Cc <String[]>]
             [-Web <WebPipeBind>]
             [-Connection <SPOnlineConnection>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Send-PnPMail -To address@tenant.sharepointonline.com -Subject test -Body test
```

Sends an e-mail using the SharePoint SendEmail method using the current context. E-mail is sent from the system account and can only be sent to accounts in the same tenant

### ------------------EXAMPLE 2------------------
```powershell
PS:> Send-PnPMail -To address@contoso.com -Subject test -Body test -From me@tenant.onmicrosoft.com -Password xyz
```

Sends an e-mail via Office 365 SMTP and requires a from address and password. E-mail is sent from the from user and can be sent to both internal and external addresses.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Send-PnPMail -To address@contoso.com -Subject test -Body test -From me@server.net -Password xyz -Server yoursmtp.server.net
```

Sends an e-mail via a custom SMTP server and requires a from address and password. E-mail is sent from the from user.

## PARAMETERS

### -Body
Body of the email

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Cc
List of recipients on CC

```yaml
Type: String[]
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -From
If using from address, you also have to provide a password

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Password
If using a password, you also have to provide the associated from address

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Server


```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Subject
Subject of the email

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -To
List of recipients

```yaml
Type: String[]
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