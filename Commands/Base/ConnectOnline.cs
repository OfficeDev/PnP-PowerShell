using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Provider;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Security;
using File = System.IO.File;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Tokens.Jwt;
using PnP.PowerShell.Commands.Enums;
#if !PNPPSCORE
using System.Web.UI.WebControls;
#endif
using PnP.PowerShell.Commands.Model;
using Resources = PnP.PowerShell.Commands.Properties.Resources;
using System.Collections.Generic;
using OfficeDevPnP.Core.Utilities;
#if !PNPPSCORE
using System.Security.Cryptography;
#endif
using System.Reflection;
#if !ONPREMISES
#endif

namespace PnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommunications.Connect, "PnPOnline", SupportsShouldProcess = false, DefaultParameterSetName = ParameterSet_MAIN)]
    [CmdletHelp("Connect to a SharePoint site",
        DetailedDescription = @"Connects to a SharePoint site or another API and creates a context that is required for the other PnP Cmdlets. See https://github.com/pnp/PnP-PowerShell/wiki/Connect-options for more information on the options to connect and the APIs you can access with them.",
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com",
        Remarks = @"Connect to SharePoint prompting for the username and password. When a generic credential is added to the Windows Credential Manager with https://contoso.sharepoint.com, PowerShell will not prompt for username and password and use those stored credentials instead.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -Credentials (Get-Credential)",
        Remarks = @"Connect to SharePoint prompting for the username and password to use to authenticate",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url http://yourlocalserver -CurrentCredentials",
        Remarks = @"Connect to SharePoint using the credentials of the current user logged in to the machine",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url http://yourlocalserver -Credentials 'O365Creds'",
        Remarks = @"Connect to SharePoint using credentials from the Windows Credential Manager, as defined by the label 'O365Creds'",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url http://yourlocalserver -Credentials (Get-Credential) -UseAdfs",
        Remarks = @"Connect to SharePoint through ADFS prompting for the username and password to authenticate with",
        SortOrder = 5)]
#if !PNPPSCORE
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url http://yourlocalserver -UseAdfsCert",
        Remarks = @"Connect to SharePoint through ADFS using client certificate allowing you to select the client certificate to use for authentication",
        SortOrder = 6)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url http://yourlocalserver -UseAdfsCert -ClientCertificate (Get-ChildItem -Path Cert:\CurrentUser\My\3A16F907D2BFFF1C22F447E55429C16F8BD3AC6E)",
        Remarks = @"Connect to SharePoint through ADFS using the client certificate with thumbprint 3A16F907D2BFFF1C22F447E55429C16F8BD3AC6E from the local machine certificate store for the current user",
        SortOrder = 7)]
#endif
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://yourserver -Credentials (Get-Credential) -CreateDrive
PS:> cd SPO:\\
PS:> dir",
        Remarks = @"This will prompt you for credentials and creates a context for the other PowerShell commands to use. It will also create a SPO:\\ drive you can use to navigate around the site",
        SortOrder = 8)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://yourserver -Credentials (Get-Credential) -AuthenticationMode FormsAuthentication",
        Remarks = @"This will prompt you for credentials and creates a context for the other PowerShell commands to use. It assumes your server is configured for Forms Based Authentication (FBA)",
        SortOrder = 9)]
#if !ONPREMISES
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://contoso.sharepoint.de -ClientId 344b8aab-389c-4e4a-8fa1-4c1ae2c0a60d -ClientSecret a3f3faf33f3awf3a3sfs3f3ss3f4f4a3fawfas3ffsrrffssfd -AzureEnvironment Germany",
        Remarks = @"This will authenticate you to the German Azure environment using the German Azure endpoints for authentication",
        SortOrder = 10)]
#endif
#if !ONPREMISES
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -SPOManagementShell",
        Remarks = @"This will authenticate you using the SharePoint Online Management Shell application",
        SortOrder = 11)]
#endif
#if !ONPREMISES
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -PnPManagementShell",
        Remarks = @"This will authenticate you using the PnP O365 Management Shell Multi-Tenant application. A browser window will have to be opened where you have to enter a code that is shown in your PowerShell window.",
        SortOrder = 12)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -PnPManagementShell -LaunchBrowser",
        Remarks = @"This will authenticate you using the PnP O365 Management Shell Multi-Tenant application. A browser window will automatically open and the code you need to enter will be automatically copied to your clipboard.",
        SortOrder = 13)]
#endif
#if !ONPREMISES
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -AccessToken $myaccesstoken",
        Remarks = @"Connects using the provided access token",
        SortOrder = 14)]
#endif
#if !ONPREMISES
    [CmdletExample(
       Code = "PS:> Connect-PnPOnline -Scopes \"Mail.Read\",\"Files.Read\",\"ActivityFeed.Read\"",
       Remarks = "Connects to Azure Active Directory interactively and gets an OAuth 2.0 Access Token to consume the resources of the declared permission scopes. It will utilize the Azure Active Directory enterprise application named PnP Management Shell with application id 31359c7f-bd7e-475c-86db-fdb8c937548e registered by the PnP team. If you want to connect using your own Azure Active Directory application registration, use one of the Connect-PnPOnline cmdlets using a -ClientId attribute instead and pre-assign the required permissions/scopes/roles in your application registration in Azure Active Directory. The available permission scopes for Microsoft Graph are defined at the following URL: https://docs.microsoft.com/graph/permissions-reference . If the requested scope(s) have been used with this connect cmdlet before, they will not be asked for consent again. You can request scopes from different APIs in one Connect, i.e. from Microsoft Graph and the Microsoft Office Management API. It will ask you to authenticate for each of the APIs you have listed scopes for.",
       SortOrder = 15)]
    [CmdletExample(
       Code = "PS:> Connect-PnPOnline -Scopes \"Mail.Read\",\"Files.Read\",\"ActivityFeed.Read\" -Credentials (New-Object System.Management.Automation.PSCredential (\"johndoe@contoso.onmicrosoft.com\", (ConvertTo-SecureString \"password\" -AsPlainText -Force)))",
       Remarks = "Connects to Azure Active Directory using delegated permissions and gets an OAuth 2.0 Access Token to consume the resources of the declared permission scopes. It will utilize the Azure Active Directory enterprise application named PnP Management Shell with application id 31359c7f-bd7e-475c-86db-fdb8c937548e registered by the PnP team. If you want to connect using your own Azure Active Directory application registration, use one of the Connect-PnPOnline cmdlets using a -ClientId attribute instead and pre-assign the required permissions/scopes/roles in your application registration in Azure Active Directory. The available permission scopes for Microsoft Graph are defined at the following URL: https://docs.microsoft.com/graph/permissions-reference . If the requested scope(s) have been used with this connect cmdlet before, they will not be asked for consent again. You can request scopes from different APIs in one Connect, i.e. from Microsoft Graph and the Microsoft Office Management API. You must have logged on interactively with the same scopes at least once without using -Credentials to allow for the permission grant dialog to show and allow constent for the user account you would like to use. You can provide this consent by logging in once with Connect-PnPOnline -Url <tenanturl> -PnPManagementShell -LaunchBrowser, and provide consent. This is a one-time action. From that moment on you will be able to use the cmdlet as stated here.",
       SortOrder = 16)]
