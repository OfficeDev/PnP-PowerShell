#Send-PnPMail
Sends an email using the Office 365 SMTP Service or SharePoint, depending on the parameters specified. See detailed help for more information.
##Syntax
```powershell
Send-PnPMail [-Server <String>]
             [-From <String>]
             [-Password <String>]
             -To <String[]>
             [-Cc <String[]>]
             -Subject <String>
             -Body <String>
             [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Body|String|True|Body of the email|
|Cc|String[]|False|List of recipients on CC|
|From|String|False|If using from address, you also have to provide a password|
|Password|String|False|If using a password, you also have to provide the associated from address|
|Server|String|False||
|Subject|String|True|Subject of the email|
|To|String[]|True|List of recipients|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Send-PnPMail -To address@tenant.sharepointonline.com -Subject test -Body test
```
Sends an e-mail using the SharePoint SendEmail method using the current context. E-mail is sent from the system account and can only be sent to accounts in the same tenant

###Example 2
```powershell
PS:> Send-PnPMail -To address@contoso.com -Subject test -Body test -From me@tenant.onmicrosoft.com -Password xyz
```
Sends an e-mail via Office 365 SMTP and requires a from address and password. E-mail is sent from the from user and can be sent to both internal and external addresses.

###Example 3
```powershell
PS:> Send-PnPMail -To address@contoso.com -Subject test -Body test -From me@server.net -Password xyz -Server yoursmtp.server.net
```
Sends an e-mail via a custom SMTP server and requires a from address and password. E-mail is sent from the from user.
