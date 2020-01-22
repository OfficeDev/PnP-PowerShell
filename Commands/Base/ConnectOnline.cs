using Microsoft.Identity.Client;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Provider;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Reflection;
using System.Security;
using File = System.IO.File;
using System.Security.Cryptography.X509Certificates;
#if NETSTANDARD2_1
using System.IdentityModel.Tokens.Jwt;
#endif
#if !ONPREMISES
#endif

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommunications.Connect, "PnPOnline", SupportsShouldProcess = false)]
    [CmdletHelp("Connect to a SharePoint site",
        @"Connects to a SharePoint site and creates a context that is required for the other PnP Cmdlets.
To automate authentication there are several options. The easiest would be to use the Windows Credential Manager. Either manually add a Generic Credential, or use the Add-PnPStoredCredential cmdlet to add an entry. The name you give to the credential can be used in two main ways. If you simply give it a name alike 'O365' or any other value you can specify this value for the Credentials parameter of this cmdlet.

Alternatively you can specify a URL as a name, alike 'https://contoso.sharepoint.com'. Any site you connect to within the contoso tenant will then use the credentials you specified. For more information see the help for the Add-PnPStoredCredential cmdlet. (Get-Help Add-PnPStoredCredential).

Make sure to check the SPOManagement, PnPO365ManagementShell and AccessToken parameters too.",
        DetailedDescription = "If no credentials have been specified, and the CurrentCredentials parameter has not been specified, you will be prompted for credentials.",
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com",
        Remarks = @"This will prompt for username and password and creates a context for the other PowerShell commands to use. When a generic credential is added to the Windows Credential Manager with https://contoso.sharepoint.com, PowerShell will not prompt for username and password.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -Credentials (Get-Credential)",
        Remarks = @"This will prompt for username and password and creates a context for the other PowerShell commands to use. ",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url http://yourlocalserver -CurrentCredentials",
        Remarks = @"This will use the current user credentials and connects to the server specified by the Url parameter.",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url http://yourlocalserver -Credentials 'O365Creds'",
        Remarks = @"This will use credentials from the Windows Credential Manager, as defined by the label 'O365Creds'.",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url http://yourlocalserver -Credentials (Get-Credential) -UseAdfs",
        Remarks = @"This will prompt for username and password and creates a context using ADFS to authenticate.",
        SortOrder = 5)]
#if !NETSTANDARD2_1
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url http://yourlocalserver -UseAdfsCert",
        Remarks = @"This will enable you to select a certificate to create a context using ADFS to authenticate.",
        SortOrder = 6)]
#endif
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://yourserver -Credentials (Get-Credential) -CreateDrive
PS:> cd SPO:\\
PS:> dir",
        Remarks = @"This will prompt you for credentials and creates a context for the other PowerShell commands to use. It will also create a SPO:\\ drive you can use to navigate around the site",
        SortOrder = 7)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://yourserver -Credentials (Get-Credential) -AuthenticationMode FormsAuthentication",
        Remarks = @"This will prompt you for credentials and creates a context for the other PowerShell commands to use. It assumes your server is configured for Forms Based Authentication (FBA)",
        SortOrder = 8)]
#if !ONPREMISES
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://contoso.sharepoint.de -AppId 344b8aab-389c-4e4a-8fa1-4c1ae2c0a60d -AppSecret a3f3faf33f3awf3a3sfs3f3ss3f4f4a3fawfas3ffsrrffssfd -AzureEnvironment Germany",
        Remarks = @"This will authenticate you to the German Azure environment using the German Azure endpoints for authentication",
        SortOrder = 9)]
#endif
#if !ONPREMISES
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -SPOManagementShell",
        Remarks = @"This will authenticate you using the SharePoint Online Management Shell application",
        SortOrder = 10)]
#endif
#if !ONPREMISES
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -PnPO365ManagementShell",
        Remarks = @"This will authenticate you using the PnP O365 Management Shell Multi-Tenant application. A browser window will have to be opened where you have to enter a code that is shown in your PowerShell window.",
        SortOrder = 11)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -PnPO365ManagementShell -LaunchBrowser",
        Remarks = @"This will authenticate you using the PnP O365 Management Shell Multi-Tenant application. A browser window will automatically open and the code you need to enter will be automatically copied to your clipboard.",
        SortOrder = 12)]
#endif
#if !ONPREMISES
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -AccessToken $myaccesstoken",
        Remarks = @"This will authenticate you using the provided access token",
        SortOrder = 13)]
#endif
#if !ONPREMISES
#if !NETSTANDARD2_1
    [CmdletExample(
       Code = "PS:> Connect-PnPOnline -Scopes \"Mail.Read\",\"Files.Read\"",
       Remarks = "Connects to Azure AD and gets and OAuth 2.0 Access Token to consume the Microsoft Graph API including the declared permission scopes. The available permission scopes are defined at the following URL: https://docs.microsoft.com/en-us/graph/permissions-reference",
       SortOrder = 14)]
