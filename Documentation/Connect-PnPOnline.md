---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Connect-PnPOnline

## SYNOPSIS
Connect to a SharePoint site

## SYNTAX 

### Main
```powershell
Connect-PnPOnline -Url <String>
                  [-ReturnConnection [<SwitchParameter>]]
                  [-Credentials <CredentialPipeBind>]
                  [-CurrentCredentials [<SwitchParameter>]]
                  [-UseAdfs [<SwitchParameter>]]
                  [-MinimalHealthScore <Int>]
                  [-RetryCount <Int>]
                  [-RetryWait <Int>]
                  [-RequestTimeout <Int>]
                  [-AuthenticationMode <ClientAuthenticationMode>]
                  [-CreateDrive [<SwitchParameter>]]
                  [-DriveName <String>]
                  [-Scopes <String[]>]
                  [-TenantAdminUrl <String>]
                  [-SkipTenantAdminCheck [<SwitchParameter>]]
                  [-IgnoreSslErrors [<SwitchParameter>]]
```

### Microsoft Graph using Scopes
```powershell
Connect-PnPOnline -Scopes <String[]>
```

### WebLogin
```powershell
Connect-PnPOnline -UseWebLogin [<SwitchParameter>]
                  -Url <String>
                  [-ReturnConnection [<SwitchParameter>]]
                  [-MinimalHealthScore <Int>]
                  [-RetryCount <Int>]
                  [-RetryWait <Int>]
                  [-RequestTimeout <Int>]
                  [-CreateDrive [<SwitchParameter>]]
                  [-DriveName <String>]
                  [-Scopes <String[]>]
                  [-TenantAdminUrl <String>]
                  [-SkipTenantAdminCheck [<SwitchParameter>]]
                  [-IgnoreSslErrors [<SwitchParameter>]]
```

### SPO Management Shell Credentials
```powershell
Connect-PnPOnline -SPOManagementShell [<SwitchParameter>]
                  -Url <String>
                  [-ReturnConnection [<SwitchParameter>]]
                  [-MinimalHealthScore <Int>]
                  [-RetryCount <Int>]
                  [-RetryWait <Int>]
                  [-RequestTimeout <Int>]
                  [-CreateDrive [<SwitchParameter>]]
                  [-DriveName <String>]
                  [-ClearTokenCache [<SwitchParameter>]]
                  [-Scopes <String[]>]
                  [-TenantAdminUrl <String>]
                  [-SkipTenantAdminCheck [<SwitchParameter>]]
                  [-IgnoreSslErrors [<SwitchParameter>]]
```

### Token
```powershell
Connect-PnPOnline -AppId <String>
                  -AppSecret <String>
                  -Url <String>
                  [-ReturnConnection [<SwitchParameter>]]
                  [-MinimalHealthScore <Int>]
                  [-RetryCount <Int>]
                  [-RetryWait <Int>]
                  [-RequestTimeout <Int>]
                  [-Realm <String>]
                  [-CreateDrive [<SwitchParameter>]]
                  [-DriveName <String>]
                  [-Scopes <String[]>]
                  [-TenantAdminUrl <String>]
                  [-SkipTenantAdminCheck [<SwitchParameter>]]
                  [-IgnoreSslErrors [<SwitchParameter>]]
```

### Azure Active Directory
```powershell
Connect-PnPOnline -ClientId <String>
                  -RedirectUri <String>
                  -Url <String>
                  [-ReturnConnection [<SwitchParameter>]]
                  [-MinimalHealthScore <Int>]
                  [-RetryCount <Int>]
                  [-RetryWait <Int>]
                  [-RequestTimeout <Int>]
                  [-CreateDrive [<SwitchParameter>]]
                  [-DriveName <String>]
                  [-ClearTokenCache [<SwitchParameter>]]
                  [-AzureEnvironment <AzureEnvironment>]
                  [-Scopes <String[]>]
                  [-TenantAdminUrl <String>]
                  [-SkipTenantAdminCheck [<SwitchParameter>]]
                  [-IgnoreSslErrors [<SwitchParameter>]]
```

