#if !NETSTANDARD2_1
using Microsoft.IdentityModel.Clients.ActiveDirectory;
#endif
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.Commands.Enums;
using System;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Net;
using OfficeDevPnP.Core;
using OfficeDevPnP.Core.Utilities;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using SharePointPnP.PowerShell.Commands.Utilities;
using SharePointPnP.PowerShell.Commands.Model;
using System.Security.Cryptography.X509Certificates;
using System.Security;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Graph;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.Base
{
    internal class PnPConnectionHelper
    {

#if DEBUG
        private static readonly Uri VersionCheckUrl = new Uri("https://raw.githubusercontent.com/SharePoint/PnP-PowerShell/dev/version.txt");
#else
        private static readonly Uri VersionCheckUrl = new Uri("https://raw.githubusercontent.com/SharePoint/PnP-PowerShell/master/version.txt");
#endif
        private static bool VersionChecked;

#if !NETSTANDARD2_1
        public static AuthenticationContext AuthContext { get; set; }
#endif
        static PnPConnectionHelper()
        {
        }

        internal static PnPConnection InstantiateSPOnlineConnection(Uri url, string realm, string clientId, string clientSecret, PSHost host, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool disableTelemetry, bool skipAdminCheck = false, AzureEnvironment azureEnvironment = AzureEnvironment.Production)
        {
            ConnectionType connectionType;
            PnPClientContext context = null;

            if (url != null)
            {
                using (var authManager = new OfficeDevPnP.Core.AuthenticationManager())
                {
                    if (realm == null)
                    {
                        realm = GetRealmFromTargetUrl(url);
                    }

                    if (url.DnsSafeHost.Contains("spoppe.com"))
                    {
                        context = PnPClientContext.ConvertFrom(authManager.GetAppOnlyAuthenticatedContext(url.ToString(), realm, clientId, clientSecret, acsHostUrl: "windows-ppe.net", globalEndPointPrefix: "login"), retryCount, retryWait * 1000);
                    }
                    else
                    {
                        context = PnPClientContext.ConvertFrom(authManager.GetAppOnlyAuthenticatedContext(url.ToString(), realm, clientId, clientSecret, acsHostUrl: authManager.GetAzureADACSEndPoint(azureEnvironment), globalEndPointPrefix: authManager.GetAzureADACSEndPointPrefix(azureEnvironment)), retryCount, retryWait * 1000);
                    }
                    context.ApplicationName = Resources.ApplicationName;
                    context.RequestTimeout = requestTimeout;
#if !SP2013
                    context.DisableReturnValueCache = true;
#endif
                    connectionType = ConnectionType.OnPrem;
                    if (url.Host.ToUpperInvariant().EndsWith("SHAREPOINT.COM"))
                    {
                        connectionType = ConnectionType.O365;
                    }
                    if (skipAdminCheck == false)
                    {
                        if (IsTenantAdminSite(context))
                        {
                            connectionType = ConnectionType.TenantAdmin;
                        }
                    }
                }
            }
            else
            {
                connectionType = ConnectionType.O365;
            }

            var spoConnection = new PnPConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, clientId, clientSecret, url?.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.SPClientSecret)
            {
                Tenant = realm
            };

            return spoConnection;
        }

#if !NETSTANDARD2_1
#if ONPREMISES
        internal static PnPConnection InstantiateHighTrustConnection(string url, string clientId, string hightrustCertificatePath, string hightrustCertificatePassword, string hightrustCertificateIssuerId, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck, string loginName)
        {
            using (var authManager = new OfficeDevPnP.Core.AuthenticationManager())
            {
                var context = authManager.GetHighTrustCertificateAppAuthenticatedContext(url, clientId, hightrustCertificatePath, hightrustCertificatePassword, hightrustCertificateIssuerId, loginName);

                return InstantiateHighTrustConnection(context, url, minimalHealthScore, retryCount, retryWait, requestTimeout, tenantAdminUrl, host, disableTelemetry, skipAdminCheck);
            }
        }

        internal static PnPConnection InstantiateHighTrustConnection(string url, string clientId, System.Security.Cryptography.X509Certificates.X509Certificate2 hightrustCertificate, string hightrustCertificateIssuerId, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck, string loginName)
        {
            using (var authManager = new OfficeDevPnP.Core.AuthenticationManager())
            {
                var context = authManager.GetHighTrustCertificateAppAuthenticatedContext(url, clientId, hightrustCertificate, hightrustCertificateIssuerId, loginName);

                return InstantiateHighTrustConnection(context, url, minimalHealthScore, retryCount, retryWait, requestTimeout, tenantAdminUrl, host, disableTelemetry, skipAdminCheck);
            }
        }

        private static PnPConnection InstantiateHighTrustConnection(ClientContext context, string url, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck)
        {
            context.ApplicationName = Resources.ApplicationName;
            context.RequestTimeout = requestTimeout;
#if !SP2013
            context.DisableReturnValueCache = true;
#endif
            var connectionType = ConnectionType.OnPrem;
            if (skipAdminCheck == false)
            {
                if (IsTenantAdminSite(context))
                {
                    connectionType = ConnectionType.TenantAdmin;
                }
            }
            return new PnPConnection(context,
                connectionType,
                minimalHealthScore,
                retryCount,
                retryWait,
                null,
                url,
                tenantAdminUrl,
                PnPPSVersionTag,
                host,
                disableTelemetry,
                InitializationType.HighTrust);
        }
#endif
#endif

        internal static PnPConnection InstantiateDeviceLoginConnection(string url, bool launchBrowser, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, Action<string> messageCallback, Action<string> progressCallback, Func<bool> cancelRequest, PSHost host, bool disableTelemetry)
        {
            PnPConnection spoConnection = null;
            var connectionUri = new Uri(url);
            HttpClient client = new HttpClient();
            var result = client.GetStringAsync($"https://login.microsoftonline.com/common/oauth2/devicecode?resource={connectionUri.Scheme}://{connectionUri.Host}&client_id={PnPConnection.DeviceLoginClientId}").GetAwaiter().GetResult();
            var returnData = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            var context = new ClientContext(url);
            messageCallback(returnData["message"]);

            if (launchBrowser)
            {
                Utilities.Clipboard.Copy(returnData["user_code"]);
                messageCallback("Code has been copied to clipboard");
#if !NETSTANDARD2_1
                BrowserHelper.OpenBrowser(returnData["verification_url"], (success) =>
                {
                    if (success)
                    {
                        var tokenResult = GetTokenResult(connectionUri, returnData, messageCallback, progressCallback, cancelRequest);
                        if (tokenResult != null)
                        {
                            progressCallback("Token received");
                            spoConnection = new PnPConnection(context, tokenResult, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.DeviceLogin);
                        }
                        else
                        {
                            progressCallback("No token received.");
                        }
                    }
                });
#else
                OpenBrowser(returnData["verification_url"]);
                messageCallback(returnData["message"]);

                var tokenResult = GetTokenResult(connectionUri, returnData, messageCallback, progressCallback, cancelRequest);

                if (tokenResult != null)
                {
                    progressCallback("Token received");
                    spoConnection = new PnPConnection(context, tokenResult, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.DeviceLogin);
                }
                else
                {
                    progressCallback("No token received.");
                }
#endif
            }
            else
            {
                var tokenResult = GetTokenResult(connectionUri, returnData, messageCallback, progressCallback, cancelRequest);
                if (tokenResult != null)
                {
                    progressCallback("Token received");
                    spoConnection = PnPConnection.GetConnectionWithToken(tokenResult, TokenAudience.SharePointOnline, host, InitializationType.DeviceLogin, null, url, context, minimalHealthScore, PnPPSVersionTag, disableTelemetry);
                }
                else
                {
                    progressCallback("No token received.");
                }
            }

            if (spoConnection != null)
            {
                spoConnection.ConnectionMethod = ConnectionMethod.DeviceLogin;
            }
            return spoConnection;
        }

        internal static PnPConnection InstantiateGraphAccessTokenConnection(string accessToken, PSHost host, bool disableTelemetry)
        {
            var tokenResult = new GenericToken(accessToken);
            var spoConnection = new PnPConnection(tokenResult, ConnectionMethod.AccessToken, ConnectionType.O365, 0, 0, 0, PnPPSVersionTag, host, disableTelemetry, InitializationType.Graph);
            spoConnection.ConnectionMethod = ConnectionMethod.GraphDeviceLogin;
            return spoConnection;
        }

        internal static PnPConnection InstantiateGraphDeviceLoginConnection(bool launchBrowser, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, Action<string> messageCallback, Action<string> progressCallback, Func<bool> cancelRequest, PSHost host, bool disableTelemetry)
        {
            var connectionUri = new Uri("https://graph.microsoft.com");
            HttpClient client = new HttpClient();
            var result = client.GetStringAsync($"https://login.microsoftonline.com/common/oauth2/devicecode?resource={connectionUri.Scheme}://{connectionUri.Host}&client_id={PnPConnection.DeviceLoginClientId}").GetAwaiter().GetResult();
            var returnData = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

            PnPConnection spoConnection = null;

            if (launchBrowser)
            {
                Utilities.Clipboard.Copy(returnData["user_code"]);
                messageCallback("Code has been copied to clipboard");
#if !NETSTANDARD2_1
                BrowserHelper.OpenBrowser(returnData["verification_url"], (success) =>
                {
                    if (success)
                    {
                        var tokenResult = GetTokenResult(connectionUri, returnData, messageCallback, progressCallback, cancelRequest);
                        if (tokenResult != null)
                        {
                            progressCallback("Token received");
                            spoConnection = new PnPConnection(tokenResult, ConnectionMethod.GraphDeviceLogin, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, PnPPSVersionTag, host, disableTelemetry, InitializationType.GraphDeviceLogin);
                        }
                        else
                        {
                            progressCallback("No token received.");
                        }
                    }
                });
#else
                OpenBrowser(returnData["verification_url"]);
                messageCallback(returnData["message"]);

                var tokenResult = GetTokenResult(connectionUri, returnData, messageCallback, progressCallback, cancelRequest);

                if (tokenResult != null)
                {
                    progressCallback("Token received");
                    spoConnection = new PnPConnection(tokenResult, ConnectionMethod.GraphDeviceLogin, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, PnPPSVersionTag, host, disableTelemetry, InitializationType.GraphDeviceLogin);
                }
                else
                {
                    progressCallback("No token received.");
                }
#endif
            }
            else
            {
                messageCallback(returnData["message"]);

                var tokenResult = GetTokenResult(connectionUri, returnData, messageCallback, progressCallback, cancelRequest);

                if (tokenResult != null)
                {
                    progressCallback("Token received");
                    spoConnection = new PnPConnection(tokenResult, ConnectionMethod.GraphDeviceLogin, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, PnPPSVersionTag, host, disableTelemetry, InitializationType.GraphDeviceLogin);
                    spoConnection.ConnectionMethod = ConnectionMethod.GraphDeviceLogin;
                }
                else
                {
                    progressCallback("No token received.");
                }
            }
            return spoConnection;
        }



        private static GenericToken GetTokenResult(Uri connectionUri, Dictionary<string, string> returnData, Action<string> messageCallback, Action<string> progressCallback, Func<bool> cancelRequest)
        {
            HttpClient client = new HttpClient();
            var body = new StringContent($"resource={connectionUri.Scheme}://{connectionUri.Host}&client_id={PnPConnection.DeviceLoginClientId}&grant_type=device_code&code={returnData["device_code"]}");
            body.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";

            var responseMessage = client.PostAsync("https://login.microsoftonline.com/common/oauth2/token", body).GetAwaiter().GetResult();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var shouldCancel = cancelRequest();
            while (!responseMessage.IsSuccessStatusCode && !shouldCancel)
            {
                if (stopWatch.ElapsedMilliseconds > 60 * 1000)
                {
                    break;
                }
                progressCallback(".");
                System.Threading.Thread.Sleep(1000);
                body = new StringContent($"resource={connectionUri.Scheme}://{connectionUri.Host}&client_id={PnPConnection.DeviceLoginClientId}&grant_type=device_code&code={returnData["device_code"]}");
                body.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
                responseMessage = client.PostAsync("https://login.microsoftonline.com/common/oauth2/token", body).GetAwaiter().GetResult();
                shouldCancel = cancelRequest();
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<SharePointToken>(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            }
            else
            {
                if (shouldCancel)
                {
                    messageCallback("Cancelled");
                }
                else
                {
                    messageCallback("Timeout");
                }
                return null;
            }
        }

        internal static void OpenBrowser(string url)
        {
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (Utilities.OperatingSystem.IsWindows())
                {
                    url = url.Replace("&", "^&");
                    System.Diagnostics.Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (Utilities.OperatingSystem.IsLinux())
                {
                    System.Diagnostics.Process.Start("xdg-open", url);
                }
                else if (Utilities.OperatingSystem.IsMacOS())
                {
                    System.Diagnostics.Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

#if !ONPREMISES
#if !NETSTANDARD2_1
        internal static PnPConnection InitiateAzureADNativeApplicationConnection(Uri url, string clientId, Uri redirectUri, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck = false, AzureEnvironment azureEnvironment = AzureEnvironment.Production)
        {
            using (var authManager = new OfficeDevPnP.Core.AuthenticationManager())
            {
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string configFile = Path.Combine(appDataFolder, "SharePointPnP.PowerShell\\tokencache.dat");
                FileTokenCache cache = new FileTokenCache(configFile);
                var context = PnPClientContext.ConvertFrom(authManager.GetAzureADNativeApplicationAuthenticatedContext(url.ToString(), clientId, redirectUri, cache, azureEnvironment), retryCount, retryWait * 10000);
                var connectionType = ConnectionType.OnPrem;
                if (url.Host.ToUpperInvariant().EndsWith("SHAREPOINT.COM"))
                {
                    connectionType = ConnectionType.O365;
                }
                if (skipAdminCheck == false)
                {
                    if (IsTenantAdminSite(context))
                    {
                        connectionType = ConnectionType.TenantAdmin;
                    }
                }
                var spoConnection = new PnPConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, clientId, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.AADNativeApp)
                {
                    ConnectionMethod = ConnectionMethod.AzureADNativeApplication
                };
                return spoConnection;
            }
        }
#endif

        internal static PnPConnection InitiateAzureADAppOnlyConnection(Uri url, string clientId, string tenant, X509Certificate2 certificate, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck = false, AzureEnvironment azureEnvironment = AzureEnvironment.Production)
        {
            using (var authManager = new OfficeDevPnP.Core.AuthenticationManager())
            {
                var context = PnPClientContext.ConvertFrom(authManager.GetAzureADAppOnlyAuthenticatedContext(url.ToString(), clientId, tenant, certificate, azureEnvironment), retryCount, retryWait * 1000);
                var connectionType = ConnectionType.OnPrem;
                if (url.Host.ToUpperInvariant().EndsWith("SHAREPOINT.COM"))
                {
                    connectionType = ConnectionType.O365;
                }
                if (skipAdminCheck == false)
                {
                    if (IsTenantAdminSite(context))
                    {
                        connectionType = ConnectionType.TenantAdmin;
                    }
                }
                var spoConnection = new PnPConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.AADAppOnly);
                spoConnection.ConnectionMethod = Model.ConnectionMethod.AzureADAppOnly;
                return spoConnection;
            }
        }

        internal static PnPConnection InitiateAzureADAppOnlyConnection(Uri url, string clientId, string tenant, string certificatePath, SecureString certificatePassword, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck = false, AzureEnvironment azureEnvironment = AzureEnvironment.Production)
        {
            X509Certificate2 certificate = CertificateHelper.GetCertificateFromPath(certificatePath, certificatePassword);

            return InitiateAzureAdAppOnlyConnectionWithCert(url, clientId, tenant, minimalHealthScore, retryCount,
                retryWait, requestTimeout, tenantAdminUrl, host, disableTelemetry, skipAdminCheck, azureEnvironment,
                certificate, true);
        }

        internal static PnPConnection InitiateAzureADAppOnlyConnection(Uri url, string clientId, string tenant,
            string thumbprint, int minimalHealthScore,
            int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host,
            bool disableTelemetry, bool skipAdminCheck = false,
            AzureEnvironment azureEnvironment = AzureEnvironment.Production)
        {
            X509Certificate2 certificate = CertificateHelper.GetCertificatFromStore(thumbprint);

            if (certificate == null)
            {
                throw new PSArgumentOutOfRangeException(nameof(thumbprint), null, string.Format(Resources.CertificateWithThumbprintNotFound, thumbprint));
            }

            return InitiateAzureAdAppOnlyConnectionWithCert(url, clientId, tenant, minimalHealthScore, retryCount, retryWait, requestTimeout, tenantAdminUrl, host, disableTelemetry, skipAdminCheck, azureEnvironment, certificate, false);
        }

        internal static PnPConnection InitiateAzureADAppOnlyConnection(Uri url, string clientId, string tenant, string certificatePEM, string privateKeyPEM, SecureString certificatePassword, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck = false, AzureEnvironment azureEnvironment = AzureEnvironment.Production)
        {
            string password = new System.Net.NetworkCredential(string.Empty, certificatePassword).Password;
            X509Certificate2 certificate = CertificateHelper.GetCertificateFromPEMstring(certificatePEM, privateKeyPEM, password);

            return InitiateAzureAdAppOnlyConnectionWithCert(url, clientId, tenant, minimalHealthScore, retryCount, retryWait, requestTimeout, tenantAdminUrl, host, disableTelemetry, skipAdminCheck, azureEnvironment, certificate, false);
        }

        /// <summary>
        /// Takes a certificate encoded in Base64 such as retrieved from Azure KeyVault when using Azure Functions to authenticate to SharePoint
        /// </summary>
        /// <remarks>See https://docs.microsoft.com/en-us/sharepoint/dev/solution-guidance/security-apponly-azuread how to set up a certificate which you can store in Azure KeyVault</remarks>
        /// <param name="url">Url of the SharePoint site to connect to</param>
        /// <param name="clientId">Application/client ID of the Azure Active Directory application registration</param>
        /// <param name="tenant">Tenant name to connect to, i.e. contoso.onmicrosoft.com</param>
        /// <param name="minimalHealthScore">Value between 0 and 10 indicating the health of the SharePoint server connected to should report before commands get executed where 0 is the healthiest and 10 the least healthy. I.e. if you set it to 3, SharePoint must report a health score of 0, 1, 2 or 3 before it will execute the commands. If set to -1, no health check will be performed.</param>
        /// <param name="retryCount">Amount of times to retry an operation that i.e. times out or runs into health issues before giving up on it</param>
        /// <param name="retryWait">Time in seconds to wait between retry attempts</param>
        /// <param name="requestTimeout">Time in milliseconds to allow a command to complete before considering it failed</param>
        /// <param name="tenantAdminUrl">Url of the admin site of the tenant. If not provided, it will assume to connect automatically to https://<tenantname>-admin.sharepoint.com.</param>
        /// <param name="host">Reference to the PowerShell session in which the commands will be executed</param>
        /// <param name="disableTelemetry">Boolean indicating whether or not telemetry should be disabled</param>
        /// <param name="skipAdminCheck">Boolean indicating if it should check if the connection is being made to the Tenand admin site</param>
        /// <param name="azureEnvironment">Type of Azure environment connecting to</param>
        /// <param name="base64EncodedCertificate">Base64 encoded string containing the certificate which grants access to SharePoint Online</param>
        /// <returns>A connection to SharePoint</returns>
        internal static PnPConnection InitiateAzureAdAppOnlyConnectionWithCert(Uri url, string clientId, string tenant,
            int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry,
            bool skipAdminCheck, AzureEnvironment azureEnvironment, string base64EncodedCertificate)
        {
            X509Certificate2 certificate = CertificateHelper.GetCertificateFromBase64Encodedstring(base64EncodedCertificate);
            return InitiateAzureAdAppOnlyConnectionWithCert(url, clientId, tenant, minimalHealthScore, retryCount, retryWait, requestTimeout, tenantAdminUrl, host, disableTelemetry, skipAdminCheck, azureEnvironment, certificate);
        }

        /// <summary>
        /// Takes a certificate encoded in Base64 such as retrieved from Azure KeyVault when using Azure Functions to authenticate to SharePoint
        /// </summary>
        /// <remarks>See https://docs.microsoft.com/en-us/sharepoint/dev/solution-guidance/security-apponly-azuread how to set up a certificate which you can store in Azure KeyVault</remarks>
        /// <param name="url">Url of the SharePoint site to connect to</param>
        /// <param name="clientId">Application/client ID of the Azure Active Directory application registration</param>
        /// <param name="tenant">Tenant name to connect to, i.e. contoso.onmicrosoft.com</param>
        /// <param name="minimalHealthScore">Value between 0 and 10 indicating the health of the SharePoint server connected to should report before commands get executed where 0 is the healthiest and 10 the least healthy. I.e. if you set it to 3, SharePoint must report a health score of 0, 1, 2 or 3 before it will execute the commands. If set to -1, no health check will be performed.</param>
        /// <param name="retryCount">Amount of times to retry an operation that i.e. times out or runs into health issues before giving up on it</param>
        /// <param name="retryWait">Time in seconds to wait between retry attempts</param>
        /// <param name="requestTimeout">Time in milliseconds to allow a command to complete before considering it failed</param>
        /// <param name="tenantAdminUrl">Url of the admin site of the tenant. If not provided, it will assume to connect automatically to https://<tenantname>-admin.sharepoint.com.</param>
        /// <param name="host">Reference to the PowerShell session in which the commands will be executed</param>
        /// <param name="disableTelemetry">Boolean indicating whether or not telemetry should be disabled</param>
        /// <param name="skipAdminCheck">Boolean indicating if it should check if the connection is being made to the Tenand admin site</param>
        /// <param name="azureEnvironment">Type of Azure environment connecting to</param>
        /// <param name="certificate">The X509Certificate2 which grants access to SharePoint Online</param>
        /// <returns>A connection to SharePoint</returns>
        internal static PnPConnection InitiateAzureAdAppOnlyConnectionWithCert(Uri url, string clientId, string tenant,
            int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry,
            bool skipAdminCheck, AzureEnvironment azureEnvironment, X509Certificate2 certificate)
        {
            return InitiateAzureAdAppOnlyConnectionWithCert(url, clientId, tenant, minimalHealthScore, retryCount, retryWait, requestTimeout, tenantAdminUrl, host, disableTelemetry, skipAdminCheck, azureEnvironment, certificate, false);
        }

        private static PnPConnection InitiateAzureAdAppOnlyConnectionWithCert(Uri url, string clientId, string tenant,
            int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry,
            bool skipAdminCheck, AzureEnvironment azureEnvironment, X509Certificate2 certificate, bool certificateFromFile)
        {
            using (var authManager = new OfficeDevPnP.Core.AuthenticationManager())
            {
                var clientContext = authManager.GetAzureADAppOnlyAuthenticatedContext(url.ToString(), clientId, tenant, certificate, azureEnvironment);
                var context = PnPClientContext.ConvertFrom(clientContext, retryCount, retryWait * 1000);
                context.RequestTimeout = requestTimeout;
                var connectionType = ConnectionType.OnPrem;
                if (url.Host.ToUpperInvariant().EndsWith("SHAREPOINT.COM"))
                {
                    connectionType = ConnectionType.O365;
                }

                if (skipAdminCheck == false)
                {
                    if (IsTenantAdminSite(context))
                    {
                        connectionType = ConnectionType.TenantAdmin;
                    }
                }

                var spoConnection = new PnPConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, clientId, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.AADAppOnly)
                {
                    ConnectionMethod = ConnectionMethod.AzureADAppOnly,
                    Certificate = certificate,
                    Tenant = tenant,
                    DeleteCertificateFromCacheOnDisconnect = certificateFromFile
                };
                return spoConnection;
            }
        }

        /// <summary>
        /// Sets up a connection to Microsoft Graph using a Client Id and Client Secret
        /// </summary>
        /// <param name="clientId">Client ID to use to authenticate</param>
        /// <param name="clientSecret">Client Secret to use to authenticate</param>
        /// <param name="aadDomain">The Azure Active Directory domain to authenticate to, i.e. contoso.onmicrosoft.com</param>
        /// <param name="host">The PowerShell host environment reference</param>
        /// <param name="disableTelemetry">Boolean indicating if telemetry should be disabled</param>
        /// <returns></returns>
        internal static PnPConnection InitiateAzureAdAppOnlyConnectionWithClientIdClientSecret(string clientId, string clientSecret, string aadDomain, PSHost host, bool disableTelemetry)
        {
            var app = ConfidentialClientApplicationBuilder.Create(clientId).WithAuthority($"https://login.microsoftonline.com/{aadDomain}").WithClientSecret(clientSecret).Build();
            var result = app.AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" }).ExecuteAsync().GetAwaiter().GetResult();
            if (result == null)
            {
                return null;
            }

            var spoConnection = InstantiateGraphAccessTokenConnection(result.AccessToken, host, disableTelemetry);
            return spoConnection;
        }

        /// <summary>
        /// Verifies if a local copy of the certificate has been stored in the machinekeys cache of Windows and if so, will remove it to avoid the cache from growing over time
        /// </summary>
        /// <param name="certificate">Certificate to validate if there is a local cached copy for and if so, delete it</param>
        internal static void CleanupCryptoMachineKey(X509Certificate2 certificate)
        {
            var privateKey = (RSACryptoServiceProvider)certificate.PrivateKey;
            string uniqueKeyContainerName = privateKey.CspKeyContainerInfo.UniqueKeyContainerName;
            certificate.Reset();
            
            var programDataPath = Environment.GetEnvironmentVariable("ProgramData");
            if (string.IsNullOrEmpty(programDataPath))
            {
                programDataPath = @"C:\ProgramData";
            }
            try
            {
                var temporaryCertificateFilePath = $@"{programDataPath}\Microsoft\Crypto\RSA\MachineKeys\{uniqueKeyContainerName}";
                if (System.IO.File.Exists(temporaryCertificateFilePath))
                {
                    System.IO.File.Delete(temporaryCertificateFilePath);
                }
            }
            catch (Exception)
            {
                // best effort cleanup
            }
        }
#endif

#if !ONPREMISES
        internal static PnPConnection InitiateAccessTokenConnection(Uri url, string accessToken, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck = false, AzureEnvironment azureEnvironment = AzureEnvironment.Production)
        {
            using (var authManager = new OfficeDevPnP.Core.AuthenticationManager())
            {
                var context = PnPClientContext.ConvertFrom(authManager.GetAzureADAccessTokenAuthenticatedContext(url.ToString(), accessToken), retryCount, retryWait);
                var connectionType = ConnectionType.O365;
                if (skipAdminCheck == false)
                {
                    if (IsTenantAdminSite(context))
                    {
                        connectionType = ConnectionType.TenantAdmin;
                    }
                }
                var spoConnection = new PnPConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.Token);
                spoConnection.ConnectionMethod = Model.ConnectionMethod.AccessToken;
                return spoConnection;
            }
        }
#endif

#if !NETSTANDARD2_1
        internal static PnPConnection InstantiateWebloginConnection(Uri url, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck = false)
        {
            using (var authManager = new OfficeDevPnP.Core.AuthenticationManager())
            {
                var context = PnPClientContext.ConvertFrom(authManager.GetWebLoginClientContext(url.ToString()), retryCount, retryWait * 1000);

                if (context != null)
                {
                    context.RetryCount = retryCount;
                    context.Delay = retryWait * 1000;
                    context.ApplicationName = Resources.ApplicationName;
                    context.RequestTimeout = requestTimeout;
#if !SP2013
                    context.DisableReturnValueCache = true;
#endif
                    var connectionType = ConnectionType.OnPrem;
                    if (url.Host.ToUpperInvariant().EndsWith("SHAREPOINT.COM"))
                    {
                        connectionType = ConnectionType.O365;
                    }
                    if (skipAdminCheck == false)
                    {
                        if (IsTenantAdminSite(context))
                        {
                            connectionType = ConnectionType.TenantAdmin;
                        }
                    }
                    var spoConnection = new PnPConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.InteractiveLogin);
                    spoConnection.ConnectionMethod = Model.ConnectionMethod.WebLogin;
                    return spoConnection;
                }
            }
            throw new Exception("Error establishing a connection, context is null");
        }
