#Get-PnPAuthenticationRealm
Gets the authentication realm for the current web
##Syntax
```powershell
Get-PnPAuthenticationRealm [-Url <String>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Url|String|False|Specifies the URL of the site|
##Examples

###Example 1
```powershell
PS:> Get-PnPAuthenticationRealm
```
This will get the authentication realm for the current connected site

###Example 2
```powershell
PS:> Get-PnPAuthenticationRealm -Url https://contoso.sharepoint.com
```
This will get the authentication realm for https://contoso.sharepoint.com
