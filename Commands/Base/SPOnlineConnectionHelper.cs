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
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using SharePointPnP.PowerShell.Commands.Utilities;
using SharePointPnP.PowerShell.Commands.Model;

namespace SharePointPnP.PowerShell.Commands.Base
{
    internal class SPOnlineConnectionHelper
    {

#if DEBUG
        private static readonly Uri VersionCheckUrl = new Uri("https://raw.githubusercontent.com/SharePoint/PnP-PowerShell/dev/version.txt");
#else
        private static readonly Uri VersionCheckUrl = new Uri("https://raw.githubusercontent.com/SharePoint/PnP-PowerShell/master/version.txt");
#endif
        private static bool VersionChecked;

#if !NETSTANDARD2_0
        public static AuthenticationContext AuthContext { get; set; }
#endif
        static SPOnlineConnectionHelper()
        {
        }

        internal static SPOnlineConnection InstantiateSPOnlineConnection(Uri url, string realm, string clientId, string clientSecret, PSHost host, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool disableTelemetry, bool skipAdminCheck = false)
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
#elif SP2016 || SP2019
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
            return new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.SPClientSecret);
        }

#if !NETSTANDARD2_0
#if ONPREMISES
        internal static SPOnlineConnection InstantiateHighTrustConnection(string url, string clientId, string hightrustCertificatePath, string hightrustCertificatePassword, string hightrustCertificateIssuerId, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck)
        {
            var authManager = new OfficeDevPnP.Core.AuthenticationManager();
            var context = authManager.GetHighTrustCertificateAppOnlyAuthenticatedContext(url, clientId, hightrustCertificatePath, hightrustCertificatePassword, hightrustCertificateIssuerId);

            return InstantiateHighTrustConnection(context, url, minimalHealthScore, retryCount, retryWait, requestTimeout, tenantAdminUrl, host, disableTelemetry, skipAdminCheck);
        }

        internal static SPOnlineConnection InstantiateHighTrustConnection(string url, string clientId, System.Security.Cryptography.X509Certificates.X509Certificate2 hightrustCertificate, string hightrustCertificateIssuerId, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck)
        {
            var authManager = new OfficeDevPnP.Core.AuthenticationManager();
            var context = authManager.GetHighTrustCertificateAppOnlyAuthenticatedContext(url, clientId, hightrustCertificate, hightrustCertificateIssuerId);

            return InstantiateHighTrustConnection(context, url, minimalHealthScore, retryCount, retryWait, requestTimeout, tenantAdminUrl, host, disableTelemetry, skipAdminCheck);
        }

        private static SPOnlineConnection InstantiateHighTrustConnection(ClientContext context, string url, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck)
        {
            context.ApplicationName = Properties.Resources.ApplicationName;
            context.RequestTimeout = requestTimeout;
#if SP2016 || SP2019
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
            return new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url, tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.HighTrust);
        }
