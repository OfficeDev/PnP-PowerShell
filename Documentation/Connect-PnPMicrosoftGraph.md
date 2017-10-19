---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Connect-PnPMicrosoftGraph

## SYNOPSIS
Connect to the Microsoft Graph

## SYNTAX 

### Scope
```powershell
Connect-PnPMicrosoftGraph -Scopes <String[]>
```

### AAD
```powershell
Connect-PnPMicrosoftGraph -AppId <String>
                          -AppSecret <String>
                          -AADDomain <String>
```

## DESCRIPTION
Uses the Microsoft Authentication Library (Preview) to connect to Azure AD and to get an OAuth 2.0 Access Token to consume the Microsoft Graph API

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Connect-PnPMicrosoftGraph -Scopes $arrayOfScopes
```

Connects to Azure AD and gets and OAuth 2.0 Access Token to consume the Microsoft Graph API including the declared permission scopes. The available permission scopes are defined at the following URL: https://graph.microsoft.io/en-us/docs/authorization/permission_scopes

### ------------------EXAMPLE 2------------------
```powershell
PS:> Connect-PnPMicrosoftGraph -AppId '<id>' -AppSecret '<secrect>' -AADDomain 'contoso.onmicrosoft.com'
```

Connects to the Microsoft Graph API using application permissions via an app's declared permission scopes. See https://github.com/SharePoint/PnP-PowerShell/tree/master/Samples/Graph.ConnectUsingAppPermissions for a sample on how to get started.

## PARAMETERS

### -AADDomain
The AAD where the O365 app is registred. Eg.: contoso.com, or contoso.onmicrosoft.com.

```yaml
Type: String
Parameter Sets: AAD

Required: True
Position: Named
Accept pipeline input: False
```

### -AppId
The client id of the app which gives you access to the Microsoft Graph API.

```yaml
Type: String
Parameter Sets: AAD

Required: True
Position: Named
Accept pipeline input: False
```

### -AppSecret
The app key of the app which gives you access to the Microsoft Graph API.

```yaml
Type: String
Parameter Sets: AAD

Required: True
Position: Named
Accept pipeline input: False
```

### -Scopes
The array of permission scopes for the Microsoft Graph API.

```yaml
Type: String[]
Parameter Sets: Scope

Required: True
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)