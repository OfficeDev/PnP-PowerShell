# Connect-PnPOnline
Connects to a SharePoint site and creates a context that is required for the other PnP Cmdlets
## Syntax
```powershell
Connect-PnPOnline -Url <String>
                  [-Credentials <CredentialPipeBind>]
                  [-CurrentCredentials [<SwitchParameter>]]
                  [-UseAdfs [<SwitchParameter>]]
                  [-AuthenticationMode <ClientAuthenticationMode>]
                  [-MinimalHealthScore <Int>]
                  [-RetryCount <Int>]
                  [-RetryWait <Int>]
                  [-RequestTimeout <Int>]
                  [-CreateDrive [<SwitchParameter>]]
                  [-DriveName <String>]
                  [-TenantAdminUrl <String>]
                  [-SkipTenantAdminCheck [<SwitchParameter>]]
```


```powershell
Connect-PnPOnline -UseWebLogin [<SwitchParameter>]
                  -Url <String>
                  [-MinimalHealthScore <Int>]
                  [-RetryCount <Int>]
                  [-RetryWait <Int>]
                  [-RequestTimeout <Int>]
                  [-CreateDrive [<SwitchParameter>]]
                  [-DriveName <String>]
                  [-TenantAdminUrl <String>]
                  [-SkipTenantAdminCheck [<SwitchParameter>]]
```


```powershell
Connect-PnPOnline -AppId <String>
                  -AppSecret <String>
                  -Url <String>
                  [-Realm <String>]
                  [-MinimalHealthScore <Int>]
                  [-RetryCount <Int>]
                  [-RetryWait <Int>]
                  [-RequestTimeout <Int>]
                  [-CreateDrive [<SwitchParameter>]]
                  [-DriveName <String>]
                  [-TenantAdminUrl <String>]
                  [-SkipTenantAdminCheck [<SwitchParameter>]]
```


```powershell
Connect-PnPOnline -ClientId <String>
                  -RedirectUri <String>
                  -Url <String>
                  [-ClearTokenCache [<SwitchParameter>]]
                  [-AzureEnvironment <AzureEnvironment>]
                  [-MinimalHealthScore <Int>]
                  [-RetryCount <Int>]
                  [-RetryWait <Int>]
                  [-RequestTimeout <Int>]
                  [-CreateDrive [<SwitchParameter>]]
                  [-DriveName <String>]
                  [-TenantAdminUrl <String>]
                  [-SkipTenantAdminCheck [<SwitchParameter>]]
```


```powershell
Connect-PnPOnline -ClientId <String>
                  -Tenant <String>
                  -CertificatePath <String>
                  -CertificatePassword <SecureString>
                  -AzureEnvironment <AzureEnvironment>
                  -Url <String>
                  [-MinimalHealthScore <Int>]
                  [-RetryCount <Int>]
                  [-RetryWait <Int>]
                  [-RequestTimeout <Int>]
                  [-CreateDrive [<SwitchParameter>]]
                  [-DriveName <String>]
                  [-TenantAdminUrl <String>]
                  [-SkipTenantAdminCheck [<SwitchParameter>]]
```


## Detailed Description
If no credentials have been specified, and the CurrentCredentials parameter has not been specified, you will be prompted for credentials.

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|AppId|String|True|The Application Client ID to use.|
|AppSecret|String|True|The Application Client Secret to use.|
|CertificatePassword|SecureString|True|Password to the certificate (*.pfx)|
|CertificatePath|String|True|Path to the certificate (*.pfx)|
|ClientId|String|True|The Client ID of the Azure AD Application|
|RedirectUri|String|True|The Redirect URI of the Azure AD Application|
|Tenant|String|True|The Azure AD Tenant name,e.g. mycompany.onmicrosoft.com|
|Url|String|True|The Url of the site collection to connect to.|
|UseWebLogin|SwitchParameter|True|If you want to connect to SharePoint with browser based login|
|AuthenticationMode|ClientAuthenticationMode|False|Specify to use for instance use forms based authentication (FBA)|
|AzureEnvironment|AzureEnvironment|False|The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.|
|ClearTokenCache|SwitchParameter|False|Clears the token cache.|
|CreateDrive|SwitchParameter|False|If you want to create a PSDrive connected to the URL|
|Credentials|CredentialPipeBind|False|Credentials of the user to connect with. Either specify a PSCredential object or a string. In case of a string value a lookup will be done to the Windows Credential Manager for the correct credentials.|
|CurrentCredentials|SwitchParameter|False|If you want to connect with the current user credentials|
|DriveName|String|False|Name of the PSDrive to create (default: SPO)|
|MinimalHealthScore|Int|False|Specifies a minimal server healthscore before any requests are executed.|
|Realm|String|False|Authentication realm. If not specified will be resolved from the url specified.|
|RequestTimeout|Int|False|The request timeout. Default is 180000|
|RetryCount|Int|False|Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.|
|RetryWait|Int|False|Defines how many seconds to wait before each retry. Default is 1 second.|
|SkipTenantAdminCheck|SwitchParameter|False|Should we skip the check if this site is the Tenant admin site. Default is false|
|TenantAdminUrl|String|False|The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.|
|UseAdfs|SwitchParameter|False|If you want to connect to your on-premises SharePoint farm using ADFS|
## Examples

### Example 1
```powershell
PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com
```
This will prompt for username and password and creates a context for the other PowerShell commands to use. When a generic credential is added to the Windows Credential Manager with https://contoso.sharepoint.com, PowerShell will not prompt for username and password.

### Example 2
```powershell
PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -Credentials (Get-Credential)
```
This will prompt for username and password and creates a context for the other PowerShell commands to use. 

### Example 3
```powershell
PS:> Connect-PnPOnline -Url http://yourlocalserver -CurrentCredentials
```
This will use the current user credentials and connects to the server specified by the Url parameter.

### Example 4
```powershell
PS:> Connect-PnPOnline -Url http://yourlocalserver -Credentials 'O365Creds'
```
This will use credentials from the Windows Credential Manager, as defined by the label 'O365Creds'.

### Example 5
```powershell
PS:> Connect-PnPOnline -Url http://yourlocalserver -Credentials (Get-Credential) -UseAdfs
```
This will prompt for username and password and creates a context using ADFS to authenticate.

### Example 6
```powershell
PS:> Connect-PnPOnline -Url https://yourserver -Credentials (Get-Credential) -CreateDrive
cd SPO:\\
dir
```
This will prompt you for credentials and creates a context for the other PowerShell commands to use. It will also create a SPO:\\ drive you can use to navigate around the site

### Example 7
```powershell
PS:> Connect-PnPOnline -Url https://yourserver -Credentials (Get-Credential) -AuthenticationMode FormsAuthentication
```
This will prompt you for credentials and creates a context for the other PowerShell commands to use. It assumes your server is configured for Forms Based Authentication (FBA)

### Example 8
```powershell
PS:> Connect-PnPOnline -Url https://contoso.sharepoint.de -AppId 344b8aab-389c-4e4a-8fa1-4c1ae2c0a60d -ClientSecret a3f3faf33f3awf3a3sfs3f3ss3f4f4a3fawfas3ffsrrffssfd -AzureEnvironment Germany
```
This will authenticate you to the German Azure environment using the German Azure endpoints for authentication