#endif
#endif
#if !ONPREMISES
    [CmdletExample(
       Code = "PS:> Connect-PnPOnline -AppId '<id>' -AppSecret '<secret>' -AADDomain 'contoso.onmicrosoft.com'",
       Remarks = "Connects to the Microsoft Graph API using application permissions via an app's declared permission scopes. See https://github.com/SharePoint/PnP-PowerShell/tree/master/Samples/Graph.ConnectUsingAppPermissions for a sample on how to get started.",
       SortOrder = 15)]
    [CmdletExample(
        Code = "PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -ClientId '<id>' -Tenant 'contoso.onmicrosoft.com' -CertificatePath c:\\absolute-path\\to\\pnp.pfx -CertificatePassword <if needed>",
        Remarks = "Connects to SharePoint using app-only tokens via an app's declared permission scopes. See https://github.com/SharePoint/PnP-PowerShell/tree/master/Samples/SharePoint.ConnectUsingAppPermissions for a sample on how to get started.",
        SortOrder = 16)]
    [CmdletExample(
        Code = "PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -ClientId '<id>' -Tenant 'contoso.onmicrosoft.com' -Thumbprint 34CFAA860E5FB8C44335A38A097C1E41EEA206AA",
        Remarks = "Connects to SharePoint using app-only tokens via an app's declared permission scopes. See https://github.com/SharePoint/PnP-PowerShell/tree/master/Samples/SharePoint.ConnectUsingAppPermissions for a sample on how to get started.",
        SortOrder = 17)]
    [CmdletExample(
        Code = "PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -ClientId '<id>' -Tenant 'contoso.onmicrosoft.com' -PEMCertificate <PEM string> -PEMPrivateKey <PEM string> -CertificatePassword <if needed>",
        Remarks = "Connects to SharePoint using app-only tokens via an app's declared permission scopes. See https://github.com/SharePoint/PnP-PowerShell/tree/master/Samples/SharePoint.ConnectUsingAppPermissions for a sample on how to get started.",
        SortOrder = 17)]
    [CmdletExample(
        Code = "PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -ClientId '<id>' -Tenant 'contoso.onmicrosoft.com' -Certificate <X509Certificate2>",
        Remarks = "Connects to SharePoint using app-only auth in combination with a certificate. See https://docs.microsoft.com/en-us/sharepoint/dev/solution-guidance/security-apponly-azuread#using-this-principal-in-your-powershell-script-using-the-pnp-sites-core-library for a sample on how to get started.",
        SortOrder = 18)]
#endif
#if ONPREMISES
    [CmdletExample(
        Code = @"PS:> certutil.exe -csp 'Microsoft Enhanced RSA and AES Cryptographic Provider' -v -p 'password' -importpfx -user c:\HighTrust.pfx NoRoot
PS:> Connect-PnPOnline -Url https://yourserver -ClientId <id> -HighTrustCertificate (Get-Item Cert:\CurrentUser\My\<thumbprint>)",
        Remarks = @"Connect to an on-premises SharePoint environment using a high trust certificate, stored in the Personal certificate store of the current user.",
        SortOrder = 15)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://yourserver -ClientId 763d5e60-b57e-426e-8e87-b7258f7f8188 -HighTrustCertificatePath c:\HighTrust.pfx -HighTrustCertificatePassword 'password' -HighTrustCertificateIssuerId 6b9534d8-c2c1-49d6-9f4b-cd415620bca8",
        Remarks = @"Connect to an on-premises SharePoint environment using a high trust certificate stored in a .PFX file.",
        SortOrder = 16)]
#endif
#if !ONPREMISES
    [CmdletExample(
       Code = "PS:> Connect-PnPOnline -ClientId <id> -CertificatePath 'c:\\mycertificate.pfx' -CertificatePassword (ConvertTo-SecureString -AsPlainText 'myprivatekeypassword' -Force) -Url https://contoso.sharepoint.com -Tenant 'contoso.onmicrosoft.com'",
       Remarks = "Connects using an Azure Active Directory registered application using a locally available certificate containing a private key. See https://docs.microsoft.com/en-us/sharepoint/dev/solution-guidance/security-apponly-azuread for a sample on how to get started.",
       SortOrder = 16)]
    [CmdletExample(
       Code = "PS:> Connect-PnPOnline -ClientId <id> -CertificateBase64Encoded 'xxxx' -CertificatePassword (ConvertTo-SecureString -AsPlainText 'myprivatekeypassword' -Force) -Url https://contoso.sharepoint.com -Tenant 'contoso.onmicrosoft.com'",
       Remarks = "Connects using an Azure Active Directory registered application using a certificate containing a private key encoded in base 64 such as received in an Azure Function when using Azure KeyVault. See https://docs.microsoft.com/en-us/sharepoint/dev/solution-guidance/security-apponly-azuread for a sample on how to get started.",
       SortOrder = 17)]
    [CmdletExample(
       Code = "PS:> Connect-PnPOnline -ClientId <id> -Certificate $cert -CertificatePassword (ConvertTo-SecureString -AsPlainText 'myprivatekeypassword' -Force) -Url https://contoso.sharepoint.com -Tenant 'contoso.onmicrosoft.com'",
       Remarks = "Connects using an Azure Active Directory registered application using a certificate instance containing a private key. See https://docs.microsoft.com/en-us/sharepoint/dev/solution-guidance/security-apponly-azuread for a sample on how to get started.",
       SortOrder = 18)]