#endif

#if !NETSTANDARD2_1
        internal static PnPConnection InstantiateSPOnlineConnection(Uri url, PSCredential credentials, PSHost host, bool currentCredentials, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool disableTelemetry, bool skipAdminCheck = false, ClientAuthenticationMode authenticationMode = ClientAuthenticationMode.Default)
        {
            var context = new PnPClientContext(url.AbsoluteUri);

            context.RetryCount = retryCount;
            context.Delay = retryWait * 1000;
            context.ApplicationName = Resources.ApplicationName;
#if !SP2013
            context.DisableReturnValueCache = true;
#endif
            context.RequestTimeout = requestTimeout;

            context.AuthenticationMode = authenticationMode;

            if (authenticationMode == ClientAuthenticationMode.FormsAuthentication)
            {
                var formsAuthInfo = new FormsAuthenticationLoginInfo(credentials.UserName, EncryptionUtility.ToInsecureString(credentials.Password));
                context.FormsAuthenticationLoginInfo = formsAuthInfo;
            }
            if (!currentCredentials)
            {
                try
                {
                    SharePointOnlineCredentials onlineCredentials = new SharePointOnlineCredentials(credentials.UserName, credentials.Password);
                    context.Credentials = onlineCredentials;
                    try
                    {
                        context.ExecuteQueryRetry();
                    }
#if !ONPREMISES
                    catch (NotSupportedException)
                    {
                        // legacy auth?
                        using (var authManager = new OfficeDevPnP.Core.AuthenticationManager())
                        {
                            context = PnPClientContext.ConvertFrom(authManager.GetAzureADCredentialsContext(url.ToString(), credentials.UserName, credentials.Password));
                            context.ExecuteQueryRetry();
                        }
                    }
#endif
                    catch (ClientRequestException)
                    {
                        context.Credentials = new NetworkCredential(credentials.UserName, credentials.Password);
                    }
                    catch (ServerException)
                    {
                        context.Credentials = new NetworkCredential(credentials.UserName, credentials.Password);
                    }
                }
                catch (ArgumentException)
                {
                    // OnPrem?
                    context.Credentials = new NetworkCredential(credentials.UserName, credentials.Password);
                    try
                    {
                        context.ExecuteQueryRetry();
                    }
                    catch (ClientRequestException ex)
                    {
                        throw new Exception("Error establishing a connection", ex);
                    }
                    catch (ServerException ex)
                    {
                        throw new Exception("Error establishing a connection", ex);
                    }
                }

            }
            else
            {
                if (credentials != null)
                {
                    context.Credentials = new NetworkCredential(credentials.UserName, credentials.Password);
                }
                else
                {
                    // If current credentials should be used, use the DefaultNetworkCredentials of the CredentialCache. This has the same effect as using "UseDefaultCredentials" in a HttpClient.
                    context.Credentials = CredentialCache.DefaultNetworkCredentials;
                }

                // Add Request Header to force Windows Authentication which avoids an issue if multiple authentication providers are enabled on a webapplication
                context.ExecutingWebRequest += delegate (object sender, WebRequestEventArgs e)
                {
                    // Add the header that tells SharePoint to use Windows authentication
                    e.WebRequestExecutor.RequestHeaders["X-FORMS_BASED_AUTH_ACCEPTED"] = "f";
                };

            }
#if SP2013 || SP2016 || SP2019
            var connectionType = ConnectionType.OnPrem;
#else
            var connectionType = ConnectionType.O365;
#endif
            if (url.Host.ToUpperInvariant().EndsWith("SHAREPOINT.COM"))
            {
                connectionType = ConnectionType.O365;
            }

            if (skipAdminCheck == false)
            {
                if (IsTenantAdminSite(context))
                {
                    connectionType = ConnectionType.TenantAdmin;
                }
            }
            var spoConnection = new PnPConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, credentials, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.Credentials);
            spoConnection.ConnectionMethod = Model.ConnectionMethod.Credentials;
            return spoConnection;
        }
