#Set-PnPRequestAccessEmails
Sets Request Access Emails on a web
##Syntax
```powershell
Set-PnPRequestAccessEmails -Emails <String[]>
                           [-Web <WebPipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Emails|String[]|True|Email address(es) to set the RequestAccessEmails to|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Set-PnPRequestAccessEmails -Emails someone@example.com 
```
This will update the request access e-mail address

###Example 2
```powershell
PS:> Set-PnPRequestAccessEmails -Emails @( someone@example.com; someoneelse@example.com )
```
This will update multiple request access e-mail addresses