#endif
#endif

        internal static SPOnlineConnection InstantiateDeviceLoginConnection(string url, bool launchBrowser, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, Action<string> messageCallback, Action<string> progressCallback, Func<bool> cancelRequest, PSHost host, bool disableTelemetry)
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
                        var tokenResult = GetTokenResult(connectionUri, returnData, messageCallback, progressCallback, cancelRequest);
                        if (tokenResult != null)
                        {
                            progressCallback("Token received");
                            spoConnection = new SPOnlineConnection(context, tokenResult, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.DeviceLogin);
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
                    spoConnection = new SPOnlineConnection(context, tokenResult, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.DeviceLogin);
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
                    spoConnection = new SPOnlineConnection(context, tokenResult, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.DeviceLogin);
                }
                else
                {
                    progressCallback("No token received.");
                }
            }
            spoConnection.ConnectionMethod = ConnectionMethod.DeviceLogin;
            return spoConnection;
        }

        internal static SPOnlineConnection InstantiateGraphAccessTokenConnection(string accessToken, PSHost host, bool disableTelemetry)
        {
            var jwtToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(accessToken);
            var tokenResult = new TokenResult();
            tokenResult.AccessToken = accessToken;
            tokenResult.ExpiresOn = jwtToken.ValidTo;
            var spoConnection = new SPOnlineConnection(tokenResult, ConnectionMethod.AccessToken, ConnectionType.O365, 0, 0, 0, PnPPSVersionTag, host, disableTelemetry, InitializationType.Graph);
            spoConnection.ConnectionMethod = ConnectionMethod.GraphDeviceLogin;
            return spoConnection;
        }

        internal static SPOnlineConnection InstantiateGraphDeviceLoginConnection(bool launchBrowser, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, Action<string> messageCallback, Action<string> progressCallback, Func<bool> cancelRequest, PSHost host, bool disableTelemetry)
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
                        var tokenResult = GetTokenResult(connectionUri, returnData, messageCallback, progressCallback, cancelRequest);
                        if (tokenResult != null)
                        {
                            progressCallback("Token received");
                            spoConnection = new SPOnlineConnection(tokenResult, ConnectionMethod.GraphDeviceLogin, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, PnPPSVersionTag, host, disableTelemetry, InitializationType.GraphDeviceLogin);
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
                    spoConnection = new SPOnlineConnection(tokenResult, ConnectionMethod.GraphDeviceLogin, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, PnPPSVersionTag, host, disableTelemetry, InitializationType.GraphDeviceLogin);
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
                    spoConnection = new SPOnlineConnection(tokenResult, ConnectionMethod.GraphDeviceLogin, ConnectionType.O365, minimalHealthScore, retryCount, retryWait, PnPPSVersionTag, host, disableTelemetry, InitializationType.GraphDeviceLogin);
                }
                else
                {
                    progressCallback("No token received.");
                }
            }
            spoConnection.ConnectionMethod = ConnectionMethod.GraphDeviceLogin;
            return spoConnection;
        }



        private static TokenResult GetTokenResult(Uri connectionUri, Dictionary<string, string> returnData, Action<string> messageCallback, Action<string> progressCallback, Func<bool> cancelRequest)
        {
            HttpClient client = new HttpClient();
            var body = new StringContent($"resource={connectionUri.Scheme}://{connectionUri.Host}&client_id={SPOnlineConnection.DeviceLoginAppId}&grant_type=device_code&code={returnData["device_code"]}");
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
                body = new StringContent($"resource={connectionUri.Scheme}://{connectionUri.Host}&client_id={SPOnlineConnection.DeviceLoginAppId}&grant_type=device_code&code={returnData["device_code"]}");
                body.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
                responseMessage = client.PostAsync("https://login.microsoftonline.com/common/oauth2/token", body).GetAwaiter().GetResult();
                shouldCancel = cancelRequest();
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TokenResult>(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
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
        internal static SPOnlineConnection InitiateAzureADNativeApplicationConnection(Uri url, string clientId, Uri redirectUri, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck = false, AzureEnvironment azureEnvironment = AzureEnvironment.Production)
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
            var spoConnection = new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.AADNativeApp);
            spoConnection.ConnectionMethod = Model.ConnectionMethod.AzureADNativeApplication;
            return spoConnection;
        }

        internal static SPOnlineConnection InitiateAzureADAppOnlyConnection(Uri url, string clientId, string tenant, string certificatePath, SecureString certificatePassword, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck = false, AzureEnvironment azureEnvironment = AzureEnvironment.Production)
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
            var spoConnection = new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.AADAppOnly);
            spoConnection.ConnectionMethod = Model.ConnectionMethod.AzureADAppOnly;
            return spoConnection;
        }

        internal static SPOnlineConnection InitiateAzureADAppOnlyConnection(Uri url, string clientId, string tenant, string certificatePEM, string privateKeyPEM, SecureString certificatePassword, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck = false, AzureEnvironment azureEnvironment = AzureEnvironment.Production)
        {
            string password = new System.Net.NetworkCredential(string.Empty, certificatePassword).Password;
            X509Certificate2 certificate = CertificateHelper.GetCertificateFromPEMstring(certificatePEM, privateKeyPEM, password);

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

            CleanupCryptoMachineKey(certificate);

            return new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.AADAppOnly);
        }

        private static void CleanupCryptoMachineKey(X509Certificate2 certificate)
        {
            var privateKey = (RSACryptoServiceProvider)certificate.PrivateKey;
            string uniqueKeyContainerName = privateKey.CspKeyContainerInfo.UniqueKeyContainerName;
            certificate.Reset();
            var path = Environment.GetEnvironmentVariable("ProgramData");
            if (string.IsNullOrEmpty(path)) path = @"C:\ProgramData";
            try
            {
                System.IO.File.Delete(string.Format(@"{0}\Microsoft\Crypto\RSA\MachineKeys\{1}", path, uniqueKeyContainerName));
            }
            catch (Exception)
            {
                // best effort cleanup
            }
        }