#endif
#if !ONPREMISES
    [CmdletExample(
       Code = "PS:> Connect-PnPOnline -ClientId '<id>' -ClientSecret '<secret>' -AADDomain 'contoso.onmicrosoft.com'",
       Remarks = "Connects to the Microsoft Graph API using application permissions via an app's declared permission scopes. See https://github.com/SharePoint/PnP-PowerShell/tree/master/Samples/Graph.ConnectUsingAppPermissions for a sample on how to get started.",
       SortOrder = 17)]
    [CmdletExample(
        Code = "PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -ClientId '<id>' -Tenant 'contoso.onmicrosoft.com' -CertificatePath c:\\absolute-path\\to\\pnp.pfx -CertificatePassword <if needed>",
        Remarks = "Connects to SharePoint using app-only tokens via an app's declared permission scopes. See https://github.com/SharePoint/PnP-PowerShell/tree/master/Samples/SharePoint.ConnectUsingAppPermissions for a sample on how to get started.",
        SortOrder = 18)]
    [CmdletExample(
        Code = "PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -ClientId '<id>' -Tenant 'contoso.onmicrosoft.com' -Thumbprint 34CFAA860E5FB8C44335A38A097C1E41EEA206AA",
        Remarks = "Connects to SharePoint using app-only tokens via an app's declared permission scopes. See https://github.com/SharePoint/PnP-PowerShell/tree/master/Samples/SharePoint.ConnectUsingAppPermissions for a sample on how to get started. Ensure you have imported the private key certificate, typically the .pfx file, into the Windows Certificate Store for the certificate with the provided thumbprint.",
        SortOrder = 19)]
    [CmdletExample(
        Code = "PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -ClientId '<id>' -Tenant 'contoso.onmicrosoft.com' -PEMCertificate <PEM string> -PEMPrivateKey <PEM string> -CertificatePassword <if needed>",
        Remarks = "Connects to SharePoint using app-only tokens via an app's declared permission scopes. See https://github.com/SharePoint/PnP-PowerShell/tree/master/Samples/SharePoint.ConnectUsingAppPermissions for a sample on how to get started.",
        SortOrder = 20)]
    [CmdletExample(
        Code = "PS:> Connect-PnPOnline -Url https://contoso.sharepoint.com -ClientId '<id>' -Tenant 'contoso.onmicrosoft.com' -Certificate <X509Certificate2>",
        Remarks = "Connects to SharePoint using app-only auth in combination with a certificate. See https://docs.microsoft.com/en-us/sharepoint/dev/solution-guidance/security-apponly-azuread#using-this-principal-in-your-powershell-script-using-the-pnp-sites-core-library for a sample on how to get started.",
        SortOrder = 21)]
#endif
#if ONPREMISES
    [CmdletExample(
        Code = @"PS:> certutil.exe -csp 'Microsoft Enhanced RSA and AES Cryptographic Provider' -v -p 'password' -importpfx -user c:\HighTrust.pfx NoRoot
PS:> Connect-PnPOnline -Url https://yourserver -ClientId <id> -HighTrustCertificate (Get-Item Cert:\CurrentUser\My\<thumbprint>)",
        Remarks = @"Connect to an on-premises SharePoint environment using a high trust certificate, stored in the Personal certificate store of the current user.",
        SortOrder = 16)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Url https://yourserver -ClientId 763d5e60-b57e-426e-8e87-b7258f7f8188 -HighTrustCertificatePath c:\HighTrust.pfx -HighTrustCertificatePassword 'password' -HighTrustCertificateIssuerId 6b9534d8-c2c1-49d6-9f4b-cd415620bca8",
        Remarks = @"Connect to an on-premises SharePoint environment using a high trust certificate stored in a .PFX file.",
        SortOrder = 17)]
#endif
#if !ONPREMISES
    [CmdletExample(
       Code = "PS:> Connect-PnPOnline -ClientId <id> -CertificatePath 'c:\\mycertificate.pfx' -CertificatePassword (ConvertTo-SecureString -AsPlainText 'myprivatekeypassword' -Force) -Url https://contoso.sharepoint.com -Tenant 'contoso.onmicrosoft.com'",
       Remarks = "Connects using an Azure Active Directory registered application using a locally available certificate containing a private key. See https://docs.microsoft.com/en-us/sharepoint/dev/solution-guidance/security-apponly-azuread for a sample on how to get started.",
       SortOrder = 18)]
    [CmdletExample(
       Code = "PS:> Connect-PnPOnline -ClientId <id> -CertificateBase64Encoded 'xxxx' -CertificatePassword (ConvertTo-SecureString -AsPlainText 'myprivatekeypassword' -Force) -Url https://contoso.sharepoint.com -Tenant 'contoso.onmicrosoft.com'",
       Remarks = "Connects using an Azure Active Directory registered application using a certificate containing a private key encoded in base 64 such as received in an Azure Function when using Azure KeyVault. See https://docs.microsoft.com/en-us/sharepoint/dev/solution-guidance/security-apponly-azuread for a sample on how to get started.",
       SortOrder = 19)]
    [CmdletExample(
       Code = "PS:> Connect-PnPOnline -ClientId <id> -Certificate $cert -CertificatePassword (ConvertTo-SecureString -AsPlainText 'myprivatekeypassword' -Force) -Url https://contoso.sharepoint.com -Tenant 'contoso.onmicrosoft.com'",
       Remarks = "Connects using an Azure Active Directory registered application using a certificate instance containing a private key. See https://docs.microsoft.com/en-us/sharepoint/dev/solution-guidance/security-apponly-azuread for a sample on how to get started.",
       SortOrder = 20)]

