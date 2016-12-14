#Connect-PnPMicrosoftGraph
Uses the Microsoft Authentication Library (Preview) to connect to Azure AD and to get an OAuth 2.0 Access Token to consume the Microsoft Graph API
##Syntax
```powershell
Connect-PnPMicrosoftGraph -Scopes <String[]>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Scopes|String[]|True|The array of permission scopes for the Microsoft Graph API.|
##Examples

###Example 1
```powershell
PS:> Connect-PnPMicrosoftGraph -Scopes $arrayOfScopes
```
Connects to Azure AD and gets and OAuth 2.0 Access Token to consume the Microsoft Graph API including the declared permission scopes. The available permission scopes are defined at the following URL: https://graph.microsoft.io/en-us/docs/authorization/permission_scopes