#endif
    public class ConnectOnline : BasePSCmdlet
    {
        private const string ParameterSet_MAIN = "Main";
        private const string ParameterSet_TOKEN = "Token";
        private const string ParameterSet_WEBLOGIN = "WebLogin";
#if !ONPREMISES
        private const string ParameterSet_NATIVEAAD = "Azure Active Directory";
        private const string ParameterSet_APPONLYAAD = "App-Only with Azure Active Directory";
        private const string ParameterSet_APPONLYAADPEM = "App-Only with Azure Active Directory using certificate as PEM strings";
        private const string ParameterSet_APPONLYAADCER = "App-Only with Azure Active Directory using X502 certificates";
        private const string ParameterSet_APPONLYAADThumb = "App-Only with Azure Active Directory using certificate from certificate store by thumbprint";
        private const string ParameterSet_SPOMANAGEMENT = "SPO Management Shell Credentials";
        private const string ParameterSet_DEVICELOGIN = "PnP O365 Management Shell / DeviceLogin";
        private const string ParameterSet_GRAPHDEVICELOGIN = "PnP Office 365 Management Shell to the Microsoft Graph";
#if !NETSTANDARD2_1
        private const string ParameterSet_GRAPHWITHSCOPE = "Microsoft Graph using Scopes";
#endif
        private const string ParameterSet_GRAPHWITHAAD = "Microsoft Graph using Azure Active Directory";
        private const string SPOManagementClientId = "9bc3ab49-b65d-410a-85ad-de819febfddc";
        private const string SPOManagementRedirectUri = "https://oauth.spops.microsoft.com/";
        private const string MSALPnPPowerShellClientId = "bb0c5778-9d5c-41ea-a4a8-8cd417b3ab71";
        private const string GraphRedirectUri = "urn:ietf:wg:oauth:2.0:oob";
        private const string ParameterSet_ACCESSTOKEN = "Access Token";
        private static readonly Uri GraphAADLogin = new Uri("https://login.microsoftonline.com/");
        private static readonly string[] GraphDefaultScope = { "https://graph.microsoft.com/.default" };
#endif


#if ONPREMISES
        private const string ParameterSet_HIGHTRUST_CERT = "High Trust using a X509Certificate2 object.";
        private const string ParameterSet_HIGHTRUST_PFX = "High Trust using a certificate from a PFX file.";
#endif
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADThumb, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADCER, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPOMANAGEMENT, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ACCESSTOKEN, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_DEVICELOGIN, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
#endif
#if ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_CERT, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_PFX, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
#endif
        public SwitchParameter ReturnConnection;

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_MAIN, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to.")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_TOKEN, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to.")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_WEBLOGIN, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to.")]
#if !ONPREMISES
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_NATIVEAAD, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to.")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_APPONLYAAD, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to.")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_APPONLYAADPEM, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to.")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_APPONLYAADCER, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to.")][Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_APPONLYAADThumb, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to.")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_SPOMANAGEMENT, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to.")]
        [Parameter(Mandatory = false, Position = 0, ParameterSetName = ParameterSet_ACCESSTOKEN, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to.")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_DEVICELOGIN, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to.")]
#endif
#if ONPREMISES
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_HIGHTRUST_PFX, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to.")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_HIGHTRUST_CERT, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to.")]
#endif
        public string Url;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "Credentials of the user to connect with. Either specify a PSCredential object or a string. In case of a string value a lookup will be done to the Generic Credentials section of the Windows Credentials in the Windows Credential Manager for the correct credentials.")]
        public CredentialPipeBind Credentials;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "If you want to connect with the current user credentials")]
        public SwitchParameter CurrentCredentials;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "If you want to connect to your on-premises SharePoint farm using ADFS")]
        public SwitchParameter UseAdfs;

        [Parameter(Mandatory = false, ParameterSetName = "Main", HelpMessage = "If you want to connect to your SharePoint farm using ADFS with Certificate Authentication")]
        public SwitchParameter UseAdfsCert;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "Authenticate using Kerberos to an on-premises ADFS instance.")]
        public SwitchParameter Kerberos;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "The name of the ADFS trusted login provider")]
        public string LoginProviderName;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "Specifies a minimal server healthscore before any requests are executed.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "Specifies a minimal server healthscore before any requests are executed.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "Specifies a minimal server healthscore before any requests are executed.")]