### Microsoft Graph using Azure Active Directory
```powershell
Connect-PnPOnline -AppId <String>
                  -AppSecret <String>
                  -AADDomain <String>
```

### App-Only with Azure Active Directory
```powershell
Connect-PnPOnline -ClientId <String>
                  -Tenant <String>
                  -CertificatePath <String>
                  -CertificatePassword <SecureString>
                  -AzureEnvironment <AzureEnvironment>
                  -Url <String>
                  [-ReturnConnection [<SwitchParameter>]]
                  [-MinimalHealthScore <Int>]
                  [-RetryCount <Int>]
                  [-RetryWait <Int>]
                  [-RequestTimeout <Int>]
                  [-CreateDrive [<SwitchParameter>]]
                  [-DriveName <String>]
                  [-Scopes <String[]>]
                  [-TenantAdminUrl <String>]
                  [-SkipTenantAdminCheck [<SwitchParameter>]]
                  [-IgnoreSslErrors [<SwitchParameter>]]
```

## DESCRIPTION
If no credentials have been specified, and the CurrentCredentials parameter has not been specified, you will be prompted for credentials.

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com
```

This will prompt for username and password and creates a context for the other PowerShell commands to use. When a generic credential is added to the Windows Credential Manager with https://contoso.sharepoint.com, PowerShell will not prompt for username and password.

### ------------------EXAMPLE 2------------------
```powershell
PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -Credentials (Get-Credential)
```

This will prompt for username and password and creates a context for the other PowerShell commands to use. 

### ------------------EXAMPLE 3------------------
```powershell
PS:> Connect-PnPOnline -Url http://yourlocalserver -CurrentCredentials
```

This will use the current user credentials and connects to the server specified by the Url parameter.

### ------------------EXAMPLE 4------------------
```powershell
PS:> Connect-PnPOnline -Url http://yourlocalserver -Credentials 'O365Creds'
```

This will use credentials from the Windows Credential Manager, as defined by the label 'O365Creds'.

### ------------------EXAMPLE 5------------------
```powershell
PS:> Connect-PnPOnline -Url http://yourlocalserver -Credentials (Get-Credential) -UseAdfs
```

This will prompt for username and password and creates a context using ADFS to authenticate.

### ------------------EXAMPLE 6------------------
```powershell
PS:> Connect-PnPOnline -Url https://yourserver -Credentials (Get-Credential) -CreateDrive
cd SPO:\\
dir
```

This will prompt you for credentials and creates a context for the other PowerShell commands to use. It will also create a SPO:\\ drive you can use to navigate around the site

### ------------------EXAMPLE 7------------------
```powershell
PS:> Connect-PnPOnline -Url https://yourserver -Credentials (Get-Credential) -AuthenticationMode FormsAuthentication
```

This will prompt you for credentials and creates a context for the other PowerShell commands to use. It assumes your server is configured for Forms Based Authentication (FBA)

### ------------------EXAMPLE 8------------------
```powershell
PS:> Connect-PnPOnline -Url https://contoso.sharepoint.de -AppId 344b8aab-389c-4e4a-8fa1-4c1ae2c0a60d -AppSecret a3f3faf33f3awf3a3sfs3f3ss3f4f4a3fawfas3ffsrrffssfd -AzureEnvironment Germany
```

This will authenticate you to the German Azure environment using the German Azure endpoints for authentication

### ------------------EXAMPLE 9------------------
```powershell
PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -SPOManagementShell
```

This will authenticate you using the SharePoint Online Management Shell application

## PARAMETERS

### -AADDomain
The AAD where the O365 app is registred. Eg.: contoso.com, or contoso.onmicrosoft.com.

```yaml
Type: String
Parameter Sets: Microsoft Graph using Azure Active Directory

Required: True
Position: Named
Accept pipeline input: False
```

### -AppId
The Application Client ID to use.

```yaml
Type: String
Parameter Sets: Token

Required: True
Position: Named
Accept pipeline input: False
```

### -AppSecret
The Application Client Secret to use.

```yaml
Type: String
Parameter Sets: Token