#endif
#endif
#if !ONPREMISES
        internal static SPOnlineConnection InitiateAccessTokenConnection(Uri url, string accessToken, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck = false, AzureEnvironment azureEnvironment = AzureEnvironment.Production)
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
            var spoConnection = new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.Token);
            spoConnection.ConnectionMethod = Model.ConnectionMethod.AccessToken;
            return spoConnection;
        }
#endif

#if !NETSTANDARD2_0
        internal static SPOnlineConnection InstantiateWebloginConnection(Uri url, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, PSHost host, bool disableTelemetry, bool skipAdminCheck = false)
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
#elif SP2016 || SP2019
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
                var spoConnection = new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.InteractiveLogin);
                spoConnection.ConnectionMethod = Model.ConnectionMethod.WebLogin;
                return spoConnection;
            }
            throw new Exception("Error establishing a connection, context is null");
        }
#endif

        internal static SPOnlineConnection InstantiateSPOnlineConnection(Uri url, PSCredential credentials, PSHost host, bool currentCredentials, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool disableTelemetry, bool skipAdminCheck = false, ClientAuthenticationMode authenticationMode = ClientAuthenticationMode.Default, bool disableFormsBasedAuthentication = false)
        {
            var context = new PnPClientContext(url.AbsoluteUri);

            context.RetryCount = retryCount;
            context.Delay = retryWait * 1000;
            context.ApplicationName = Properties.Resources.ApplicationName;
#if !ONPREMISES
            context.DisableReturnValueCache = true;
#elif SP2016 || SP2019
            context.DisableReturnValueCache = true;
#endif
            context.RequestTimeout = requestTimeout;

            context.AuthenticationMode = authenticationMode;

            if (authenticationMode == ClientAuthenticationMode.FormsAuthentication)
            {
                var formsAuthInfo = new FormsAuthenticationLoginInfo(credentials.UserName, EncryptionUtility.ToInsecureString(credentials.Password));
                context.FormsAuthenticationLoginInfo = formsAuthInfo;
            }

#if ONPREMISES
            if (disableFormsBasedAuthentication)
            {
                context.ExecutingWebRequest += (sender, args) =>
                {
                    // Disables the forms based authentication, so that SharePoint will (ideally) fall back to Windows Authentication
                    // see https://github.com/SharePoint/PnP-PowerShell/issues/808#issuecomment-369554812 for further details
                    args.WebRequestExecutor.RequestHeaders.Remove("X-FORMS_BASED_AUTH_ACCEPTED");
                    args.WebRequestExecutor.RequestHeaders.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
                };
            }
#endif

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
                        var authManager = new OfficeDevPnP.Core.AuthenticationManager();
                        context = PnPClientContext.ConvertFrom(authManager.GetAzureADCredentialsContext(url.ToString(), credentials.UserName, credentials.Password));
                        context.ExecuteQueryRetry();
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
            var spoConnection = new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, credentials, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.Credentials, disableFormsBasedAuthentication);
            spoConnection.ConnectionMethod = Model.ConnectionMethod.Credentials;
            return spoConnection;
        }

#if !NETSTANDARD2_0
        internal static SPOnlineConnection InstantiateAdfsConnection(Uri url, bool useKerberos, PSCredential credentials, PSHost host, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool disableTelemetry, bool skipAdminCheck = false, string loginProviderName = null)
        {
            var authManager = new OfficeDevPnP.Core.AuthenticationManager();

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

            context.ApplicationName = Properties.Resources.ApplicationName;
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
            var spoConnection = new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl, PnPPSVersionTag, host, disableTelemetry, InitializationType.ADFS);
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
            catch (ClientRequestException x1)
            {
                return false;
            }
            catch (ServerException x2)
            {
                return false;
            }
            catch (WebException x3)
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
                        SPOnlineConnectionHelper.VersionChecked = true;
                        var onlineVersion = client.DownloadString(VersionCheckUrl);
                        onlineVersion = onlineVersion.Trim(new char[] { '\t', '\r', '\n' });
                        var assembly = Assembly.GetExecutingAssembly();
                        var currentVersion = ((AssemblyFileVersionAttribute)assembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version;
                        if (Version.TryParse(onlineVersion, out Version availableVersion))
                        {
                            if (availableVersion > new Version(currentVersion))
                            {
                                return $"A newer version of PnP PowerShell is available: {availableVersion}. Consider upgrading.";
                            }
                        }
                        SPOnlineConnectionHelper.VersionChecked = true;
                    }
                }
            }
            catch
            { }
            return null;
        }
    }
}