#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "Specifies a minimal server healthscore before any requests are executed.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "Specifies a minimal server healthscore before any requests are executed.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "Specifies a minimal server healthscore before any requests are executed.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "Specifies a minimal server healthscore before any requests are executed.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "Specifies a minimal server healthscore before any requests are executed.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPOMANAGEMENT, HelpMessage = "Specifies a minimal server healthscore before any requests are executed.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ACCESSTOKEN, HelpMessage = "Specifies a minimal server healthscore before any requests are executed.")]
#endif
#if ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "Specifies a minimal server healthscore before any requests are executed.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "Specifies a minimal server healthscore before any requests are executed.")]
#endif
        public int MinimalHealthScore = -1;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPOMANAGEMENT, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ACCESSTOKEN, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
#endif
#if ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
#endif
        public int RetryCount = 10;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPOMANAGEMENT, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ACCESSTOKEN, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
#endif
#if ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
#endif
        public int RetryWait = 1;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "The request timeout. Default is 1800000")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "The request timeout. Default is 1800000")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "The request timeout. Default is 1800000")]
#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "The request timeout. Default is 1800000")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "The request timeout. Default is 1800000")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "The request timeout. Default is 1800000")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "The request timeout. Default is 1800000")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "The request timeout. Default is 180000")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPOMANAGEMENT, HelpMessage = "The request timeout. Default is 1800000")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ACCESSTOKEN, HelpMessage = "The request timeout. Default is 1800000")]
#endif
#if ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "The request timeout. Default is 1800000")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "The request timeout. Default is 1800000")]
#endif
        public int RequestTimeout = 1800000;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "Authentication realm. If not specified will be resolved from the url specified.")]
        public string Realm;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "The Application Client ID to use.")]
#if !ONPREMISES
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_GRAPHWITHAAD, HelpMessage = "The client id of the app which gives you access to the Microsoft Graph API.")]
#endif
        public string AppId;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "The Application Client Secret to use.")]
#if !ONPREMISES
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_GRAPHWITHAAD, HelpMessage = "The app key of the app which gives you access to the Microsoft Graph API.")]
#endif
        public string AppSecret;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "If you want to connect to SharePoint with browser based login. This is required when you have multi-factor authentication (MFA) enabled.")]
        public SwitchParameter UseWebLogin;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "Specify to use for instance use forms based authentication (FBA)")]
        public ClientAuthenticationMode AuthenticationMode = ClientAuthenticationMode.Default;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "If you want to create a PSDrive connected to the URL")]
#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPOMANAGEMENT, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ACCESSTOKEN, HelpMessage = "If you want to create a PSDrive connected to the URL")]
#endif
#if ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "If you want to create a PSDrive connected to the URL")]
#endif
        public SwitchParameter CreateDrive;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPOMANAGEMENT, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ACCESSTOKEN, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
#endif
#if ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
#endif
        public string DriveName = "SPO";

#if !ONPREMISES
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_SPOMANAGEMENT, HelpMessage = "Log in using the SharePoint Online Management Shell application")]
        public SwitchParameter SPOManagementShell;


        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_DEVICELOGIN, HelpMessage = @"Log in using the PnP O365 Management Shell application. You will be asked to consent to:

* Read and write managed metadata
* Have full control of all site collections
* Read user profiles
* Invite guest users to the organization
* Read and write all groups
* Read and write directory data
* Read and write identity providers
* Access the directory as you")]
        public SwitchParameter PnPO365ManagementShell;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_DEVICELOGIN, HelpMessage = "Launch a browser automatically and copy the code to enter to the clipboard")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_GRAPHDEVICELOGIN, HelpMessage = "Launch a browser automatically and copy the code to enter to the clipboard")]
        public SwitchParameter LaunchBrowser;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_GRAPHDEVICELOGIN, HelpMessage = @"Log in using the PnP O365 Management Shell application towards the Graph. You will be asked to consent to:

* Read and write managed metadata
* Have full control of all site collections
* Read user profiles
* Invite guest users to the organization
* Read and write all groups
* Read and write directory data
* Read and write identity providers
* Access the directory as you
")]
        public SwitchParameter Graph;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "The Client ID of the Azure AD Application")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "The Client ID of the Azure AD Application")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "The Client ID of the Azure AD Application")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "The Client ID of the Azure AD Application")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "The Client ID of the Azure AD Application")]
#endif
#if ONPREMISES
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "The Client ID of the Add-In Registration in SharePoint. Used as the HighTrustCertificateIssuerId if none is specified.")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "The Client ID of the Add-In Registration in SharePoint. Used as the HighTrustCertificateIssuerId if none is specified.")]
#endif
        public string ClientId;