#endif

#if NETSTANDARD2_1
        internal static PnPConnection InstantiateSPOnlineConnection(Uri url, PSCredential credentials, PSHost host, bool currentCredentials, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool disableTelemetry, bool skipAdminCheck = false)
        {
            var context = new PnPClientContext(url.AbsoluteUri);

            context.RetryCount = retryCount;
            context.Delay = retryWait * 1000;
            context.ApplicationName = Resources.ApplicationName;
            context.DisableReturnValueCache = true;
            context.RequestTimeout = requestTimeout;

            try
            {
                using (var authManager = new OfficeDevPnP.Core.AuthenticationManager())
                {
                    context = PnPClientContext.ConvertFrom(authManager.GetAzureADCredentialsContext(url.ToString(), credentials.UserName, credentials.Password));
                    context.ExecuteQueryRetry();
                }
            }
            catch (ClientRequestException)
            {
                context.Credentials = new NetworkCredential(credentials.UserName, credentials.Password);
            }
            catch (ServerException)
            {
                context.Credentials = new NetworkCredential(credentials.UserName, credentials.Password);
            }
            var connectionType = ConnectionType.O365;
            if (url.Host.ToUpperInvariant().EndsWith("SHAREPOINT.COM"))
            {
                connectionType = ConnectionType.O365;
            }

            if (skipAdminCheck == false)
            {
                if (IsTenantAdminSite(context))
                {
                    connectionType = ConnectionType.TenantAdmin;
                }
            }
            var spoConnection = new PnPConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, credentials, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.Credentials);
            spoConnection.ConnectionMethod = Model.ConnectionMethod.Credentials;
            return spoConnection;
        }