#endif
    public class ConnectOnline : BasePSCmdlet
    {
        private const string ParameterSet_MAIN = "Main";
        private const string ParameterSet_TOKEN = "Token";
        private const string ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL = "App-Only using a clientId and clientSecret and an URL";
        private const string ParameterSet_APPONLYCLIENTIDCLIENTSECRETAADDOMAIN = "App-Only using a clientId and clientSecret and an AAD Domain";
        private const string ParameterSet_WEBLOGIN = "WebLogin";
        private const string ParameterSet_ADFSCERT = "ADFS with client Certificate";
        private const string ParameterSet_ADFSCREDENTIALS = "ADFS with user credentials";
#if !ONPREMISES
        private const string ParameterSet_NATIVEAAD = "Azure Active Directory";
        private const string ParameterSet_APPONLYAAD = "App-Only with Azure Active Directory";
        private const string ParameterSet_APPONLYAADPEM = "App-Only with Azure Active Directory using certificate as PEM strings";
        private const string ParameterSet_APPONLYAADCER = "App-Only with Azure Active Directory using X502 certificates";
        private const string ParameterSet_APPONLYAADThumb = "App-Only with Azure Active Directory using certificate from certificate store by thumbprint";
        private const string ParameterSet_SPOMANAGEMENT = "SPO Management Shell Credentials";
        private const string ParameterSet_DEVICELOGIN = "PnP Management Shell / DeviceLogin";
        private const string ParameterSet_GRAPHDEVICELOGIN = "PnP Management Shell to the Microsoft Graph";
        private const string ParameterSet_AADWITHSCOPE = "Azure Active Directory using Scopes";
        private const string ParameterSet_GRAPHWITHAAD = "Microsoft Graph using Azure Active Directory";
        private const string SPOManagementClientId = "9bc3ab49-b65d-410a-85ad-de819febfddc";
        private const string SPOManagementRedirectUri = "https://oauth.spops.microsoft.com/";
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
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETAADDOMAIN, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCERT, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCREDENTIALS, ValueFromPipeline = true, HelpMessage = "Returns the connection for use with the -Connection parameter on cmdlets.")]
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

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_MAIN, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_TOKEN, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_WEBLOGIN, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_ADFSCERT, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_ADFSCREDENTIALS, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
#if !ONPREMISES
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_NATIVEAAD, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_APPONLYAAD, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_APPONLYAADPEM, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_APPONLYAADCER, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_APPONLYAADThumb, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_SPOMANAGEMENT, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
        [Parameter(Mandatory = false, Position = 0, ParameterSetName = ParameterSet_ACCESSTOKEN, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_DEVICELOGIN, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
#endif
#if ONPREMISES
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_HIGHTRUST_PFX, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet_HIGHTRUST_CERT, ValueFromPipeline = true, HelpMessage = "The Url of the site collection to connect to")]
#endif
        public string Url;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "Credentials of the user to connect with. Either specify a PSCredential object or a string. In case of a string value a lookup will be done to the Generic Credentials section of the Windows Credentials in the Windows Credential Manager for the correct credentials.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCREDENTIALS, HelpMessage = "Credentials of the user to connect with. Either specify a PSCredential object or a string. In case of a string value a lookup will be done to the Generic Credentials section of the Windows Credentials in the Windows Credential Manager for the correct credentials.")]
#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_AADWITHSCOPE, HelpMessage = "Credentials of the user to connect with. Either specify a PSCredential object or a string. In case of a string value a lookup will be done to the Generic Credentials section of the Windows Credentials in the Windows " +
            "Credential Manager for the correct credentials.")]
#endif
        public CredentialPipeBind Credentials;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "If you want to connect with the current user credentials")]
        public SwitchParameter CurrentCredentials;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCREDENTIALS, HelpMessage = "If you want to connect to SharePoint using ADFS and credentials")]
        public SwitchParameter UseAdfs;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCERT, HelpMessage = "If you want to connect to SharePoint farm using ADFS with a client certificate")]
        public SwitchParameter UseAdfsCert;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCERT, HelpMessage = "The client certificate which you want to use for the ADFS authentication")]
        public X509Certificate2 ClientCertificate;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCREDENTIALS, HelpMessage = "Authenticate using Kerberos to ADFS")]
        public SwitchParameter Kerberos;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCERT, HelpMessage = "The name of the ADFS trusted login provider")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCREDENTIALS, HelpMessage = "The name of the ADFS trusted login provider")]
        public string LoginProviderName;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "Specifies a minimal server healthscore before any requests are executed")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "Specifies a minimal server healthscore before any requests are executed")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, HelpMessage = "Specifies a minimal server healthscore before any requests are executed")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETAADDOMAIN, HelpMessage = "Specifies a minimal server healthscore before any requests are executed")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "Specifies a minimal server healthscore before any requests are executed")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCERT, HelpMessage = "Specifies a minimal server healthscore before any requests are executed")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCREDENTIALS, HelpMessage = "Specifies a minimal server healthscore before any requests are executed")]
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
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "Specifies a minimal server healthscore before any requests are executed")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "Specifies a minimal server healthscore before any requests are executed")]
#endif
        public int MinimalHealthScore = -1;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETAADDOMAIN, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCERT, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCREDENTIALS, HelpMessage = "Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.")]
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
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETAADDOMAIN, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCERT, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCREDENTIALS, HelpMessage = "Defines how many seconds to wait before each retry. Default is 1 second.")]
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
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, HelpMessage = "The request timeout. Default is 1800000")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETAADDOMAIN, HelpMessage = "The request timeout. Default is 1800000")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "The request timeout. Default is 1800000")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCERT, HelpMessage = "The request timeout. Default is 1800000")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCREDENTIALS, HelpMessage = "The request timeout. Default is 1800000")]
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
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, HelpMessage = "Authentication realm. If not specified will be resolved from the url specified.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETAADDOMAIN, HelpMessage = "Authentication realm. If not specified will be resolved from the url specified.")]
        public string Realm;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "The Application Client ID to use.")]

