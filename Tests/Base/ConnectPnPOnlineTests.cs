using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Base
{
    [TestClass]
    public class ConnectOnlineTests
    {
        #region Test Setup/CleanUp
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            // This runs on class level once before all tests run
            //using (var ctx = TestCommon.CreateClientContext())
            //{
            //}
        }

        [ClassCleanup]
        public static void Cleanup(TestContext testContext)
        {
            // This runs on class level once
            //using (var ctx = TestCommon.CreateClientContext())
            //{
            //}
        }

        [TestInitialize]
        public void Initialize()
        {
            using (var scope = new PSTestScope())
            {
                // Example
                // scope.ExecuteCommand("cmdlet", new CommandParameter("param1", prop));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var scope = new PSTestScope())
            {
                try
                {
                    // Do Test Setup - Note, this runs PER test
                }
                catch (Exception)
                {
                    // Describe Exception
                }
            }
        }
        #endregion

        #region Scaffolded Cmdlet Tests
        //TODO: This is a scaffold of the cmdlet - complete the unit test
        //[TestMethod]
        public void ConnectPnPOnlineTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Returns the connection for use with the -Connection parameter on cmdlets.
				var returnConnection = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Url of the site collection to connect to
				var url = "";
				// From Cmdlet Help: Credentials of the user to connect with. Either specify a PSCredential object or a string. In case of a string value a lookup will be done to the Generic Credentials section of the Windows Credentials in the Windows Credential Manager for the correct credentials.
				var credentials = "";
				// From Cmdlet Help: If you want to connect with the current user credentials
				var currentCredentials = "";
				// From Cmdlet Help: If you want to connect to SharePoint using ADFS and credentials
				var useAdfs = "";
				// From Cmdlet Help: If you want to connect to SharePoint farm using ADFS with a client certificate
				var useAdfsCert = "";
				// From Cmdlet Help: The client certificate which you want to use for the ADFS authentication
				var clientCertificate = "";
				// From Cmdlet Help: Authenticate using Kerberos to ADFS
				var kerberos = "";
				// From Cmdlet Help: The name of the ADFS trusted login provider
				var loginProviderName = "";
				// From Cmdlet Help: Specifies a minimal server healthscore before any requests are executed
				var minimalHealthScore = "";
				// From Cmdlet Help: Defines how often a retry should be executed if the server healthscore is not sufficient. Default is 10 times.
				var retryCount = "";
				// From Cmdlet Help: Defines how many seconds to wait before each retry. Default is 1 second.
				var retryWait = "";
				// From Cmdlet Help: The request timeout. Default is 1800000
				var requestTimeoutVar = "";
				// From Cmdlet Help: Authentication realm. If not specified will be resolved from the url specified.
				var realm = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Application Client ID to use.
				var appId = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Application Client Secret to use.
				var appSecret = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The client secret to use.
				var clientSecret = "";
				// This is a mandatory parameter
				// From Cmdlet Help: If you want to connect to SharePoint with browser based login. This is required when you have multi-factor authentication (MFA) enabled.
				var useWebLogin = "";
				// From Cmdlet Help: Specify to use for instance use forms based authentication (FBA)
				var authenticationMode = "";
				// From Cmdlet Help: If you want to create a PSDrive connected to the URL
				var createDrive = "";
				// From Cmdlet Help: Name of the PSDrive to create (default: SPO)
				var driveName = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Log in using the SharePoint Online Management Shell application
				var sPOManagementShell = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Log in using the PnP O365 Management Shell application. You will be asked to consent to:* Read and write managed metadata
				// * Have full control of all site collections
				// * Read user profiles
				// * Invite guest users to the organization
				// * Read and write all groups
				// * Read and write directory data
				// * Read and write identity providers
				// * Access the directory as you
				var pnPO365ManagementShell = "";
				// From Cmdlet Help: Launch a browser automatically and copy the code to enter to the clipboard
				var launchBrowser = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Log in using the PnP O365 Management Shell application towards the Graph. You will be asked to consent to:* Read and write managed metadata
				// * Have full control of all site collections
				// * Read user profiles
				// * Invite guest users to the organization
				// * Read and write all groups
				// * Read and write directory data
				// * Read and write identity providers
				// * Access the directory as you
				// 
				var graph = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Client ID of the Azure AD Application
				var clientId = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Redirect URI of the Azure AD Application
				var redirectUri = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Azure AD Tenant name,e.g. mycompany.onmicrosoft.com
				var tenant = "";
				// From Cmdlet Help: Path to the certificate containing the private key (*.pfx)
				var certificatePath = "";
				// From Cmdlet Help: Base64 Encoded X509Certificate2 certificate containing the private key to authenticate the requests to SharePoint Online such as retrieved in Azure Functions from Azure KeyVault
				var certificateBase64Encoded = "";
				// From Cmdlet Help: X509Certificate2 reference containing the private key to authenticate the requests to SharePoint Online
				var certificate = "";
				// From Cmdlet Help: Password to the certificate (*.pfx)
				var certificatePassword = "";
				// This is a mandatory parameter
				// From Cmdlet Help: PEM encoded certificate
				var pEMCertificate = "";
				// This is a mandatory parameter
				// From Cmdlet Help: PEM encoded private key for the certificate
				var pEMPrivateKey = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Certificate thumbprint
				var thumbprint = "";
				// From Cmdlet Help: Clears the token cache.
				var clearTokenCache = "";
				// From Cmdlet Help: The Azure environment to use for authentication, the defaults to 'Production' which is the main Azure environment.
				var azureEnvironment = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The array of permission scopes for the Microsoft Graph API.
				var scopeVars = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The AAD where the O365 app is registered. Eg.: contoso.com, or contoso.onmicrosoft.com.
				var aADDomain = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Connect with an existing Access Token
				var accessToken = "";
				// From Cmdlet Help: The url to the Tenant Admin site. If not specified, the cmdlets will assume to connect automatically to https://<tenantname>-admin.sharepoint.com where appropriate.
				var tenantAdminUrl = "";
				// From Cmdlet Help: Should we skip the check if this site is the Tenant admin site. Default is false
				var skipTenantAdminCheck = "";
				// From Cmdlet Help: Ignores any SSL errors. To be used i.e. when connecting to a SharePoint farm using self signed certificates or using a certificate authority not trusted by this machine.
				var ignoreSslErrors = "";
				// From Cmdlet Help: In order to help to make PnP PowerShell better, we can track anonymous telemetry. We track the version of the cmdlets you are using, which cmdlet you are executing and which version of SharePoint you are connecting to. Use Disable-PnPPowerShellTelemetry to turn this off in general or use the -NoTelemetry switch to turn it off for that session.
				var noTelemetry = "";

                var results = scope.ExecuteCommand("Connect-PnPOnline",
					new CommandParameter("ReturnConnection", returnConnection),
					new CommandParameter("Url", url),
					new CommandParameter("Credentials", credentials),
					new CommandParameter("CurrentCredentials", currentCredentials),
					new CommandParameter("UseAdfs", useAdfs),
					new CommandParameter("UseAdfsCert", useAdfsCert),
					new CommandParameter("ClientCertificate", clientCertificate),
					new CommandParameter("Kerberos", kerberos),
					new CommandParameter("LoginProviderName", loginProviderName),
					new CommandParameter("MinimalHealthScore", minimalHealthScore),
					new CommandParameter("RetryCount", retryCount),
					new CommandParameter("RetryWait", retryWait),
					new CommandParameter("RequestTimeout", requestTimeoutVar),
					new CommandParameter("Realm", realm),
					new CommandParameter("AppId", appId),
					new CommandParameter("AppSecret", appSecret),
					new CommandParameter("ClientSecret", clientSecret),
					new CommandParameter("UseWebLogin", useWebLogin),
					new CommandParameter("AuthenticationMode", authenticationMode),
					new CommandParameter("CreateDrive", createDrive),
					new CommandParameter("DriveName", driveName),
					new CommandParameter("SPOManagementShell", sPOManagementShell),
					new CommandParameter("PnPO365ManagementShell", pnPO365ManagementShell),
					new CommandParameter("LaunchBrowser", launchBrowser),
					new CommandParameter("Graph", graph),
					new CommandParameter("ClientId", clientId),
					new CommandParameter("RedirectUri", redirectUri),
					new CommandParameter("Tenant", tenant),
					new CommandParameter("CertificatePath", certificatePath),
					new CommandParameter("CertificateBase64Encoded", certificateBase64Encoded),
					new CommandParameter("Certificate", certificate),
					new CommandParameter("CertificatePassword", certificatePassword),
					new CommandParameter("PEMCertificate", pEMCertificate),
					new CommandParameter("PEMPrivateKey", pEMPrivateKey),
					new CommandParameter("Thumbprint", thumbprint),
					new CommandParameter("ClearTokenCache", clearTokenCache),
					new CommandParameter("AzureEnvironment", azureEnvironment),
					new CommandParameter("Scopes", scopeVars),
					new CommandParameter("AADDomain", aADDomain),
					new CommandParameter("AccessToken", accessToken),
					new CommandParameter("TenantAdminUrl", tenantAdminUrl),
					new CommandParameter("SkipTenantAdminCheck", skipTenantAdminCheck),
					new CommandParameter("IgnoreSslErrors", ignoreSslErrors),
					new CommandParameter("NoTelemetry", noTelemetry));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            