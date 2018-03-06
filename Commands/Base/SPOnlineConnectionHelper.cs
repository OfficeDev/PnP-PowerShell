#if !NETSTANDARD2_0
using Microsoft.IdentityModel.Clients.ActiveDirectory;
#endif
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.Commands.Enums;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Net;
using System.Security;
using OfficeDevPnP.Core;
using OfficeDevPnP.Core.Utilities;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using SharePointPnP.PowerShell.Commands.Utilities;
using SharePointPnP.PowerShell.Commands.Model;

namespace SharePointPnP.PowerShell.Commands.Base
{
    internal class SPOnlineConnectionHelper
    {
#if !NETSTANDARD2_0
        public static AuthenticationContext AuthContext { get; set; }
#endif
        static SPOnlineConnectionHelper()
        {
        }

        internal static SPOnlineConnection InstantiateSPOnlineConnection(Uri url, string realm, string clientId, string clientSecret, PSHost host, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool skipAdminCheck = false)
        {
            var authManager = new OfficeDevPnP.Core.AuthenticationManager();
            if (realm == null)
            {
                realm = GetRealmFromTargetUrl(url);
            }

            PnPClientContext context;
            if (url.DnsSafeHost.Contains("spoppe.com"))
            {
                context = PnPClientContext.ConvertFrom(authManager.GetAppOnlyAuthenticatedContext(url.ToString(), realm, clientId, clientSecret, acsHostUrl: "windows-ppe.net", globalEndPointPrefix: "login"), retryCount, retryWait * 1000);
            }
            else
            {
                context = PnPClientContext.ConvertFrom(authManager.GetAppOnlyAuthenticatedContext(url.ToString(), realm, clientId, clientSecret), retryCount, retryWait * 1000);
            }

            context.ApplicationName = Properties.Resources.ApplicationName;
            context.RequestTimeout = requestTimeout;
#if !ONPREMISES
            context.DisableReturnValueCache = true;
#elif SP2016
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
            return new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag);
        }

#if !NETSTANDARD2_0
#if ONPREMISES
        internal static SPOnlineConnection InstantiateHighTrustConnection(string url, string clientId, string hightrustCertificatePath, string hightrustCertificatePassword, string hightrustCertificateIssuerId, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool skipAdminCheck = false)
        {
            var authManager = new OfficeDevPnP.Core.AuthenticationManager();
            var context = authManager.GetHighTrustCertificateAppOnlyAuthenticatedContext(url, clientId, hightrustCertificatePath, hightrustCertificatePassword, hightrustCertificateIssuerId);

            return InstantiateHighTrustConnection(context, url, minimalHealthScore, retryCount, retryWait, requestTimeout, tenantAdminUrl, skipAdminCheck);
        }

        internal static SPOnlineConnection InstantiateHighTrustConnection(string url, string clientId, System.Security.Cryptography.X509Certificates.X509Certificate2 hightrustCertificate, string hightrustCertificateIssuerId, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool skipAdminCheck = false)
        {
            var authManager = new OfficeDevPnP.Core.AuthenticationManager();
            var context = authManager.GetHighTrustCertificateAppOnlyAuthenticatedContext(url, clientId, hightrustCertificate, hightrustCertificateIssuerId);

            return InstantiateHighTrustConnection(context, url, minimalHealthScore, retryCount, retryWait, requestTimeout, tenantAdminUrl, skipAdminCheck);
        }

        private static SPOnlineConnection InstantiateHighTrustConnection(ClientContext context, string url, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool skipAdminCheck = false)
        {
            context.ApplicationName = Properties.Resources.ApplicationName;
            context.RequestTimeout = requestTimeout;
#if SP2016
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
            return new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url, tenantAdminUrl, PnPPSVersionTag);
        }