#if !ONPREMISES
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_GRAPHWITHAAD, HelpMessage = "The client id of the app which gives you access to the Microsoft Graph API.")]
#endif
        [Obsolete("Use ClientId instead")]
        public string AppId;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "The Application Client Secret to use.")]
#if !ONPREMISES
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_GRAPHWITHAAD, HelpMessage = "The app key of the app which gives you access to the Microsoft Graph API.")]
#endif
        [Obsolete("Use ClientSecret instead")]
        public string AppSecret;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, HelpMessage = "The client secret to use.")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETAADDOMAIN, HelpMessage = "The client secret to use.")]

        public string ClientSecret;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "If you want to connect to SharePoint with browser based login. This is required when you have multi-factor authentication (MFA) enabled.")]
        public SwitchParameter UseWebLogin;

#if !PNPPSCORE
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "Specify to use for instance use forms based authentication (FBA)")]
        public ClientAuthenticationMode AuthenticationMode = ClientAuthenticationMode.Default;
#endif

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETAADDOMAIN, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCERT, HelpMessage = "If you want to create a PSDrive connected to the URL")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCREDENTIALS, HelpMessage = "If you want to create a PSDrive connected to the URL")]
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
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETAADDOMAIN, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCERT, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCREDENTIALS, HelpMessage = "Name of the PSDrive to create (default: SPO)")]
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
        [Alias("PnPO365ManagementShell")]
        public SwitchParameter PnPManagementShell;

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
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, HelpMessage = "The Client ID of the Azure AD Application")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETAADDOMAIN, HelpMessage = "The Client ID of the Azure AD Application")]
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

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "The thumbprint of the certificate containing the private key registered with the application in Azure Active Directory")]
        public string Thumbprint;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "Clears the token cache.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPOMANAGEMENT, HelpMessage = "Clears the token cache.")]
        public SwitchParameter ClearTokenCache;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, HelpMessage = "The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.")]
        public AzureEnvironment AzureEnvironment = AzureEnvironment.Production;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_AADWITHSCOPE, HelpMessage = "The array of permission scopes to request from Azure Active Directory")]
        public string[] Scopes;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_GRAPHWITHAAD, HelpMessage = "The AAD where the O365 app is registered. Eg.: contoso.com, or contoso.onmicrosoft.com.")]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETAADDOMAIN, HelpMessage = "The AAD where the O365 app is registered. Eg.: contoso.com, or contoso.onmicrosoft.com.")]
        public string AADDomain;

        [Parameter(Mandatory = true, ParameterSetName = ParameterSet_ACCESSTOKEN, HelpMessage = "Connect with an existing Access Token")]
        public string AccessToken;
#endif
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCERT, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCREDENTIALS, HelpMessage = "The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.")]
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
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCERT, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCREDENTIALS, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_SPOMANAGEMENT, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ACCESSTOKEN, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
#endif
#if ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "Should we skip the check if this site is the Tenant admin site. Default is false")]
#endif
        public SwitchParameter SkipTenantAdminCheck;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_MAIN, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_TOKEN, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_WEBLOGIN, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCERT, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_ADFSCREDENTIALS, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
#if !ONPREMISES
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_NATIVEAAD, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAAD, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADPEM, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADThumb, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_APPONLYAADCER, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_GRAPHWITHAAD, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting through a proxy to the Microsoft Graph API which has SSL interception enabled.")]
#if !PNPPSCORE
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_AADWITHSCOPE, HelpMessage = "Ignores any SSL errors. To be used i.e. when connecting through a proxy to the Microsoft Graph API which has SSL interception enabled.")]
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

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_CERT, HelpMessage = "Name of the user (login name) on whose behalf to create the access token. Supported input formats are SID and User Principal Name (UPN) in the format user@domain.local. If the parameter is not specified, an App Only Context is created.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet_HIGHTRUST_PFX, HelpMessage = "Name of the user (login name) on whose behalf to create the access token. Supported input formats are SID and User Principal Name (UPN) in the format user@domain.local. If the parameter is not specified, an App Only Context is created.")]
        [ValidateNotNullOrEmpty()]
        public string UserName;