#endif

#if !NETSTANDARD2_1
        internal static PnPConnection InstantiateAdfsConnection(Uri url, bool useKerberos, PSCredential credentials, PSHost host, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool disableTelemetry, bool skipAdminCheck = false, string loginProviderName = null)
        {
            using (var authManager = new OfficeDevPnP.Core.AuthenticationManager())
            {
                string adfsHost;
                string adfsRelyingParty;
                OfficeDevPnP.Core.AuthenticationManager.GetAdfsConfigurationFromTargetUri(url, loginProviderName, out adfsHost, out adfsRelyingParty);

                if (string.IsNullOrEmpty(adfsHost) || string.IsNullOrEmpty(adfsRelyingParty))
                {
                    throw new Exception("Cannot retrieve ADFS settings.");
                }

                PnPClientContext context;
                if (useKerberos)
                {
                    context = PnPClientContext.ConvertFrom(authManager.GetADFSKerberosMixedAuthenticationContext(url.ToString(),
                                    adfsHost,
                                    adfsRelyingParty),
                                retryCount,
                                retryWait * 1000);
                }
                else
                {
                    if (null == credentials)
                    {
                        throw new ArgumentNullException(nameof(credentials));
                    }
                    var networkCredentials = credentials.GetNetworkCredential();
                    context = PnPClientContext.ConvertFrom(authManager.GetADFSUserNameMixedAuthenticatedContext(url.ToString(),
                                    networkCredentials.UserName,
                                    networkCredentials.Password,
                                    networkCredentials.Domain,
                                    adfsHost,
                                    adfsRelyingParty),
                                retryCount,
                                retryWait * 1000);
                }

                context.RetryCount = retryCount;
                context.Delay = retryWait * 1000;

                context.ApplicationName = Resources.ApplicationName;
                context.RequestTimeout = requestTimeout;
#if !SP2013
                context.DisableReturnValueCache = true;
#endif

                var connectionType = ConnectionType.OnPrem;

                if (skipAdminCheck == false)
                {
                    if (IsTenantAdminSite(context))
                    {
                        connectionType = ConnectionType.TenantAdmin;
                    }
                }
                var spoConnection = new PnPConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.ADFS);
                spoConnection.ConnectionMethod = ConnectionMethod.ADFS;
                return spoConnection;
            }
        }

        internal static PnPConnection InstantiateAdfsCertificateConnection(Uri url, string serialNumber, PSHost host, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool disableTelemetry, bool skipAdminCheck = false, string loginProviderName = null)
        {
            using (var authManager = new OfficeDevPnP.Core.AuthenticationManager())
            {
                string adfsHost;
                string adfsRelyingParty;
                OfficeDevPnP.Core.AuthenticationManager.GetAdfsConfigurationFromTargetUri(url, loginProviderName, out adfsHost, out adfsRelyingParty);

                if (string.IsNullOrEmpty(adfsHost) || string.IsNullOrEmpty(adfsRelyingParty))
                {
                    throw new Exception("Cannot retrieve ADFS settings.");
                }

                var context = authManager.GetADFSCertificateMixedAuthenticationContext(url.ToString(), serialNumber, adfsHost, adfsRelyingParty);

                context.ApplicationName = Resources.ApplicationName;
                context.RequestTimeout = requestTimeout;
#if !ONPREMISES
                context.DisableReturnValueCache = true;
#elif SP2016 || SP2019
                context.DisableReturnValueCache = true;
#endif
                var connectionType = ConnectionType.OnPrem;

                if (skipAdminCheck == false)
                {
                    if (IsTenantAdminSite(context))
                    {
                        connectionType = ConnectionType.TenantAdmin;
                    }
                }
                var spoConnection = new PnPConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.ADFS);
                spoConnection.ConnectionMethod = ConnectionMethod.ADFS;
                return spoConnection;
            }
        }
