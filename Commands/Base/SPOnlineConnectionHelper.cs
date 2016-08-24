using Microsoft.IdentityModel.Clients.ActiveDirectory;
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
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using OfficeDevPnP.Core;
using OfficeDevPnP.Core.Utilities;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SharePointPnP.PowerShell.Commands.Base
{
    internal class SPOnlineConnectionHelper
    {
        private const string CommonAuthority = "https://login.windows.net/Common";
        public static AuthenticationContext AuthContext { get; set; }
        private static string ContextUrl { get; set; }

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

            var context = PnPClientContext.ConvertFrom(authManager.GetAppOnlyAuthenticatedContext(url.ToString(), realm, clientId, clientSecret), retryCount, retryWait * 1000);
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
            return new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl);
        }


#if !ONPREMISES
        internal static SPOnlineConnection InitiateAzureADNativeApplicationConnection(Uri url, string clientId, Uri redirectUri, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool skipAdminCheck = false)
        {
            var authManager = new OfficeDevPnP.Core.AuthenticationManager();


            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string configFile = Path.Combine(appDataFolder, "SharePointPnP.PowerShell\\tokencache.dat");
            FileTokenCache cache = new FileTokenCache(configFile);

            var context = PnPClientContext.ConvertFrom(authManager.GetAzureADNativeApplicationAuthenticatedContext(url.ToString(), clientId, redirectUri, cache), retryCount, retryWait * 10000);

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
            return new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl);
        }

        internal static SPOnlineConnection InitiateAzureADAppOnlyConnection(Uri url, string clientId, string tenant, string certificatePath, SecureString certificatePassword, int minimalHealthScore, int retryCount, int retryWait, int requestTimeout, string tenantAdminUrl, bool skipAdminCheck = false)
        {
            var authManager = new OfficeDevPnP.Core.AuthenticationManager();
            var context = PnPClientContext.ConvertFrom(authManager.GetAzureADAppOnlyAuthenticatedContext(url.ToString(), clientId, tenant, certificatePath, certificatePassword), retryCount, retryWait * 1000);
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
            return new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl);
        }
#endif

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

                return new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl);
            }
            throw new Exception("Error establishing a connection, context is null");
        }

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
            return new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, credentials, url.ToString(), tenantAdminUrl);
        }

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
            return new SPOnlineConnection(context, connectionType, minimalHealthScore, retryCount, retryWait, null, url.ToString(), tenantAdminUrl);
        }

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
    }
}