#endif
#endif

        internal static SPOnlineConnection InstantiateDeviceLoginConnection(string url, bool launchBrowser, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, Action<string> messageCallback, Action<string> progressCallback)
        {
            SPOnlineConnection spoConnection = null;
            var connectionUri = new Uri(url);
            HttpClient client = new HttpClient();
            var result = client.GetStringAsync($"https://login.microsoftonline.com/common/oauth2/devicecode?resource={connectionUri.Scheme}://{connectionUri.Host}&client_id={SPOnlineConnection.DeviceLoginAppId}").GetAwaiter().GetResult();
            var returnData = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            var context = new ClientContext(url);
            messageCallback(returnData["message"]);

            if (launchBrowser)
            {
                Utilities.Clipboard.Copy(returnData["user_code"]);
                messageCallback("Code has been copied to clipboard");
#if !NETSTANDARD2_0
                BrowserHelper.OpenBrowser(returnData["verification_url"], (success) =>
                {
                    if (success)
                    {
                        var tokenResult = GetTokenResult(connectionUri, returnData, messageCallback, progressCallback);
                        if (tokenResult != null)
                        {
                            progressCallback("Token received");
                            spoConnection = new SPOnlineConnection(context, tokenResult, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag);
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

                var tokenResult = GetTokenResult(connectionUri, returnData, messageCallback, progressCallback);

                if (tokenResult != null)
                {
                    progressCallback("Token received");
                    spoConnection = new SPOnlineConnection(context, tokenResult, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag);
                }
                else
                {
                    progressCallback("No token received.");
                }
#endif
            }
            else
            {
                var tokenResult = GetTokenResult(connectionUri, returnData, messageCallback, progressCallback);
                if (tokenResult != null)
                {
                    progressCallback("Token received");
                    spoConnection = new SPOnlineConnection(context, tokenResult, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag);
                }
                else
                {
                    progressCallback("No token received.");
                }
            }
            spoConnection.ConnectionMethod = ConnectionMethod.DeviceLogin;
            return spoConnection;
        }

        internal static SPOnlineConnection InstantiateGraphAccessTokenConnection(string accessToken)
        {
#if NETSTANDARD2_0
            var jwtToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(accessToken);
#else
            var jwtToken = new System.IdentityModel.Tokens.JwtSecurityToken(accessToken);
#endif
            var tokenResult = new TokenResult();
            tokenResult.AccessToken = accessToken;
            tokenResult.ExpiresOn = jwtToken.ValidTo;
            var spoConnection = new SPOnlineConnection(tokenResult, ConnectionMethod.AccessToken, ConnectionType.O365, 0, 0, 0, PnPPSVersionTag);
            spoConnection.ConnectionMethod = ConnectionMethod.GraphDeviceLogin;
            return spoConnection;
        }

        internal static SPOnlineConnection InstantiateGraphDeviceLoginConnection(bool launchBrowser, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, Action<string> messageCallback, Action<string> progressCallback)
        {
            var connectionUri = new Uri("https://graph.microsoft.com");
            HttpClient client = new HttpClient();
            var result = client.GetStringAsync($"https://login.microsoftonline.com/common/oauth2/devicecode?resource={connectionUri.Scheme}://{connectionUri.Host}&client_id={SPOnlineConnection.DeviceLoginAppId}").GetAwaiter().GetResult();
            var returnData = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

            SPOnlineConnection spoConnection = null;

            if (launchBrowser)
            {
                Utilities.Clipboard.Copy(returnData["user_code"]);
                messageCallback("Code has been copied to clipboard");
#if !NETSTANDARD2_0
                BrowserHelper.OpenBrowser(returnData["verification_url"], (success) =>
                {
                    if (success)
                    {
                        var tokenResult = GetTokenResult(connectionUri, returnData, messageCallback, progressCallback);
                        if (tokenResult != null)
                        {
                            progressCallback("Token received");
                            spoConnection = new SPOnlineConnection(tokenResult, ConnectionMethod.GraphDeviceLogin, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, PnPPSVersionTag);
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

                var tokenResult = GetTokenResult(connectionUri, returnData, messageCallback, progressCallback);

                if (tokenResult != null)
                {
                    progressCallback("Token received");
                    spoConnection = new SPOnlineConnection(tokenResult, ConnectionMethod.GraphDeviceLogin, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, PnPPSVersionTag);
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


                var tokenResult = GetTokenResult(connectionUri, returnData, messageCallback, progressCallback);

                if (tokenResult != null)
                {
                    progressCallback("Token received");
                    spoConnection = new SPOnlineConnection(tokenResult, ConnectionMethod.GraphDeviceLogin, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, PnPPSVersionTag);
                }
                else
                {
                    progressCallback("No token received.");
                }
            }
            spoConnection.ConnectionMethod = ConnectionMethod.GraphDeviceLogin;
            return spoConnection;
        }



        private static TokenResult GetTokenResult(Uri connectionUri, Dictionary<string, string> returnData, Action<string> messageCallback, Action<string> progressCallback)
        {
            HttpClient client = new HttpClient();
            var body = new StringContent($"resource={connectionUri.Scheme}://{connectionUri.Host}&client_id={SPOnlineConnection.DeviceLoginAppId}&grant_type=device_code&code={returnData["device_code"]}");
            body.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";

            var responseMessage = client.PostAsync("https://login.microsoftonline.com/common/oauth2/token", body).GetAwaiter().GetResult();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            while (!responseMessage.IsSuccessStatusCode)
            {
                if (stopWatch.ElapsedMilliseconds > 60 * 1000)
                {
                    break;
                }
                progressCallback(".");
                System.Threading.Thread.Sleep(1000);
                body = new StringContent($"resource={connectionUri.Scheme}://{connectionUri.Host}&client_id={SPOnlineConnection.DeviceLoginAppId}&grant_type=device_code&code={returnData["device_code"]}");
                body.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
                responseMessage = client.PostAsync("https://login.microsoftonline.com/common/oauth2/token", body).GetAwaiter().GetResult();
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TokenResult>(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            }
            else
            {
                progressCallback("Timeout");
                return null;
            }
        }

        internal static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (Utilities.OperatingSystem.IsWindows())
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (Utilities.OperatingSystem.IsLinux())
                {
                    Process.Start("xdg-open", url);
                }
                else if (Utilities.OperatingSystem.IsMacOS())
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
#if !NETSTANDARD2_0
#if !ONPREMISES
        internal static SPOnlineConnection InitiateAzureADNativeApplicationConnection(Uri url, string clientId, Uri redirectUri, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool skipAdminCheck = false, AzureEnvironment azureEnvironment = AzureEnvironment.Production)
        {
            var authManager = new OfficeDevPnP.Core.AuthenticationManager();


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
            var spoConnection = new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag);
            spoConnection.ConnectionMethod = Model.ConnectionMethod.AzureADNativeApplication;
            return spoConnection;
        }

        internal static SPOnlineConnection InitiateAzureADAppOnlyConnection(Uri url, string clientId, string tenant, string certificatePath, SecureString certificatePassword, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool skipAdminCheck = false, AzureEnvironment azureEnvironment = AzureEnvironment.Production)
        {
            var authManager = new OfficeDevPnP.Core.AuthenticationManager();
            var context = PnPClientContext.ConvertFrom(authManager.GetAzureADAppOnlyAuthenticatedContext(url.ToString(), clientId, tenant, certificatePath, certificatePassword, azureEnvironment), retryCount, retryWait * 1000);
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
            var spoConnection = new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag);
            spoConnection.ConnectionMethod = Model.ConnectionMethod.AzureADAppOnly;
            return spoConnection;
        }

        internal static SPOnlineConnection InitiateAzureADAppOnlyConnection(Uri url, string clientId, string tenant, string certificatePEM, string privateKeyPEM, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool skipAdminCheck = false, AzureEnvironment azureEnvironment = AzureEnvironment.Production)
        {
            X509Certificate2 certificate = CertificateHelper.GetCertificateFromPEMstring(certificatePEM, privateKeyPEM);

            var authManager = new OfficeDevPnP.Core.AuthenticationManager();
            var clientContext = authManager.GetAzureADAppOnlyAuthenticatedContext(url.ToString(), clientId, tenant, certificate, azureEnvironment);
            var context = PnPClientContext.ConvertFrom(clientContext, retryCount, retryWait * 1000);
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
            return new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag);
        }
#endif
#endif
#if !ONPREMISES
        internal static SPOnlineConnection InitiateAccessTokenConnection(Uri url, string accessToken, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool skipAdminCheck = false, AzureEnvironment azureEnvironment = AzureEnvironment.Production)
        {
            var authManager = new OfficeDevPnP.Core.AuthenticationManager();
            var context = PnPClientContext.ConvertFrom(authManager.GetAzureADAccessTokenAuthenticatedContext(url.ToString(), accessToken), retryCount, retryWait);
            var connectionType = ConnectionType.O365;
            if (skipAdminCheck == false)
            {
                if (IsTenantAdminSite(context))
                {
                    connectionType = ConnectionType.TenantAdmin;
                }
            }
            var spoConnection = new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag);
            spoConnection.ConnectionMethod = Model.ConnectionMethod.AccessToken;
            return spoConnection;
        }
#endif

#if !NETSTANDARD2_0
        internal static SPOnlineConnection InstantiateWebloginConnection(Uri url, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool skipAdminCheck = false)
        {
            var authManager = new OfficeDevPnP.Core.AuthenticationManager();

            var context = PnPClientContext.ConvertFrom(authManager.GetWebLoginClientContext(url.ToString()), retryCount, retryWait * 1000);

            if (context != null)
            {
                context.RetryCount = retryCount;
                context.Delay = retryWait * 1000;
                context.ApplicationName = Properties.Resources.ApplicationName;
                context.RequestTimeout = requestTimeout;
#if !ONPREMISES
                context.DisableReturnValueCache = true;
#elif SP2016
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
                var spoConnection = new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag);
                spoConnection.ConnectionMethod = Model.ConnectionMethod.WebLogin;
                return spoConnection;
            }
            throw new Exception("Error establishing a connection, context is null");
        }
#endif

        internal static SPOnlineConnection InstantiateSPOnlineConnection(Uri url, PSCredential credentials, PSHost host, bool currentCredentials, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool skipAdminCheck = false, ClientAuthenticationMode authenticationMode = ClientAuthenticationMode.Default)
        {
            var context = new PnPClientContext(url.AbsoluteUri);

            context.RetryCount = retryCount;
            context.Delay = retryWait * 1000;
            context.ApplicationName = Properties.Resources.ApplicationName;
#if !ONPREMISES
            context.DisableReturnValueCache = true;
#elif SP2016
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
            }
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
            var spoConnection = new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, credentials, url.ToString(), tenantAdminUrl, PnPPSVersionTag);
            spoConnection.ConnectionMethod = Model.ConnectionMethod.Credentials;
            return spoConnection;
        }

#if !NETSTANDARD2_0
        internal static SPOnlineConnection InstantiateAdfsConnection(Uri url, PSCredential credentials, PSHost host, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool skipAdminCheck = false)
        {
            var authManager = new OfficeDevPnP.Core.AuthenticationManager();

            var networkCredentials = credentials.GetNetworkCredential();

            string adfsHost;
            string adfsRelyingParty;
            GetAdfsConfigurationFromTargetUri(url, out adfsHost, out adfsRelyingParty);

            if (string.IsNullOrEmpty(adfsHost) || string.IsNullOrEmpty(adfsRelyingParty))
            {
                throw new Exception("Cannot retrieve ADFS settings.");
            }

            var context = PnPClientContext.ConvertFrom(authManager.GetADFSUserNameMixedAuthenticatedContext(url.ToString(), networkCredentials.UserName, networkCredentials.Password, networkCredentials.Domain, adfsHost, adfsRelyingParty), retryCount, retryWait * 1000);

            context.RetryCount = retryCount;
            context.Delay = retryWait * 1000;

            context.ApplicationName = Properties.Resources.ApplicationName;
            context.RequestTimeout = requestTimeout;
#if !ONPREMISES
            context.DisableReturnValueCache = true;
#elif SP2016
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
            var spoConnection = new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag);
            spoConnection.ConnectionMethod = Model.ConnectionMethod.ADFS;
            return spoConnection;
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

        public static void GetAdfsConfigurationFromTargetUri(Uri targetApplicationUri, out string adfsHost, out string adfsRelyingParty)
        {
            adfsHost = "";
            adfsRelyingParty = "";

            var trustEndpoint = new Uri(new Uri(targetApplicationUri.GetLeftPart(UriPartial.Authority)), "/_trust/");
            var request = (HttpWebRequest)WebRequest.Create(trustEndpoint);
            request.AllowAutoRedirect = false;

            try
            {
                using (var response = request.GetResponse())
                {
                    var locationHeader = response.Headers["Location"];
                    if (locationHeader != null)
                    {
                        var redirectUri = new Uri(locationHeader);
                        Dictionary<string, string> queryParameters = Regex.Matches(redirectUri.Query, "([^?=&]+)(=([^&]*))?").Cast<Match>().ToDictionary(x => x.Groups[1].Value, x => Uri.UnescapeDataString(x.Groups[3].Value));
                        adfsHost = redirectUri.Host;
                        adfsRelyingParty = queryParameters["wtrealm"];
                    }
                }
            }
            catch (WebException ex)
            {
                throw new Exception("Endpoint does not use ADFS for authentication.", ex);
            }
        }

        private static bool IsTenantAdminSite(ClientRuntimeContext clientContext)
        {
            try
            {
                var tenant = new Tenant(clientContext);
                clientContext.ExecuteQueryRetry();
                return true;
            }
            catch (ClientRequestException)
            {
                return false;
            }
            catch (ServerException)
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
    }
}