#if !ONPREMISES
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "The Redirect URI of the Azure AD Application")]
        public string RedirectUri;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "The Azure AD Tenant name,e.g. mycompany.onmicrosoft.com")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "The Azure AD Tenant name,e.g. mycompany.onmicrosoft.com")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "The Azure AD Tenant name,e.g. mycompany.onmicrosoft.com")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "The Azure AD Tenant name,e.g. mycompany.onmicrosoft.com")]
        public string Tenant;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "Path to the certificate containing the private key (*.pfx)")]
        public string CertificatePath;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "Base64 Encoded X509Certificate2 certificate containing the private key to authenticate the requests to SharePoint Online such as retrieved in Azure Functions from Azure KeyVault")]
        public string CertificateBase64Encoded;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "X509Certificate2 reference containing the private key to authenticate the requests to SharePoint Online")]
        public System.Security.Cryptography.X509Certificates.X509Certificate2 Certificate;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "Password to the certificate (*.pfx)")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "Password to the certificate (*.pfx)")]
        public SecureString CertificatePassword;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "PEM encoded certificate")]
        public string PEMCertificate;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "PEM encoded private key for the certificate")]
        public string PEMPrivateKey;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "Certificate thumbprint")]
        public string Thumbprint;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "Clears the token cache.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPOMANAGEMENT, HelpMessage = "Clears the token cache.")]
        public SwitchParameter ClearTokenCache;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.")]
        public AzureEnvironment AzureEnvironment = AzureEnvironment.Production;

#if !NETSTANDARD2_1
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "The array of permission scopes for the Microsoft Graph API.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "The array of permission scopes for the Microsoft Graph API.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "The array of permission scopes for the Microsoft Graph API.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "The array of permission scopes for the Microsoft Graph API.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "The array of permission scopes for the Microsoft Graph API.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPOMANAGEMENT, HelpMessage = "The array of permission scopes for the Microsoft Graph API.")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_GRAPHWITHSCOPE, HelpMessage = "The array of permission scopes for the Microsoft Graph API.")]
        public string[] Scopes;
#endif

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_GRAPHWITHAAD, HelpMessage = "The AAD where the O365 app is registered. Eg.: contoso.com, or contoso.onmicrosoft.com.")]
        public string AADDomain;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_ACCESSTOKEN, HelpMessage = "Connect with an existing Access Token")]
        public string AccessToken;
#endif
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPOMANAGEMENT, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
#endif
#if ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
#endif
        public string TenantAdminUrl;


        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")][Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPOMANAGEMENT, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ACCESSTOKEN, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
#endif
#if ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
#endif
        public SwitchParameter SkipTenantAdminCheck;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_GRAPHWITHAAD, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting through a proxy to the Microsoft Graph API which has SSL interception enabled.")]
#if !NETSTANDARD2_1
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_GRAPHWITHSCOPE, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting through a proxy to the Microsoft Graph API which has SSL interception enabled.")]
#endif
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_GRAPHDEVICELOGIN, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting through a proxy to the Microsoft Graph API which has SSL interception enabled.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPOMANAGEMENT, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]        
#endif
#if ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
#endif
        public SwitchParameter IgnoreSslErrors;

        [Parameter(Mandatory = false, HelpMessage = "In order to help to make PnP PowerShell better, we can track anonymous telemetry. We track the version of the cmdlets you are using, which cmdlet you are executing and which version of SharePoint you are connecting to. Use Disable-PnPPowerShellTelemetry to turn this off in general or use the -NoTelemetry switch to turn it off for that session.")]
        public SwitchParameter NoTelemetry;

#if ONPREMISES
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "The certificate which has been registered in SharePoint as a Trusted Security Token issuer to use for the High Trust connection. Note that CNG key storage providers are not supported.")]
        public System.Security.Cryptography.X509Certificates.X509Certificate2 HighTrustCertificate;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "The path to the private key certificate (.pfx) to use for the High Trust connection")]
        public string HighTrustCertificatePath;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "The password of the private key certificate (.pfx) to use for the High Trust connection")]
        public string HighTrustCertificatePassword;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "The IssuerID under which the certificate has been registered in SharePoint as a Trusted Security Token issuer to use for the High Trust connection. Uses the ClientID if not specified.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "The IssuerID under which the CER counterpart of the PFX has been registered in SharePoint as a Trusted Security Token issuer to use for the High Trust connection. Uses the ClientID if not specified.")]
        public string HighTrustCertificateIssuerId;