#endif

        public static string GetRealmFromTargetUrl(Uri targetApplicationUri)
        {
            WebRequest request = WebRequest.Create(targetApplicationUri + "/_vti_bin/client.svc");
            request.Headers.Add("Authorization: Bearer ");

            try
            {
                using (request.GetResponse())
                {
                }
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    return null;
                }

                string bearerResponseHeader = e.Response.Headers["WWW-Authenticate"];
                if (string.IsNullOrEmpty(bearerResponseHeader))
                {
                    return null;
                }

                const string bearer = "Bearer realm=\"";
                int bearerIndex = bearerResponseHeader.IndexOf(bearer, StringComparison.Ordinal);
                if (bearerIndex < 0)
                {
                    return null;
                }

                int realmIndex = bearerIndex + bearer.Length;

                if (bearerResponseHeader.Length >= realmIndex + 36)
                {
                    string targetRealm = bearerResponseHeader.Substring(realmIndex, 36);

                    Guid realmGuid;

                    if (Guid.TryParse(targetRealm, out realmGuid))
                    {
                        return targetRealm;
                    }
                }
            }
            return null;
        }

        private static bool IsTenantAdminSite(ClientRuntimeContext clientContext)
        {
            try
            {
                using (var clonedContext = clientContext.Clone(clientContext.Url))
                {
                    var tenant = new Tenant(clonedContext);
                    clonedContext.ExecuteQueryRetry();
                    return true;
                }
            }
            catch (ClientRequestException)
            {
                return false;
            }
            catch (ServerException)
            {
                return false;
            }
            catch (WebException)
            {
                return false;
            }
        }

        private static string PnPPSVersionTag => (PnPPSVersionTagLazy.Value);

        private static readonly Lazy<string> PnPPSVersionTagLazy = new Lazy<string>(
            () =>
            {
                var coreAssembly = Assembly.GetExecutingAssembly();
                var result = $"PnPPS:{((AssemblyFileVersionAttribute)coreAssembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version.Split('.')[2]}";
                return (result);
            },
            true);

        public static string GetLatestVersion()
        {
            try
            {
                if (!VersionChecked)
                {
                    using (var client = new WebClient())
                    {
                        PnPConnectionHelper.VersionChecked = true;
                        var onlineVersion = client.DownloadString(VersionCheckUrl);
                        onlineVersion = onlineVersion.Trim(new char[] { '\t', '\r', '\n' });
                        var assembly = Assembly.GetExecutingAssembly();
                        var currentVersion = ((AssemblyFileVersionAttribute)assembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version;
                        if (Version.TryParse(onlineVersion, out Version availableVersion))
                        {
                            if (availableVersion > new Version(currentVersion))
                            {
                                return $"\nA newer version of PnP PowerShell is available: {availableVersion}. Consider upgrading.\n";
                            }
                        }
                        PnPConnectionHelper.VersionChecked = true;
                    }
                }
            }
            catch
            { }
            return null;
        }
    }
}