#endif

        protected override void ProcessRecord()
        {
            try
            {
                Connect();
            }
            catch (Exception ex)
            {
                if (!ex.Data.Contains("TimeStampUtc"))
                {
                    ex.Data.Add("TimeStampUtc", DateTime.UtcNow);
                }
                else
                {
                    ex.Data["TimeStampUtc"] = DateTime.UtcNow;
                }
                throw ex;
            }
        }

        /// <summary>
        /// Sets up the connection using the information provided through the cmdlet arguments
        /// </summary>
        protected void Connect()
        {
            PnPConnection connection = null;

            var latestVersion = PnPConnectionHelper.GetLatestVersion();
            if (!string.IsNullOrEmpty(latestVersion))
            {
                WriteUpdateMessage(latestVersion);
            }

            if (IgnoreSslErrors)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            }

            PSCredential credentials = null;
            if (Credentials != null)
            {
                credentials = Credentials.Credential;
            }

            WriteVerbose($"Using parameter set '{ParameterSetName}'");

            // Connect using the used set parameters
            switch (ParameterSetName)
            {
#if !ONPREMISES
                case ParameterSet_GRAPHWITHAAD:
                    connection = ConnectGraphWithAad();
                    break;

                case ParameterSet_SPOMANAGEMENT:
                    connection = ConnectSpoManagement();
                    break;

                case ParameterSet_DEVICELOGIN:
                    connection = ConnectDeviceLogin();
                    break;

                case ParameterSet_GRAPHDEVICELOGIN:
                    connection = ConnectGraphDeviceLogin(null);
                    break;

                case ParameterSet_NATIVEAAD:
                    connection = ConnectNativeAAD(ClientId, RedirectUri);
                    break;

                case ParameterSet_APPONLYAAD:
                    connection = ConnectAppOnlyAad();
                    break;

                case ParameterSet_APPONLYAADPEM:
                    connection = ConnectAppOnlyAadPem();
                    break;

                case ParameterSet_APPONLYAADThumb:
                    connection = ConnectAppOnlyAadThumb();
                    break;

                case ParameterSet_APPONLYAADCER:
                    connection = ConnectAppOnlyAadCer();
                    break;

                case ParameterSet_AADWITHSCOPE:
                    connection = ConnectAadWithScope(credentials);
                    break;
                case ParameterSet_ACCESSTOKEN:
                    connection = ConnectAccessToken();
                    break;
#else
                case ParameterSet_HIGHTRUST_CERT:
                    connection = ConnectHighTrustCert();
                    break;

                case ParameterSet_HIGHTRUST_PFX:
                    connection = ConnectHighTrustPfx();
                    break;
#endif
                case ParameterSet_TOKEN:
                    connection = ConnectToken();
                    break;

                case ParameterSet_APPONLYCLIENTIDCLIENTSECRETURL:
                    connection = ConnectAppOnlyClientIdCClientSecretUrl();
                    break;

                case ParameterSet_APPONLYCLIENTIDCLIENTSECRETAADDOMAIN:
                    connection = ConnectAppOnlyClientIdCClientSecretAadDomain();
                    break;

                case ParameterSet_ADFSCERT:
                    connection = ConnectAdfsCertificate();
                    break;

                case ParameterSet_ADFSCREDENTIALS:
                    connection = ConnectAdfsCredentials(credentials);
                    break;

                case ParameterSet_MAIN:
                    connection = ConnectCredentials(credentials);
                    break;
            }

            if (UseWebLogin.IsPresent)
            {
                connection = ConnectWebLogin();
            }

            // Ensure a connection instance has been created by now
            if (connection == null)
            {
                // No connection instance was created
                throw new PSInvalidOperationException("Unable to connect using provided arguments");
            }

            // Connection has been established
            WriteVerbose($"PnP PowerShell Cmdlets ({Assembly.GetExecutingAssembly().GetName().Version}): Connected to {Url}");
            PnPConnection.CurrentConnection = connection;
            if (CreateDrive && PnPConnection.CurrentConnection.Context != null)
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

            if (PnPConnection.CurrentConnection.Url != null)
            {
                var hostUri = new Uri(PnPConnection.CurrentConnection.Url);
                Environment.SetEnvironmentVariable("PNPPSHOST", hostUri.Host);
                Environment.SetEnvironmentVariable("PNPPSSITE", hostUri.LocalPath);
            }
            else
            {
                Environment.SetEnvironmentVariable("PNPPSHOST", "GRAPH");
                Environment.SetEnvironmentVariable("PNPPSSITE", "GRAPH");
            }

            if (ReturnConnection)
            {
                WriteObject(connection);
            }
        }

        #region Connect Types

        /// <summary>
        /// Connect using the paramater set TOKEN
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectToken()
        {
#if !ONPREMISES
#pragma warning disable CS0618 // Type or member is obsolete
            return PnPConnectionHelper.InstantiateSPOnlineConnection(new Uri(Url), AADDomain, AppId, AppSecret, Host, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, SkipTenantAdminCheck, false, AzureEnvironment);
#pragma warning restore CS0618 // Type or member is obsolete
#else
#pragma warning disable CS0618 // Type or member is obsolete
            return PnPConnectionHelper.InstantiateSPOnlineConnection(new Uri(Url), null, AppId, AppSecret, Host, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, SkipTenantAdminCheck, false);
#pragma warning restore CS0618 // Type or member is obsolete
#endif
        }

        /// <summary>
        /// Connect using the parameter set GRAPHWITHAAD
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectGraphWithAad()
        {
#if !ONPREMISES
#pragma warning disable CS0618 // Type or member is obsolete
            return PnPConnection.GetConnectionWithClientIdAndClientSecret(AppId, AppSecret, Host, InitializationType.AADAppOnly, Url, AADDomain, disableTelemetry: NoTelemetry);
#pragma warning restore CS0618 // Type or member is obsolete
#else
            return null;
#endif
        }

        /// <summary>
        /// Connect using the parameter set APPONLYCLIENTIDCLIENTSECRETURL
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectAppOnlyClientIdCClientSecretUrl()
        {
#if !ONPREMISES
            return PnPConnectionHelper.InstantiateSPOnlineConnection(new Uri(Url), AADDomain, ClientId, ClientSecret, Host, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, SkipTenantAdminCheck, false, AzureEnvironment);
#else
            return PnPConnectionHelper.InstantiateSPOnlineConnection(new Uri(Url), null, ClientId, ClientSecret, Host, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, SkipTenantAdminCheck, false);
#endif
        }

        /// <summary>
        /// Connect using the parameter set APPONLYCLIENTIDCLIENTSECRETAADDOMAIN
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectAppOnlyClientIdCClientSecretAadDomain()
        {
#if !ONPREMISES
            return PnPConnection.GetConnectionWithClientIdAndClientSecret(ClientId, ClientSecret, Host, InitializationType.AADAppOnly, Url, AADDomain, disableTelemetry: NoTelemetry);
#else
            return PnPConnectionHelper.InstantiateSPOnlineConnection(new Uri(Url), null, ClientId, ClientSecret, Host, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, SkipTenantAdminCheck, false);
#endif
        }

        /// <summary>
        /// Connect using the parameter set SPOMANAGEMENT
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectSpoManagement()
        {
#if !ONPREMISES
            return ConnectNativeAAD(SPOManagementClientId, SPOManagementRedirectUri);
#else
            return null;
#endif
        }

        /// <summary>
        /// Connect using the parameter set DEVICELOGIN
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectDeviceLogin()
        {
#if !ONPREMISES
            bool ctrlCAsInput = false;
            if (Host.Name == "ConsoleHost")
            {
                ctrlCAsInput = Console.TreatControlCAsInput;
                Console.TreatControlCAsInput = true;
            }

            var uri = new Uri(Url);
            if ($"https://{uri.Host}".Equals(Url.ToLower()))
            {
                Url += "/";
            }
            var connection = PnPConnectionHelper.InstantiateDeviceLoginConnection(Url, LaunchBrowser, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry);

            if (Host.Name == "ConsoleHost")
            {
                Console.TreatControlCAsInput = ctrlCAsInput;
            }
            return connection;
#else
            return null;
#endif
        }

        /// <summary>
        /// Connect using the parameter set GRAPHDEVICELOGIN
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectGraphDeviceLogin(string accessToken)
        {
#if !ONPREMISES
            if (string.IsNullOrEmpty(accessToken))
            {
                bool ctrlCAsInput = false;
                if (Host.Name == "ConsoleHost")
                {
                    ctrlCAsInput = Console.TreatControlCAsInput;
                    Console.TreatControlCAsInput = true;
                }

                var connection = PnPConnectionHelper.InstantiateGraphDeviceLoginConnection(LaunchBrowser, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, (message) =>
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
                if (Host.Name == "ConsoleHost")
                {
                    Console.TreatControlCAsInput = ctrlCAsInput;
                }
                return connection;
            }
            else
            {
                // TODO KZ: GetConnectionWithToken?
                return PnPConnectionHelper.InstantiateGraphAccessTokenConnection(accessToken, Host, NoTelemetry);
            }
#else
            return null;
#endif
        }

        /// <summary>
        /// Connect using the parameter set NativeAAD
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectNativeAAD(string clientId, string redirectUrl)
        {
#if !ONPREMISES
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string configFolder = Path.Combine(appDataFolder, "PnP.PowerShell");
            Directory.CreateDirectory(configFolder); // Ensure folder exists
            if (ClearTokenCache)
            {
                string configFile = Path.Combine(configFolder, "tokencache.dat");

                if (File.Exists(configFile))
                {
                    File.Delete(configFile);
                }
            }
#if !PNPPSCORE
            return PnPConnectionHelper.InitiateAzureADNativeApplicationConnection(
                new Uri(Url), clientId, new Uri(redirectUrl), MinimalHealthScore, RetryCount,
                RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment);
#else
            throw new NotImplementedException();
#endif
#else
            return null;
#endif
        }

        /// <summary>
        /// Connect using the parameter set APPONLYAAD
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectAppOnlyAad()
        {
#if !ONPREMISES
#if !PNPPSCORE
            if (ParameterSpecified(nameof(CertificatePath)))
            {
                WriteWarning(@"Your certificate is copied by the operating system to c:\ProgramData\Microsoft\Crypto\RSA\MachineKeys. Over time this folder may increase heavily in size. Use Disconnect-PnPOnline in your scripts remove the certificate from this folder to clean up. Consider using -Thumbprint instead of -CertificatePath.");
                return PnPConnectionHelper.InitiateAzureADAppOnlyConnection(new Uri(Url), ClientId, Tenant, CertificatePath, CertificatePassword, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment);
            }
            else if (ParameterSpecified(nameof(Certificate)))
            {
                return PnPConnectionHelper.InitiateAzureAdAppOnlyConnectionWithCert(new Uri(Url), ClientId, Tenant, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment, Certificate);
            }
            else if (ParameterSpecified(nameof(CertificateBase64Encoded)))
            {
                return PnPConnectionHelper.InitiateAzureAdAppOnlyConnectionWithCert(new Uri(Url), ClientId, Tenant, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment, CertificateBase64Encoded);
            }
            else
            {
                throw new ArgumentException("You must either provide CertificatePath, Certificate or CertificateBase64Encoded when connecting using an Azure Active Directory registered application");
            }
#else
            throw new NotImplementedException();
#endif
#else
            return null;
#endif
        }

        /// <summary>
        /// Connect using the parameter set APPONLYAADPEM
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectAppOnlyAadPem()
        {
#if !ONPREMISES
#if !PNPPSCORE
            return PnPConnectionHelper.InitiateAzureADAppOnlyConnection(new Uri(Url), ClientId, Tenant, PEMCertificate, PEMPrivateKey, CertificatePassword, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment);
#else
            return PnPConnectionHelper.InitiateAzureADAppOnlyConnection(new Uri(Url), ClientId, Tenant, PEMCertificate, PEMPrivateKey, CertificatePassword, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment);
#endif
#else
            return null;
#endif
        }

        /// <summary>
        /// Connect using the parameter set APPONLYAADThumb
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectAppOnlyAadThumb()
        {
#if !ONPREMISES
#if !PNPPSCORE
            return PnPConnectionHelper.InitiateAzureADAppOnlyConnection(new Uri(Url), ClientId, Tenant, Thumbprint, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment);
#else
            return PnPConnectionHelper.InitiateAzureADAppOnlyConnection(new Uri(Url), ClientId, Tenant, Thumbprint, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment);
#endif
#else
            return null;
#endif
        }

        /// <summary>
        /// Connect using the parameter set APPONLYAADCER
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectAppOnlyAadCer()
        {
#if !ONPREMISES
#if !PNPPSCORE
            return PnPConnectionHelper.InitiateAzureADAppOnlyConnection(new Uri(Url), ClientId, Tenant, Certificate, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, NoTelemetry, SkipTenantAdminCheck, AzureEnvironment);
#else
            throw new NotImplementedException();
#endif
#else
            return null;
#endif
        }

        /// <summary>
        /// Connect using the parameter set AADWITHSCOPE
        /// </summary>
        /// <param name="credentials">Credentials to authenticate with for delegated access or NULL for application permissions</param>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectAadWithScope(PSCredential credentials)
        {
#if !ONPREMISES
            // Filter out the scopes for the Microsoft Office 365 Management API
            var officeManagementApiScopes = Enum.GetNames(typeof(OfficeManagementApiPermission)).Select(s => s.Replace("_", ".")).Intersect(Scopes).ToArray();

            // Take the remaining scopes and try requesting them from the Microsoft Graph API
            var graphScopes = Scopes.Except(officeManagementApiScopes).ToArray();

            PnPConnection connection = null;

            // If we have Office 365 scopes, get a token for those first
            if (officeManagementApiScopes.Length > 0)
            {
#if !PNPPSCORE
                //if (credentials == null)
                //{
                //    TokenManager.InitializeAsync(TokenManager.CLIENTID_PNPMANAGEMENTSHELL, officeManagementApiScopes.Select(s => $"https://manage.office.com/{s}").ToArray(), cacheIdentifierName: "OfficeManagementApi").GetAwaiter().GetResult();
                //}
                //else
                //{
                //    TokenManager.InitializeAsync(TokenManager.CLIENTID_PNPMANAGEMENTSHELL, officeManagementApiScopes.Select(s => $"https://manage.office.com/{s}").ToArray(), credentials.UserName, credentials.Password, cacheIdentifierName: "OfficeManagementApi").GetAwaiter().GetResult();
                //}

                var officeManagementApiToken = credentials == null ? OfficeManagementApiToken.AcquireApplicationTokenInteractive(PnPConnection.PnPManagementShellClientId, officeManagementApiScopes) : OfficeManagementApiToken.AcquireDelegatedTokenWithCredentials(PnPConnection.PnPManagementShellClientId, officeManagementApiScopes, credentials.UserName, credentials.Password);
#else
                var officeManagementApiToken = credentials == null ? OfficeManagementApiToken.AcquireApplicationTokenDeviceLogin(PnPConnection.PnPManagementShellClientId, officeManagementApiScopes, PnPConnection.DeviceLoginCallback(this.Host, true)) : OfficeManagementApiToken.AcquireDelegatedTokenWithCredentials(PnPConnection.PnPManagementShellClientId, officeManagementApiScopes, credentials.UserName, credentials.Password);
#endif
                connection = PnPConnection.GetConnectionWithToken(officeManagementApiToken, TokenAudience.OfficeManagementApi, Host, InitializationType.InteractiveLogin, credentials, disableTelemetry: NoTelemetry.ToBool());
            }

            // If we have Graph scopes, get a token for it
            if (graphScopes.Length > 0)
            {
#if !PNPPSCORE
                var graphToken = credentials == null ? GraphToken.AcquireApplicationTokenInteractive(PnPConnection.PnPManagementShellClientId, graphScopes) : GraphToken.AcquireDelegatedTokenWithCredentials(PnPConnection.PnPManagementShellClientId, graphScopes, credentials.UserName, credentials.Password);
#else
                var graphToken = credentials == null ? GraphToken.AcquireApplicationTokenDeviceLogin(PnPConnection.PnPManagementShellClientId, graphScopes, PnPConnection.DeviceLoginCallback(this.Host, true)) : GraphToken.AcquireDelegatedTokenWithCredentials(PnPConnection.PnPManagementShellClientId, graphScopes, credentials.UserName, credentials.Password);
#endif
                // If there's a connection already, add the AAD token to it, otherwise set up a new connection with it
                if (connection != null)
                {
                    //connection.AddToken(TokenAudience.MicrosoftGraph, graphToken);
                }
                else
                {
                    connection = PnPConnection.GetConnectionWithToken(graphToken, TokenAudience.MicrosoftGraph, Host, InitializationType.InteractiveLogin, credentials, disableTelemetry: NoTelemetry.ToBool());
                }
            }
            connection.Scopes = Scopes;
            return connection;
#else
                return null;
#endif
        }

        /// <summary>
        /// Connect using the parameter set ACCESSTOKEN
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectAccessToken()
        {
#if !ONPREMISES
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(AccessToken);
            var aud = jwtToken.Audiences.FirstOrDefault();
            var url = Url ?? aud ?? throw new PSArgumentException(Resources.AccessTokenConnectFailed);

            switch (url.ToLower())
            {
                case GraphToken.ResourceIdentifier:
                    return PnPConnection.GetConnectionWithToken(new GraphToken(AccessToken), TokenAudience.MicrosoftGraph, Host, InitializationType.Token, null, disableTelemetry: NoTelemetry.ToBool());

                case OfficeManagementApiToken.ResourceIdentifier:
                    return PnPConnection.GetConnectionWithToken(new OfficeManagementApiToken(AccessToken), TokenAudience.OfficeManagementApi, Host, InitializationType.Token, null, disableTelemetry: NoTelemetry.ToBool());

                default:
                    return PnPConnection.GetConnectionWithToken(new SharePointToken(AccessToken), TokenAudience.SharePointOnline, Host, InitializationType.Token, null, Url, disableTelemetry: NoTelemetry.ToBool());
            }
#else
            return null;
#endif
        }

        /// <summary>
        /// Connect using the parameter set HIGHTRUST_CERT
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectHighTrustCert()
        {
#if !ONPREMISES
            return null;
#else
            return PnPConnectionHelper.InstantiateHighTrustConnection(Url,
                                                                      ClientId,
                                                                      HighTrustCertificate,
                                                                      HighTrustCertificateIssuerId ?? ClientId,
                                                                      MinimalHealthScore,
                                                                      RetryCount,
                                                                      RetryWait,
                                                                      RequestTimeout,
                                                                      TenantAdminUrl,
                                                                      Host,
                                                                      NoTelemetry,
                                                                      SkipTenantAdminCheck,
                                                                      UserName);
#endif
        }

        /// <summary>
        /// Connect using the parameter set HIGHTRUST_PFX
        /// </summary>
        /// <returns>PnPConnection based on the parameters provided in the parameter set</returns>
        private PnPConnection ConnectHighTrustPfx()
        {
#if !ONPREMISES
            return null;
#else
            return PnPConnectionHelper.InstantiateHighTrustConnection(Url,
                                                                      ClientId,
                                                                      HighTrustCertificatePath,
                                                                      HighTrustCertificatePassword,
                                                                      HighTrustCertificateIssuerId ?? ClientId,
                                                                      MinimalHealthScore,
                                                                      RetryCount,
                                                                      RetryWait,
                                                                      RequestTimeout,
                                                                      TenantAdminUrl,
                                                                      Host,
                                                                      NoTelemetry,
                                                                      SkipTenantAdminCheck,
                                                                      UserName);
#endif
        }

        /// <summary>
        /// Connect using ADFS using client credentials
        /// </summary>
        /// <param name="credentials">Credentials to use to authenticate to ADFS</param>
        /// <returns>PnPConnection based on ADFS authentication</returns>
        private PnPConnection ConnectAdfsCredentials(PSCredential credentials)
        {
            if (!Kerberos && credentials == null)
            {
                if ((credentials = GetCredentials()) == null)
                {
                    credentials = Host.UI.PromptForCredential(Resources.EnterYourCredentials, "", "", "");

                    // Ensure credentials have been entered
                    if (credentials == null)
                    {
                        // No credentials have been provided
                        return null;
                    }
                }
            }
#if !PNPPSCORE
            return PnPConnectionHelper.InstantiateAdfsConnection(new Uri(Url),
                                                                 Kerberos,
                                                                 credentials,
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

        /// <summary>
        /// Connect using ADFS Client Certificate
        /// </summary>
        /// <returns>PnPConnection based on ADFS Client Certificate authentication</returns>
        private PnPConnection ConnectAdfsCertificate()
        {
#if !PNPPSCORE
            // Check if we already have a client certificate, if not, ask for selecting one
            if (ClientCertificate == null)
            {
                // Modal Dialog to enable a user to select a certificate to use to authenticate against ADFS
                X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                var certs = X509Certificate2UI.SelectFromCollection(store.Certificates, "Select ADFS User Certificate", "Selec the certificate to use to authenticate to ADFS", X509SelectionFlag.SingleSelection);

                // Ensure a certificate has been chosen
                if (certs == null || certs.Count == 0 || certs[0] == null)
                {
                    // No certificate has been chosen
                    return null;
                }

                ClientCertificate = certs[0];
            }

            if (ClientCertificate != null)
            {
                var serialNumber = ClientCertificate.SerialNumber;
                try
                {
                    return PnPConnectionHelper.InstantiateAdfsCertificateConnection(new Uri(Url), serialNumber, Host, MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, SkipTenantAdminCheck);
                }
                catch (TargetInvocationException e) when (e.InnerException != null && e.InnerException is CryptographicException)
                {
                    throw new PSArgumentException(Resources.ClientCertificateInvalid, e);
                }
            }

            return null;
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// Connect using WebLogin
        /// </summary>
        /// <returns>PnPConnection based on WebLogin authentication</returns>
        private PnPConnection ConnectWebLogin()
        {
#if !PNPPSCORE
            return PnPConnectionHelper.InstantiateWebloginConnection(new Uri(Url.ToLower()), MinimalHealthScore, RetryCount, RetryWait, RequestTimeout, TenantAdminUrl, Host, SkipTenantAdminCheck);
#else
            WriteWarning(@"-UseWebLogin is not implemented, due to restrictions of the .NET Standard framework. Use -PnPO365ManagementShell instead");
            return null;
#endif
        }

        /// <summary>
        /// Connect using provided credentials or the current credentials
        /// </summary>
        /// <returns>PnPConnection based on credentials authentication</returns>
        private PnPConnection ConnectCredentials(PSCredential credentials)
        {
            if (!CurrentCredentials && credentials == null)
            {
                credentials = GetCredentials();
                if (credentials == null)
                {
                    credentials = Host.UI.PromptForCredential(Resources.EnterYourCredentials, "", "", "");

                    // Ensure credentials have been entered
                    if (credentials == null)
                    {
                        // No credentials have been provided
                        return null;
                    }
                }
            }

#if !PNPPSCORE
            return PnPConnectionHelper.InstantiateSPOnlineConnection(new Uri(Url),
                                                                     credentials,
                                                                     Host,
                                                                     CurrentCredentials,
                                                                     MinimalHealthScore,
                                                                     RetryCount,
                                                                     RetryWait,
                                                                     RequestTimeout,
                                                                     TenantAdminUrl,
                                                                     NoTelemetry,
                                                                     SkipTenantAdminCheck,
                                                                     AuthenticationMode);
#else
            return PnPConnectionHelper.InstantiateSPOnlineConnection(new Uri(Url),
                                                               credentials,
                                                               Host,
                                                               CurrentCredentials,
                                                               MinimalHealthScore,
                                                               RetryCount,
                                                               RetryWait,
                                                               RequestTimeout,
                                                               TenantAdminUrl,
                                                               NoTelemetry,
                                                               SkipTenantAdminCheck);
#endif
        }

        #endregion

        #region Helper methods
        private PSCredential GetCredentials()
        {
            var connectionUri = new Uri(Url);

            // Try to get the credentials by full url
            PSCredential credentials = Utilities.CredentialManager.GetCredential(Url);
            if (credentials == null)
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
                        credentials = Utilities.CredentialManager.GetCredential(pathUrl);
                        if (credentials != null)
                        {
                            break;
                        }
                    }
                }

                if (credentials == null)
                {
                    // Try to find the credentials by schema and hostname
                    credentials = Utilities.CredentialManager.GetCredential(connectionUri.Scheme + "://" + connectionUri.Host);

                    if (credentials == null)
                    {
                        // try to find the credentials by hostname
                        credentials = Utilities.CredentialManager.GetCredential(connectionUri.Host);
                    }
                }

            }

            return credentials;
        }

        private void WriteUpdateMessage(string message)
        {

            if (Host.Name == "ConsoleHost")
            {
                // Use Warning Color
                var notificationColor = "\x1B[7m";
                var resetColor = "\x1B[0m";

                var lineLength = 0;
                foreach (var line in message.Split('\n'))
                {
                    if (line.Length > lineLength)
                    {
                        lineLength = line.Length;
                    }
                }
                var outMessage = string.Empty;
                foreach (var line in message.Split('\n'))
                {
                    var lineToAdd = line.PadRight(lineLength);
                    outMessage += $"{notificationColor} {lineToAdd} {resetColor}\n";
                }
                Host.UI.WriteLine(outMessage);
            }
            else
            {
                WriteWarning(message);
            }
        }
        #endregion
    }
}