Required: True
Position: Named
Accept pipeline input: False
```

### -AuthenticationMode
Specify to use for instance use forms based authentication (FBA)

```yaml
Type: ClientAuthenticationMode
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: False
```

### -AzureEnvironment
The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.

```yaml
Type: AzureEnvironment
Parameter Sets: Azure Active Directory

Required: False
Position: Named
Accept pipeline input: False
```

### -CertificatePassword
Password to the certificate (*.pfx)

```yaml
Type: SecureString
Parameter Sets: App-Only with Azure Active Directory

Required: True
Position: Named
Accept pipeline input: False
```

### -CertificatePath
Path to the certificate (*.pfx)

```yaml
Type: String
Parameter Sets: App-Only with Azure Active Directory

Required: True
Position: Named
Accept pipeline input: False
```

### -ClearTokenCache
Clears the token cache.

```yaml
Type: SwitchParameter
Parameter Sets: Azure Active Directory

Required: False
Position: Named
Accept pipeline input: False
```

### -ClientId
The Client ID of the Azure AD Application

```yaml
Type: String
Parameter Sets: Azure Active Directory

Required: True
Position: Named
Accept pipeline input: False
```

### -CreateDrive
If you want to create a PSDrive connected to the URL

```yaml
Type: SwitchParameter
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: False
```

### -Credentials
Credentials of the user to connect with. Either specify a PSCredential object or a string. In case of a string value a lookup will be done to the Windows Credential Manager for the correct credentials.

```yaml
Type: CredentialPipeBind
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: False
```

### -CurrentCredentials
If you want to connect with the current user credentials

```yaml
Type: SwitchParameter
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: False
```

### -DriveName
Name of the PSDrive to create (default: SPO)

```yaml
Type: String
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: False
```

### -IgnoreSslErrors
Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.

```yaml
Type: SwitchParameter
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: False
```

### -MinimalHealthScore
Specifies a minimal server healthscore before any requests are executed.

```yaml
Type: Int
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: False
```

### -Realm
Authentication realm. If not specified will be resolved from the url specified.

```yaml
Type: String
Parameter Sets: Token

Required: False
Position: Named
Accept pipeline input: False
```

### -RedirectUri
The Redirect URI of the Azure AD Application

```yaml
Type: String
Parameter Sets: Azure Active Directory

Required: True
Position: Named
Accept pipeline input: False
```

### -RequestTimeout
The request timeout. Default is 180000

```yaml
Type: Int
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: False
```

### -RetryCount
Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.

```yaml
Type: Int
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: False
```

### -RetryWait
Defines how many seconds to wait before each retry. Default is 1 second.

```yaml
Type: Int
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: False
```

### -ReturnConnection
Returns the connection for use with the -Connection parameter on cmdlets.

```yaml
Type: SwitchParameter
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: True
```

### -Scopes
The array of permission scopes for the Microsoft Graph API.

```yaml
Type: String[]
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: False
```

### -SkipTenantAdminCheck
Should we skip the check if this site is the Tenant admin site. Default is false

```yaml
Type: SwitchParameter
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: False
```

### -SPOManagementShell
Log in using the SharePoint Online Management Shell application

```yaml
Type: SwitchParameter
Parameter Sets: SPO Management Shell Credentials

Required: True
Position: Named
Accept pipeline input: False
```

### -Tenant
The Azure AD Tenant name,e.g. mycompany.onmicrosoft.com

```yaml
Type: String
Parameter Sets: App-Only with Azure Active Directory

Required: True
Position: Named
Accept pipeline input: False
```

### -TenantAdminUrl
The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.

```yaml
Type: String
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: False
```

### -Url
The Url of the site collection to connect to.

```yaml
Type: String
Parameter Sets: Main

Required: True
Position: 0
Accept pipeline input: True
```

### -UseAdfs
If you want to connect to your on-premises SharePoint farm using ADFS

```yaml
Type: SwitchParameter
Parameter Sets: Main

Required: False
Position: Named
Accept pipeline input: False
```

### -UseWebLogin
If you want to connect to SharePoint with browser based login

```yaml
Type: SwitchParameter
Parameter Sets: WebLogin

Required: True
Position: Named
Accept pipeline input: False
```

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)