#endif

        protected override void ProcessRecord()
        {
            try
            {
                Connect();
            } catch (Exception ex)
            {
                ex.Data.Add("TimeStampUtc", DateTime.UtcNow);
                throw ex;
            }
        }
        protected void Connect()
        {
            var latestVersion = SPOnlineConnectionHelper.GetLatestVersion();
            if (!string.IsNullOrEmpty(latestVersion))
            {
                WriteWarning(latestVersion);
            }

            if (IgnoreSslErrors)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            }
            PSCredential creds = null;
            if (Credentials != null)
            {
                creds = Credentials.Credential;
            }
            SPOnlineConnection connection = null;
            if (ParameterSetName == ParameterSet_TOKEN)
            {
                connection = SPOnlineConnectionHelper.InstantiateSPOnlineConnection(new Uri(Url), Realm, AppId, AppSecret, Host, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, SkipTenantAdminCheck, false, AzureEnvironment);
            }
            else if (UseWebLogin)
            {
#if !NETSTANDARD2_1
                connection = SPOnlineConnectionHelper.InstantiateWebloginConnection(new Uri(Url), MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, SkipTenantAdminCheck);
#else
                WriteWarning(@"-UseWebLogin is not implemented, due to restrictions of the .NET Standard framework.
Use -PnPO365ManagementShell instead");
#endif
            }
            else if (UseAdfs)
            {
                if (!Kerberos && creds == null)
                {
                    if ((creds = GetCredentials()) == null)
                    {
                        creds = Host.UI.PromptForCredential(Properties.Resources.EnterYourCredentials, "", "", "");
                    }
                }
#if !NETSTANDARD2_1
                connection = SPOnlineConnectionHelper.InstantiateAdfsConnection(new Uri(Url),
                    Kerberos,
                    creds,
                    Host,
                    MinimalHealthScore,
                    RetryCount,
                    RetryWait,
                    RequestTimeout,
                    TenantAdminUrl,
                    NoTelemetry,
                    SkipTenantAdminCheck,
                    LoginProviderName);
#else
                throw new NotImplementedException();
#endif
            }
#if !NETSTANDARD2_1
            else if (UseAdfsCert)
            {
                // Modal Dialog to enable a user to select a certificate to use to authenticate against ADFS
                X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                var certs = X509Certificate2UI.SelectFromCollection(store.Certificates, "Select ADFS User Certificate", "Selec the certificate to use to authenticate to ADFS", X509SelectionFlag.SingleSelection);

                if (certs[0] != null)
                {
                    var serialNumber = certs[0].SerialNumber;

                    SPOnlineConnection.CurrentConnection = SPOnlineConnectionHelper.InstantiateAdfsCertificateConnection(new Uri(Url), serialNumber, Host, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, SkipTenantAdminCheck);
                }
            }
#endif
#if !ONPREMISES
            else if (ParameterSetName == ParameterSet_SPOMANAGEMENT)
            {
                connection = ConnectNativeAAD(SPOManagementClientId, SPOManagementRedirectUri);
            }
            else if (ParameterSetName == ParameterSet_DEVICELOGIN)
            {
                connection = ConnectDeviceLogin();
            }
            else if (ParameterSetName == ParameterSet_GRAPHDEVICELOGIN)
            {
                connection = ConnectGraphDeviceLogin(null);
            }
            else if (ParameterSetName == ParameterSet_NATIVEAAD)
            {
                connection = ConnectNativeAAD(ClientId, RedirectUri);
            }
            else if (ParameterSetName == ParameterSet_APPONLYAAD)
            {
#if !NETSTANDARD2_1
                if (MyInvocation.BoundParameters.ContainsKey("CertificatePath"))
                {
                    connection = SPOnlineConnectionHelper.InitiateAzureADAppOnlyConnection(new Uri(Url), ClientId, Tenant, CertificatePath, CertificatePassword, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment);
                    WriteWarning(@"Your certificate is copied by the operating system to c:\ProgramData\Microsoft\Crypto\RSA\MachineKeys. Over time this folder may increase heavily in size. Use Disconnect-PnPOnline in your scripts remove the certificate from this folder to clean up. Consider using -Thumbprint instead of -CertificatePath.");
                } else if (MyInvocation.BoundParameters.ContainsKey("Certificate"))
                {
                    connection = SPOnlineConnectionHelper.InitiateAzureAdAppOnlyConnectionWithCert(new Uri(Url), ClientId, Tenant, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment, Certificate);
                } else if (MyInvocation.BoundParameters.ContainsKey("CertificateBase64Encoded"))
                {
                    connection = SPOnlineConnectionHelper.InitiateAzureAdAppOnlyConnectionWithCert(new Uri(Url), ClientId, Tenant, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment, CertificateBase64Encoded);
                } else
                {
                    throw new ArgumentException("You must either provide CertificatePath, Certificate or CertificateBase64Encoded when connecting using an Azure Active Directory registered application");
                }
#else
                throw new NotImplementedException();
#endif
            }
            else if (ParameterSetName == ParameterSet_APPONLYAADPEM)
            {
#if !NETSTANDARD2_1
                connection = SPOnlineConnectionHelper.InitiateAzureADAppOnlyConnection(new Uri(Url), ClientId, Tenant, PEMCertificate, PEMPrivateKey, CertificatePassword, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment);
#else
                throw new NotImplementedException();
#endif
            }
            else if (ParameterSetName == ParameterSet_APPONLYAADThumb)
            {
#if !NETSTANDARD2_1
                connection = SPOnlineConnectionHelper.InitiateAzureADAppOnlyConnection(new Uri(Url), ClientId, Tenant, Thumbprint, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment);
#else
                throw new NotImplementedException();
#endif
            }
            else if (ParameterSetName == ParameterSet_APPONLYAADCER)
            {
#if !NETSTANDARD2_1
                connection = SPOnlineConnectionHelper.InitiateAzureADAppOnlyConnection(new Uri(Url), ClientId, Tenant, Certificate, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment);
#else
                throw new NotImplementedException();	
#endif	
            }
#if !NETSTANDARD2_1
            else if (ParameterSetName == ParameterSet_GRAPHWITHSCOPE)
            {
                ConnectGraphScopes();
            }
#endif
            else if (ParameterSetName == ParameterSet_GRAPHWITHAAD)
            {
                ConnectGraphAAD();
            }
            else if (ParameterSetName == ParameterSet_ACCESSTOKEN)
            {
                var jwtToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(AccessToken);
                var aud = jwtToken.Audiences.FirstOrDefault();
                var url = Url;
                if (aud != null)
                {
                    url = aud;
                }
                if (url.ToLower() == "https://graph.microsoft.com")
                {
                    connection = ConnectGraphDeviceLogin(AccessToken);
                }
                else
                {
                    Uri uri = null;
                    try
                    {
                        uri = new Uri(url);
                    }
                    catch
                    {
                        uri = new Uri(Url);
                    }
                    //#if !NETSTANDARD2_0
                    connection = SPOnlineConnectionHelper.InitiateAccessTokenConnection(uri, AccessToken, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment);
                    //#else
                    //throw new NotImplementedException();
                    //#endif
                }
            }
#endif
#if ONPREMISES
            else if (ParameterSetName == ParameterSet_HIGHTRUST_CERT)
            {
                connection = SPOnlineConnectionHelper.InstantiateHighTrustConnection(Url, ClientId, HighTrustCertificate, HighTrustCertificateIssuerId ?? ClientId, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck);
            }
            else if (ParameterSetName == ParameterSet_HIGHTRUST_PFX)
            {
                connection = SPOnlineConnectionHelper.InstantiateHighTrustConnection(Url, ClientId, HighTrustCertificatePath, HighTrustCertificatePassword, HighTrustCertificateIssuerId ?? ClientId, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck);
            }
#endif
            else
            {
                if (!CurrentCredentials && creds == null)
                {
                    creds = GetCredentials();
                    if (creds == null)
                    {
                        creds = Host.UI.PromptForCredential(Properties.Resources.EnterYourCredentials, "", "", "");
                    }
                }
                connection = SPOnlineConnectionHelper.InstantiateSPOnlineConnection(new Uri(Url), creds, Host, CurrentCredentials, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, NoTelemetry, SkipTenantAdminCheck, AuthenticationMode);
            }
#if !ONPREMISES
#if !NETSTANDARD2_1
            if (MyInvocation.BoundParameters.ContainsKey("Scopes") && ParameterSetName != ParameterSet_GRAPHWITHSCOPE)
            {
                ConnectGraphScopes();
            }
#endif
#endif
            WriteVerbose($"PnP PowerShell Cmdlets ({System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}): Connected to {Url}");
            SPOnlineConnection.CurrentConnection = connection;
            if (CreateDrive && SPOnlineConnection.CurrentConnection.Context != null)
            {
                var provider = SessionState.Provider.GetAll().FirstOrDefault(p => p.Name.Equals(SPOProvider.PSProviderName, StringComparison.InvariantCultureIgnoreCase));
                if (provider != null)
                {
                    if (provider.Drives.Any(d => d.Name.Equals(DriveName, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        SessionState.Drive.Remove(DriveName, true, "Global");
                    }

                    var drive = new PSDriveInfo(DriveName, provider, string.Empty, Url, null);
                    SessionState.Drive.New(drive, "Global");
                }
            }
            if (SPOnlineConnection.CurrentConnection != null)
            {
                if (SPOnlineConnection.CurrentConnection.ConnectionMethod != Model.ConnectionMethod.GraphDeviceLogin)
                {
                    var hostUri = new Uri(SPOnlineConnection.CurrentConnection.Url);
                    Environment.SetEnvironmentVariable("PNPPSHOST", hostUri.Host);
                    Environment.SetEnvironmentVariable("PNPPSSITE", hostUri.LocalPath);
                }
                else
                {
                    Environment.SetEnvironmentVariable("PNPPSHOST", "GRAPH");
                    Environment.SetEnvironmentVariable("PNPPSSITE", "GRAPH");
                }
            }
            if (ReturnConnection)
            {
                WriteObject(connection);
            }
        }

#if !ONPREMISES
        private SPOnlineConnection ConnectNativeAAD(string clientId, string redirectUrl)
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string configFolder = Path.Combine(appDataFolder, "SharePointPnP.PowerShell");
            Directory.CreateDirectory(configFolder); // Ensure folder exists
            if (ClearTokenCache)
            {
                string configFile = Path.Combine(configFolder, "tokencache.dat");

                if (File.Exists(configFile))
                {
                    File.Delete(configFile);
                }
            }
#if !NETSTANDARD2_1
            return SPOnlineConnectionHelper.InitiateAzureADNativeApplicationConnection(
                new Uri(Url), clientId, new Uri(redirectUrl), MinimalHealthScore, RetryCount,
                RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment);
#else
            throw new NotImplementedException();
#endif
        }

#if !NETSTANDARD2_1
        private void ConnectGraphScopes()
        {
            var clientApplication = new PublicClientApplication(MSALPnPPowerShellClientId);
            var authenticationResult = clientApplication.AcquireTokenAsync(Scopes).GetAwaiter().GetResult();
            SPOnlineConnection.AuthenticationResult = authenticationResult;
        }
#endif

        private SPOnlineConnection ConnectDeviceLogin()
        {
            bool ctrlCAsInput = false;
            if (Host.Name == "ConsoleHost")
            {
                ctrlCAsInput = Console.TreatControlCAsInput;
                Console.TreatControlCAsInput = true;
            }

            var uri = new Uri(Url);
            if ($"https://{uri.Host}".Equals(Url.ToLower()))
            {
                Url = Url + "/";
            }
            var connection = SPOnlineConnectionHelper.InstantiateDeviceLoginConnection(Url, LaunchBrowser, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, (message) =>
            {
                WriteWarning(message);
            },
            (progress) =>
            {
                Host.UI.Write(progress);
            },
            () =>
            {
                if (Host.Name == "ConsoleHost")
                {
                    if (Console.KeyAvailable)
                    {
                        var cki = Console.ReadKey(true);
                        if (cki.Key == ConsoleKey.C && cki.Modifiers == ConsoleModifiers.Control)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }, Host, NoTelemetry);
            Console.TreatControlCAsInput = ctrlCAsInput;
            return connection;
        }

        private SPOnlineConnection ConnectGraphDeviceLogin(string accessToken)
        {

            if (string.IsNullOrEmpty(accessToken))
            {
                bool ctrlCAsInput = false;
                if (Host.Name == "ConsoleHost")
                {
                    ctrlCAsInput = Console.TreatControlCAsInput;
                    Console.TreatControlCAsInput = true;
                }

                var connection = SPOnlineConnectionHelper.InstantiateGraphDeviceLoginConnection(LaunchBrowser, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, (message) =>
                {
                    WriteWarning(message);
                },
                (progress) =>
                {
                    Host.UI.Write(progress);
                },
                () =>
                {
                    if (Host.Name == "ConsoleHost")
                    {
                        if (Console.KeyAvailable)
                        {
                            var cki = Console.ReadKey(true);
                            if (cki.Key == ConsoleKey.C && cki.Modifiers == ConsoleModifiers.Control)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }, Host, NoTelemetry);
                Console.TreatControlCAsInput = ctrlCAsInput;
                return connection;
            }
            else
            {
                return SPOnlineConnectionHelper.InstantiateGraphAccessTokenConnection(accessToken, Host, NoTelemetry);
            }
        }


        private void ConnectGraphAAD()
        {
            var appCredentials = new ClientCredential(AppSecret);
            var authority = new Uri(GraphAADLogin, AADDomain).AbsoluteUri;
#if !NETSTANDARD2_1
            var clientApplication = new ConfidentialClientApplication(authority, AppId, RedirectUri, appCredentials, null);
            var authenticationResult = clientApplication.AcquireTokenForClient(GraphDefaultScope, null).GetAwaiter().GetResult();
            SPOnlineConnection.AuthenticationResult = authenticationResult;
#else
            throw new NotImplementedException();
#endif
        }
#endif

        private PSCredential GetCredentials()
        {
            PSCredential creds;

            var connectionUri = new Uri(Url);

            // Try to get the credentials by full url

            creds = Utilities.CredentialManager.GetCredential(Url);
            if (creds == null)
            {
                // Try to get the credentials by splitting up the path
                var pathString = $"{connectionUri.Scheme}://{(connectionUri.IsDefaultPort ? connectionUri.Host : $"{connectionUri.Host}:{connectionUri.Port}")}";
                var path = connectionUri.AbsolutePath;
                while (path.IndexOf('/') != -1)
                {
                    path = path.Substring(0, path.LastIndexOf('/'));
                    if (!string.IsNullOrEmpty(path))
                    {
                        var pathUrl = $"{pathString}{path}";
                        creds = Utilities.CredentialManager.GetCredential(pathUrl);
                        if (creds != null)
                        {
                            break;
                        }
                    }
                }

                if (creds == null)
                {
                    // Try to find the credentials by schema and hostname
                    creds = Utilities.CredentialManager.GetCredential(connectionUri.Scheme + "://" + connectionUri.Host);

                    if (creds == null)
                    {
                        // try to find the credentials by hostname
                        creds = Utilities.CredentialManager.GetCredential(connectionUri.Host);
                    }
                }

            }

            return creds;
        }
    }
}
