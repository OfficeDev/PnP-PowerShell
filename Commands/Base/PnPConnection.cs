using Microsoft.ApplicationInsights;
using Microsoft.Identity.Client;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Extensions;
using PnP.PowerShell.Commands.Enums;
using PnP.PowerShell.Commands.Model;
using PnP.PowerShell.Core.Attributes;
using PnP.PowerShell.Commands.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using TextCopy;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Base
{
    public class PnPConnection
    {
        #region Constants

        /// <summary>
        /// ClientId of the application registered in Azure Active Directory which should be used for the device oAuth flow
        /// </summary>
        internal const string PnPManagementShellClientId = "31359c7f-bd7e-475c-86db-fdb8c937548e";
        #endregion

        #region Properties

        private HttpClient httpClient;

        public HttpClient HttpClient
        {
            get
            {
                if (httpClient == null)
                {
                    httpClient = new HttpClient();
                }
                return httpClient;
            }
        }
        /// <summary>
        /// User Agent identifier to use on all connections being made to the APIs
        /// </summary>
        internal string UserAgent { get; set; }


        internal ConnectionMethod ConnectionMethod { get; set; }

        /// <summary>
        /// Identifier set on the SharePoint ClientContext as the ClientTag to identify the source of the requests to SharePoint
        /// </summary>
        internal string PnPVersionTag { get; set; }

        internal static List<ClientContext> ContextCache { get; set; }

        public static PnPConnection CurrentConnection { get; internal set; }
        public ConnectionType ConnectionType { get; protected set; }

        /// <summary>
        /// Indication for telemetry through which method a connection has been established
        /// </summary>
        public InitializationType InitializationType { get; protected set; }

        /// <summary>
        /// used to retrieve a new token in case the current token expires
        /// </summary>
        public string[] Scopes { get; internal set; }

        /// <summary>
        /// If provided, it defines the minimal health score the SharePoint server should return back before executing requests on it. Use scale 0 - 10 where 0 is most health and 10 is least healthy. If set to NULL, no health score check will take place.
        /// </summary>
        public int? MinimalHealthScore { get; protected set; }

        public int RetryCount { get; protected set; }
        public int RetryWait { get; protected set; }
        public PSCredential PSCredential { get; protected set; }

        /// <summary>
        /// ClientId under which the connection has been made
        /// </summary>
        public string ClientId { get; protected set; }

        /// <summary>
        /// ClientSecret used to set up the connection
        /// </summary>
        public string ClientSecret { get; protected set; }

        public TelemetryClient TelemetryClient { get; set; }

        public string Url { get; protected set; }

        public string TenantAdminUrl { get; protected set; }

        /// <summary>
        /// Certificate used to set up the connection
        /// </summary>
        public X509Certificate2 Certificate { get; internal set; }

        /// <summary>
        /// Boolean indicating if when using Disconnect-PnPOnline to destruct this PnPConnection instance, if the certificate file should be deleted from C:\ProgramData\Microsoft\Crypto\RSA\MachineKeys
        /// </summary>
        public bool DeleteCertificateFromCacheOnDisconnect { get; internal set; }

        public ClientContext Context { get; set; }

        /// <summary>
        /// Tenant name to which the connection exists
        /// </summary>
        public string Tenant { get; set; }

        #endregion

        #region Fields

        /// <summary>
        /// Collection with all available access tokens in the current session to access APIs. Key is the token audience, value is the JWT token itself.
        /// </summary>
        private readonly Dictionary<TokenAudience, GenericToken> AccessTokens = new Dictionary<TokenAudience, GenericToken>();

        #endregion

        #region Methods

        /// <summary>
        /// Tries to get an access token for the provided audience
        /// </summary>
        /// <param name="tokenAudience">Audience to try to get an access token for</param>
        /// <param name="roles">The specific roles to request access to (i.e. Group.ReadWrite.All). Optional, will use default roles assigned to clientId if not specified.</param>
        /// <returns>AccessToken for the audience or NULL if unable to retrieve an access token for the audience on the current connection</returns>
        internal string TryGetAccessToken(TokenAudience tokenAudience, string[] roles = null)
        {
            return TryGetToken(tokenAudience, roles)?.AccessToken;
        }

        internal static Action<DeviceCodeResult> DeviceLoginCallback(PSHost host, bool launchBrowser)
        {
            return deviceCodeResult =>
            {

                if (launchBrowser)
                {
                    ClipboardService.SetText(deviceCodeResult.UserCode);
                    host?.UI.WriteLine($"Code {deviceCodeResult.UserCode} has been copied to clipboard");
                    BrowserHelper.LaunchBrowser(deviceCodeResult.VerificationUrl);
                }
                else
                {
                    host?.UI.WriteLine(deviceCodeResult.Message);
                }
            };
        }
        /// <summary>
        /// Tries to get a token for the provided audience
        /// </summary>
        /// <param name="tokenAudience">Audience to try to get a token for</param>
        /// <param name="orRoles">The specific roles to request access to (i.e. Group.ReadWrite.All). Optional, will use default groups assigned to clientId if not specified.</param>
        /// <returns><see cref="GenericToken"/> for the audience or NULL if unable to retrieve a token for the audience on the current connection</returns>
        internal GenericToken TryGetToken(TokenAudience tokenAudience, string[] orRoles = null, string[] andRoles = null, TokenType tokenType = TokenType.All)
        {
            GenericToken token = null;

            switch (tokenAudience)
            {
                case TokenAudience.MicrosoftGraph:

                    if (ConnectionMethod == ConnectionMethod.DeviceLogin || ConnectionMethod == ConnectionMethod.GraphDeviceLogin)
                    {
                        var officeManagementApiScopes = Enum.GetNames(typeof(OfficeManagementApiPermission)).Select(s => s.Replace("_", ".")).Intersect(Scopes).ToArray();
                        // Take the remaining scopes and try requesting them from the Microsoft Graph API
                        var scopes = Scopes.Except(officeManagementApiScopes).ToArray();
                        token = GraphToken.AcquireApplicationTokenDeviceLogin(PnPConnection.PnPManagementShellClientId, scopes, DeviceLoginCallback(null, false));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Tenant))
                        {
                            if (Certificate != null)
                            {
                                token = GraphToken.AcquireApplicationToken(Tenant, ClientId, Certificate);
                            }
                            else if (ClientSecret != null)
                            {
                                token = GraphToken.AcquireApplicationToken(Tenant, ClientId, ClientSecret);
                            }
                            else if (Scopes != null)
                            {
                                var officeManagementApiScopes = Enum.GetNames(typeof(OfficeManagementApiPermission)).Select(s => s.Replace("_", ".")).Intersect(Scopes).ToArray();
                                // Take the remaining scopes and try requesting them from the Microsoft Graph API
                                var scopes = Scopes.Except(officeManagementApiScopes).ToArray();
                                if (scopes.Length > 0)
                                {
                                    token = PSCredential == null ? GraphToken.AcquireApplicationTokenInteractive(PnPManagementShellClientId, scopes) : GraphToken.AcquireDelegatedTokenWithCredentials(PnPManagementShellClientId, scopes, PSCredential.UserName, PSCredential.Password);
                                }
                                else
                                {
                                    throw new PSSecurityException($"Access to {tokenAudience} failed because you did not connect with any permission scopes related to this service (for instance 'Group.Read.All').");
                                }
                            }
                        }
                    }
                    break;

                case TokenAudience.OfficeManagementApi:
                    if (!string.IsNullOrEmpty(Tenant))
                    {
                        if (Certificate != null)
                        {
                            token = OfficeManagementApiToken.AcquireApplicationToken(Tenant, ClientId, Certificate);
                        }
                        else if (ClientSecret != null)
                        {
                            token = OfficeManagementApiToken.AcquireApplicationToken(Tenant, ClientId, ClientSecret);
                        }
                        else if (Scopes != null)
                        {
                            var scopes = Enum.GetNames(typeof(OfficeManagementApiPermission)).Select(s => s.Replace("_", ".")).Intersect(Scopes).ToArray();
                            // Take the remaining scopes and try requesting them from the Microsoft Graph API
                            if (scopes.Length > 0)
                            {
                                token = PSCredential == null ? OfficeManagementApiToken.AcquireApplicationTokenInteractive(PnPManagementShellClientId, scopes) : OfficeManagementApiToken.AcquireDelegatedTokenWithCredentials(PnPManagementShellClientId, scopes, PSCredential.UserName, PSCredential.Password);
                            }
                            else
                            {
                                throw new PSSecurityException($"Access to {tokenAudience} failed because you did not connect with any permission scopes related to this service (for instance 'ServiceHealth.Read').");
                            }
                        }

                    }
                    break;

                case TokenAudience.SharePointOnline:
                    // This is not a token type we can request on demand
                    return null;
            }

            if (token != null)
            {
                var validationResults = ValidateTokenForPermissions(token, tokenAudience, orRoles, andRoles, tokenType);
                if (!validationResults.valid)
                {
                    throw new PSSecurityException($"Access to {tokenAudience} failed because the app registration {ClientId} in tenant {Tenant} is not granted {validationResults.message}");
                }
                return token;
            }

            // Didn't have a token yet and unable to retrieve one
            return null;
        }

        /// <summary>
        /// Adds the provided token to the available tokens in the current connection
        /// </summary>
        /// <param name="tokenAudience">Audience the token is for</param>
        /// <param name="token">The token to add</param>
        internal void AddToken(TokenAudience tokenAudience, GenericToken token)
        {
            AccessTokens[tokenAudience] = token;
        }

        /// <summary>
        /// Clears all available tokens on the current connection
        /// </summary>
        internal void ClearTokens()
        {
            AccessTokens.Clear();
        }

        private (bool valid, string message) ValidateTokenForPermissions(GenericToken token, TokenAudience tokenAudience, string[] orRoles = null, string[] andRoles = null, TokenType tokenType = TokenType.All)
        {
            bool valid = false;
            var message = string.Empty;
            if (tokenType != TokenType.All && token.TokenType != tokenType)
            {
                throw new PSSecurityException($"Access to {tokenAudience} failed because the API requires {(tokenType == TokenType.Application ? "an" : "a")} {tokenType} token while you currently use {(token.TokenType == TokenType.Application ? "an" : "a")} {token.TokenType} token.");
            }
            var andRolesMatched = false;
            if (andRoles != null && andRoles.Length != 0)
            {
                // we have explicitely required roles
                andRolesMatched = andRoles.All(r => token.Roles.Contains(r));
            }
            else
            {
                andRolesMatched = true;
            }

            var orRolesMatched = false;
            if (orRoles != null && orRoles.Length != 0)
            {
                orRolesMatched = orRoles.Any(r => token.Roles.Contains(r));
            }
            else
            {
                orRolesMatched = true;
            }

            if (orRolesMatched && andRolesMatched)
            {
                valid = true;
            }

            if (orRoles != null || andRoles != null)
            {
                if (!valid)
                {                // Requested role was not part of the access token, throw an exception explaining which application registration is missing which role
                    if (!orRolesMatched)
                    {
                        message += "for one of the following roles: " + string.Join(", ", orRoles);
                    }
                    if (!andRolesMatched)
                    {
                        message += (message != string.Empty ? ", and " : ", ") + "for all of the following roles: " + string.Join(", ", andRoles);
                    }
                    throw new PSSecurityException($"Access to {tokenAudience} failed because the app registration {ClientId} in tenant {Tenant} is not granted {message}");
                }
            }
            return (valid, message);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a basic new PnP Connection. Use one of the static methods to retrieve a PnPConnection for a specific purpose.
        /// </summary>
        /// <param name="host">PowerShell Host environment in which the commands are being run</param>
        /// <param name="initializationType">Indicator of type <see cref="InitializationType"/> which indicates the method used to set up the connection. Used for gathering usage analytics.</param>
        /// <param name="url">Url of the SharePoint environment to connect to, if applicable. Leave NULL not to connect to a SharePoint environment.</param>
        /// <param name="clientContext">A SharePoint ClientContext to make available within this connection. Leave NULL to not connect to a SharePoint environment.</param>
        /// <param name="minimalHealthScore">Minimum health score that the SharePoint server should report before allowing requests to be executed on it. Scale of 0 to 10 where 0 is healthiest and 10 is least healthy. Leave NULL not to perform health checks on SharePoint.</param>
        /// <param name="pnpVersionTag">Identifier set on the SharePoint ClientContext as the ClientTag to identify the source of the requests to SharePoint. Leave NULL not to set it.</param>
        /// <param name="disableTelemetry">Boolean indicating if telemetry on the commands being executed should be disabled. Telemetry is enabled by default.</param>
        private PnPConnection(PSHost host,
                              InitializationType initializationType,
                              string url = null,
                              ClientContext clientContext = null,
                              Dictionary<TokenAudience, GenericToken> tokens = null,
                              int? minimalHealthScore = null,
                              string pnpVersionTag = null,
                              bool disableTelemetry = false)
        {
            if (!disableTelemetry)
            {
                InitializeTelemetry(clientContext, host, initializationType);
            }

            UserAgent = $"NONISV|SharePointPnP|PnPPS/{((AssemblyFileVersionAttribute)Assembly.GetExecutingAssembly().GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version}";
            Context = clientContext;

            // Enrich the AccessTokens collection with the token(s) passed in
            if (tokens != null)
            {
                AccessTokens.AddRange(tokens);
            }

            // Validate if we have a SharePoint Context
            if (Context != null)
            {
                // We have a SharePoint Context, configure the context
                ContextCache = new List<ClientContext> { Context };

#if !ONPREMISES
                // If we have a SharePoint or a Graph Access Token, use it for the SharePoint connection
                var accessToken = AccessTokens.ContainsKey(TokenAudience.SharePointOnline) ? TryGetAccessToken(TokenAudience.SharePointOnline) : TryGetAccessToken(TokenAudience.MicrosoftGraph);
                if (accessToken != null)
                {
                    Context.ExecutingWebRequest += (sender, args) =>
                    {
                        args.WebRequestExecutor.WebRequest.UserAgent = UserAgent;
                        args.WebRequestExecutor.RequestHeaders["Authorization"] = "Bearer " + accessToken;
                    };
                }
#endif
            }
            else
            {
                // We do not have a SharePoint Context
                ContextCache = null;
            }

            PnPVersionTag = pnpVersionTag;
            MinimalHealthScore = minimalHealthScore;
            Url = url;
        }

        #endregion

        #region Connection Creation

        /// <summary>
        /// Returns a PnPConnection based on connecting using a ClientId and ClientSecret
        /// </summary>
        /// <param name="clientId">ClientId to connect with</param>
        /// <param name="clientSecret">ClientSecret to connect with</param>
        /// <param name="aadDomain">The Azure Active Directory tenant name (i.e. contoso.onmicrosoft.com) or the tenant identifier to which to connect</param>
        /// <param name="host">PowerShell Host environment in which the commands are being run</param>
        /// <param name="initializationType">Indicator of type <see cref="InitializationType"/> which indicates the method used to set up the connection. Used for gathering usage analytics.</param>
        /// <param name="url">Url of the SharePoint environment to connect to, if applicable. Leave NULL not to connect to a SharePoint environment.</param>
        /// <param name="clientContext">A SharePoint ClientContext to make available within this connection. Leave NULL to not connect to a SharePoint environment.</param>
        /// <param name="minimalHealthScore">Minimum health score that the SharePoint server should report before allowing requests to be executed on it. Scale of 0 to 10 where 0 is healthiest and 10 is least healthy. Leave NULL not to perform health checks on SharePoint.</param>
        /// <param name="pnpVersionTag">Identifier set on the SharePoint ClientContext as the ClientTag to identify the source of the requests to SharePoint. Leave NULL not to set it.</param>
        /// <param name="disableTelemetry">Boolean indicating if telemetry on the commands being executed should be disabled. Telemetry is enabled by default.</param>
        /// <returns><see cref="PnPConnection"/ instance which can be used to communicate with one of the supported APIs</returns>
        public static PnPConnection GetConnectionWithClientIdAndClientSecret(string clientId,
                                                                             string clientSecret,
                                                                             PSHost host,
                                                                             InitializationType initializationType,
                                                                             string url = null,
                                                                             string aadDomain = null,
                                                                             ClientContext clientContext = null,
                                                                             int? minimalHealthScore = null,
                                                                             string pnpVersionTag = null,
                                                                             bool disableTelemetry = false)
        {
            return new PnPConnection(host, initializationType, url, clientContext, null, minimalHealthScore, pnpVersionTag, disableTelemetry)
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                ConnectionMethod = ConnectionMethod.AzureADAppOnly,
                Tenant = aadDomain
            };
        }

        /// <summary>
        /// Returns a PnPConnection based on connecting using a ClientId and Certificate
        /// </summary>
        /// <param name="clientId">ClientId to connect with</param>
        /// <param name="certificate">Certificate to connect with</param>
        /// <param name="aadDomain">The Azure Active Directory tenant name (i.e. contoso.onmicrosoft.com) or the tenant identifier to which to connect</param>
        /// <param name="host">PowerShell Host environment in which the commands are being run</param>
        /// <param name="initializationType">Indicator of type <see cref="InitializationType"/> which indicates the method used to set up the connection. Used for gathering usage analytics.</param>
        /// <param name="url">Url of the SharePoint environment to connect to, if applicable. Leave NULL not to connect to a SharePoint environment.</param>
        /// <param name="clientContext">A SharePoint ClientContext to make available within this connection. Leave NULL to not connect to a SharePoint environment.</param>
        /// <param name="minimalHealthScore">Minimum health score that the SharePoint server should report before allowing requests to be executed on it. Scale of 0 to 10 where 0 is healthiest and 10 is least healthy. Leave NULL not to perform health checks on SharePoint.</param>
        /// <param name="pnpVersionTag">Identifier set on the SharePoint ClientContext as the ClientTag to identify the source of the requests to SharePoint. Leave NULL not to set it.</param>
        /// <param name="disableTelemetry">Boolean indicating if telemetry on the commands being executed should be disabled. Telemetry is enabled by default.</param>
        /// <returns><see cref="PnPConnection"/ instance which can be used to communicate with one of the supported APIs</returns>
        public static PnPConnection GetConnectionWithClientIdAndCertificate(string clientId,
                                                                            X509Certificate2 certificate,
                                                                            PSHost host,
                                                                            InitializationType initializationType,
                                                                            string url = null,
                                                                            string aadDomain = null,
                                                                            ClientContext clientContext = null,
                                                                            int? minimalHealthScore = null,
                                                                            string pnpVersionTag = null,
                                                                            bool disableTelemetry = false)
        {
            return new PnPConnection(host, initializationType, url, clientContext, null, minimalHealthScore, pnpVersionTag, disableTelemetry)
            {
                ClientId = clientId,
                Certificate = certificate,
                ConnectionMethod = ConnectionMethod.AzureADAppOnly,
                Tenant = aadDomain
            };
        }

        /// <summary>
        /// Returns a PnPConnection based on connecting using an username and password
        /// </summary>
        /// <param name="credential">Credential set to connect with</param>
        /// <param name="host">PowerShell Host environment in which the commands are being run</param>
        /// <param name="initializationType">Indicator of type <see cref="InitializationType"/> which indicates the method used to set up the connection. Used for gathering usage analytics.</param>
        /// <param name="url">Url of the SharePoint environment to connect to, if applicable. Leave NULL not to connect to a SharePoint environment.</param>
        /// <param name="clientContext">A SharePoint ClientContext to make available within this connection. Leave NULL to not connect to a SharePoint environment.</param>
        /// <param name="minimalHealthScore">Minimum health score that the SharePoint server should report before allowing requests to be executed on it. Scale of 0 to 10 where 0 is healthiest and 10 is least healthy. Leave NULL not to perform health checks on SharePoint.</param>
        /// <param name="pnpVersionTag">Identifier set on the SharePoint ClientContext as the ClientTag to identify the source of the requests to SharePoint. Leave NULL not to set it.</param>
        /// <param name="disableTelemetry">Boolean indicating if telemetry on the commands being executed should be disabled. Telemetry is enabled by default.</param>
        /// <returns><see cref="PnPConnection"/ instance which can be used to communicate with one of the supported APIs</returns>
        public static PnPConnection GetConnectionWithPsCredential(PSCredential credential,
                                                                  PSHost host,
                                                                  InitializationType initializationType,
                                                                  string url = null,
                                                                  ClientContext clientContext = null,
                                                                  int? minimalHealthScore = null,
                                                                  string pnpVersionTag = null,
                                                                  bool disableTelemetry = false)
        {
            return new PnPConnection(host, initializationType, url, clientContext, null, minimalHealthScore, pnpVersionTag, disableTelemetry)
            {
                PSCredential = credential,
                ConnectionMethod = ConnectionMethod.Credentials
            };
        }

        /// <summary>
        /// Returns a PnPConnection based on connecting using an existing token
        /// </summary>
        /// <param name="token">Token to connect with</param>
        /// <param name="tokenAudience">Indicator of <see cref="TokenAudience"/> indicating for which API this token is meant to be used</param>
        /// <param name="host">PowerShell Host environment in which the commands are being run</param>
        /// <param name="initializationType">Indicator of type <see cref="InitializationType"/> which indicates the method used to set up the connection. Used for gathering usage analytics.</param>
        /// <param name="url">Url of the SharePoint environment to connect to, if applicable. Leave NULL not to connect to a SharePoint environment.</param>
        /// <param name="clientContext">A SharePoint ClientContext to make available within this connection. Leave NULL to not connect to a SharePoint environment.</param>
        /// <param name="minimalHealthScore">Minimum health score that the SharePoint server should report before allowing requests to be executed on it. Scale of 0 to 10 where 0 is healthiest and 10 is least healthy. Leave NULL not to perform health checks on SharePoint.</param>
        /// <param name="pnpVersionTag">Identifier set on the SharePoint ClientContext as the ClientTag to identify the source of the requests to SharePoint. Leave NULL not to set it.</param>
        /// <param name="disableTelemetry">Boolean indicating if telemetry on the commands being executed should be disabled. Telemetry is enabled by default.</param>
        /// <returns><see cref="PnPConnection"/ instance which can be used to communicate with one of the supported APIs</returns>
        public static PnPConnection GetConnectionWithToken(GenericToken token,
                                                           TokenAudience tokenAudience,
                                                           PSHost host,
                                                           InitializationType initializationType,
                                                           PSCredential credentials,
                                                           string url = null,
                                                           ClientContext clientContext = null,
                                                           int? minimalHealthScore = null,
                                                           string pnpVersionTag = null,
                                                           bool disableTelemetry = false)
        {
            var connection = new PnPConnection(host, initializationType, url, clientContext, new Dictionary<TokenAudience, GenericToken>(1) { { tokenAudience, token } }, minimalHealthScore, pnpVersionTag, disableTelemetry)
            {
                ConnectionMethod = ConnectionMethod.AccessToken,
                Tenant = token.ParsedToken.Claims.FirstOrDefault(c => c.Type.Equals("tid", StringComparison.InvariantCultureIgnoreCase))?.Value,
                ClientId = token.ParsedToken.Claims.FirstOrDefault(c => c.Type.Equals("appid", StringComparison.InvariantCultureIgnoreCase))?.Value
            };
            connection.PSCredential = credentials;
            return connection;
        }

        #endregion

        internal PnPConnection(ClientContext context, ConnectionType connectionType, int minimalHealthScore, int retryCount, int retryWait, PSCredential credential, string clientId, string clientSecret, string url, string tenantAdminUrl, string pnpVersionTag, PSHost host, bool disableTelemetry, InitializationType initializationType)
        : this(context, connectionType, minimalHealthScore, retryCount, retryWait, credential, url, tenantAdminUrl, pnpVersionTag, host, disableTelemetry, initializationType)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        internal PnPConnection(ClientContext context,
                                    ConnectionType connectionType,
                                    int minimalHealthScore,
                                    int retryCount,
                                    int retryWait,
                                    PSCredential credential,
                                    string url,
                                    string tenantAdminUrl,
                                    string pnpVersionTag,
                                    PSHost host,
                                    bool disableTelemetry,
                                    InitializationType initializationType)
        {
            if (!disableTelemetry)
            {
                InitializeTelemetry(context, host, initializationType);
            }
            var coreAssembly = Assembly.GetExecutingAssembly();
            UserAgent = $"NONISV|SharePointPnP|PnPPS/{((AssemblyFileVersionAttribute)coreAssembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version}";
            //if (context == null)
            //    throw new ArgumentNullException(nameof(context));
            Context = context;
            Context.ExecutingWebRequest += Context_ExecutingWebRequest;

            ConnectionType = connectionType;
            MinimalHealthScore = minimalHealthScore;
            RetryCount = retryCount;
            RetryWait = retryWait;
            TenantAdminUrl = tenantAdminUrl;

            PSCredential = credential;
            PnPVersionTag = pnpVersionTag;
            ContextCache = new List<ClientContext> { context };
            Url = (new Uri(url)).AbsoluteUri;
            ConnectionMethod = ConnectionMethod.Credentials;
        }

        internal PnPConnection(ClientContext context, GenericToken tokenResult, ConnectionType connectionType, int minimalHealthScore, int retryCount, int retryWait, PSCredential credential, string url, string tenantAdminUrl, string pnpVersionTag, PSHost host, bool disableTelemetry, InitializationType initializationType)
        {
            if (!disableTelemetry)
            {
                InitializeTelemetry(context, host, initializationType);
            }

            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var coreAssembly = Assembly.GetExecutingAssembly();
            UserAgent = $"NONISV|SharePointPnP|PnPPS/{((AssemblyFileVersionAttribute)coreAssembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version}";
            Context = context;
            ConnectionType = connectionType;
            MinimalHealthScore = minimalHealthScore;
            RetryCount = retryCount;
            RetryWait = retryWait;
            PSCredential = credential;
            TenantAdminUrl = tenantAdminUrl;
            ContextCache = new List<ClientContext> { context };
            PnPVersionTag = pnpVersionTag;
            Url = (new Uri(url)).AbsoluteUri;
            ConnectionMethod = ConnectionMethod.AccessToken;
            ClientId = PnPManagementShellClientId;
            Tenant = tokenResult.ParsedToken.Claims.FirstOrDefault(c => c.Type == "tid").Value;
            context.ExecutingWebRequest += (sender, args) =>
            {
                args.WebRequestExecutor.WebRequest.UserAgent = UserAgent;
                args.WebRequestExecutor.RequestHeaders["Authorization"] = "Bearer " + tokenResult.AccessToken;
            };
        }

        internal PnPConnection(GenericToken tokenResult, ConnectionMethod connectionMethod, ConnectionType connectionType, int minimalHealthScore, int retryCount, int retryWait, string pnpVersionTag, PSHost host, bool disableTelemetry, InitializationType initializationType)
        {
            if (!disableTelemetry)
            {
                InitializeTelemetry(null, host, initializationType);
            }
            var coreAssembly = Assembly.GetExecutingAssembly();
            UserAgent = $"NONISV|SharePointPnP|PnPPS/{((AssemblyFileVersionAttribute)coreAssembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version}";
            ConnectionType = connectionType;
            MinimalHealthScore = minimalHealthScore;
            RetryCount = retryCount;
            RetryWait = retryWait;
            PnPVersionTag = pnpVersionTag;
            ConnectionMethod = connectionMethod;
        }

        internal PnPConnection(ConnectionMethod connectionMethod, ConnectionType connectionType, int minimalHealthScore, int retryCount, int retryWait, string pnpVersionTag, PSHost host, bool disableTelemetry, InitializationType initializationType)
        {
            if (!disableTelemetry)
            {
                InitializeTelemetry(null, host, initializationType);
            }
            var coreAssembly = Assembly.GetExecutingAssembly();
            UserAgent = $"NONISV|SharePointPnP|PnPPS/{((AssemblyFileVersionAttribute)coreAssembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version}";
            ConnectionType = connectionType;
            MinimalHealthScore = minimalHealthScore;
            RetryCount = retryCount;
            RetryWait = retryWait;
            PnPVersionTag = pnpVersionTag;
            ConnectionMethod = connectionMethod;
        }

        private void Context_ExecutingWebRequest(object sender, WebRequestEventArgs e)
        {
            e.WebRequestExecutor.WebRequest.UserAgent = UserAgent;
        }

        internal void RestoreCachedContext(string url)
        {
            Context = ContextCache.FirstOrDefault(c => new Uri(c.Url).AbsoluteUri == new Uri(url).AbsoluteUri);
        }

        internal void CacheContext()
        {
            var c = ContextCache.FirstOrDefault(cc => new Uri(cc.Url).AbsoluteUri == new Uri(Context.Url).AbsoluteUri);
            if (c == null)
            {
                ContextCache.Add(Context);
            }
        }

        internal ClientContext CloneContext(string url)
        {
            var context = ContextCache.FirstOrDefault(c => new Uri(c.Url).AbsoluteUri == new Uri(url).AbsoluteUri);
            if (context == null)
            {
                context = Context.Clone(url);
                try
                {
                    context.ExecuteQueryRetry();
                }
                catch (Exception ex)
                {
#if !ONPREMISES && !PNPPSCORE
                    if ((ex is WebException || ex is NotSupportedException) && CurrentConnection.PSCredential != null)
                    {
                        // legacy auth?
                        using (var authManager = new OfficeDevPnP.Core.AuthenticationManager())
                        {
                            context = authManager.GetAzureADCredentialsContext(url.ToString(), CurrentConnection.PSCredential.UserName, CurrentConnection.PSCredential.Password);
                        }
                        context.ExecuteQueryRetry();
                    }
                    else
                    {
#endif
                        throw;
#if !ONPREMISES && !PNPPSCORE
                    }
#endif
                }
                ContextCache.Add(context);
            }
            Context = context;
            return context;
        }

        internal static ClientContext GetCachedContext(string url)
        {
            return ContextCache.FirstOrDefault(c => HttpUtility.UrlEncode(c.Url) == HttpUtility.UrlEncode(url));
        }

        internal static void ClearContextCache()
        {
            ContextCache.Clear();
        }

        internal void InitializeTelemetry(ClientContext context, PSHost host, InitializationType initializationType)
        {

            var enableTelemetry = false;
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var telemetryFile = System.IO.Path.Combine(userProfile, ".pnppowershelltelemetry");

            if (!System.IO.File.Exists(telemetryFile))
            {
#if ONPREMISES
                if (Environment.UserInteractive && Environment.GetCommandLineArgs().FirstOrDefault(a => a.ToLower().StartsWith("-noni")) == null)
                {
                    var choices = new System.Collections.ObjectModel.Collection<ChoiceDescription>();
                    choices.Add(new ChoiceDescription("&Allow", "You will allow us to transmit anonymous data"));
                    choices.Add(new ChoiceDescription("&Do not allow", "You do not allow us to transmit anonymous data"));
                    if (host.UI.PromptForChoice("PnP PowerShell Telemetry", $"Please allow us to transmit anonymous metrics in order to make PnP PowerShell even better.{Environment.NewLine}We transmit the version of PnP PowerShell you are using, the version of SharePoint you are connecting to and which cmdlet you are executing. We do not transmit tenant/server URLs nor parameter values and content.{Environment.NewLine}{Environment.NewLine}Your decision will be recorded in a file a called .pnppowershelltelemetry which will be located in your profile folder ({userProfile}).{Environment.NewLine}{Environment.NewLine}You can choose to disable and/or enable telemetry at a later stage by using Enable-PnPPowerShellTelemetry or Disable-PnPPowerShellTelemetry. Get-PnPPowerShellTelemetryEnabled will provide you with your current setting.", choices, 0) == 0)
                    {
                        enableTelemetry = true;
                        System.IO.File.WriteAllText(telemetryFile, "allow");
                    }
                    else
                    {
                        System.IO.File.WriteAllText(telemetryFile, "disallow");
                    }
                }
#else
                enableTelemetry = true;
#endif

            }
            else
            {
                if (System.IO.File.ReadAllText(telemetryFile).ToLower() == "allow")
                {
                    enableTelemetry = true;
                }
            }
            if (enableTelemetry)
            {
                var serverLibraryVersion = "";
                var serverVersion = "";
                if (context != null)
                {
                    try
                    {
                        if (context.ServerLibraryVersion != null)
                        {
                            serverLibraryVersion = context.ServerLibraryVersion.ToString();
                        }
                        if (context.ServerVersion != null)
                        {
                            serverVersion = context.ServerVersion.ToString();
                        }
                    }
                    catch { }
                }
                TelemetryClient = new TelemetryClient();
                TelemetryClient.InstrumentationKey = "a301024a-9e21-4273-aca5-18d0ef5d80fb";
                TelemetryClient.Context.Session.Id = Guid.NewGuid().ToString();
                TelemetryClient.Context.Cloud.RoleInstance = "PnPPowerShell";
                TelemetryClient.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
#if !PNPPSCORE
                TelemetryClient.Context.GlobalProperties.Add("ServerLibraryVersion", serverLibraryVersion);
                TelemetryClient.Context.GlobalProperties.Add("ServerVersion", serverVersion);
                TelemetryClient.Context.GlobalProperties.Add("ConnectionMethod", initializationType.ToString());
#else
                TelemetryClient.Context.Properties.Add("ServerLibraryVersion", serverLibraryVersion);
                TelemetryClient.Context.Properties.Add("ServerVersion", serverVersion);
                TelemetryClient.Context.Properties.Add("ConnectionMethod", initializationType.ToString());
#endif
                var coreAssembly = Assembly.GetExecutingAssembly();
#if !PNPPSCORE
                TelemetryClient.Context.GlobalProperties.Add("Version", ((AssemblyFileVersionAttribute)coreAssembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version.ToString());
#else
                TelemetryClient.Context.Properties.Add("Version", ((AssemblyFileVersionAttribute)coreAssembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version.ToString());
#endif

#if SP2013
            TelemetryClient.Context.GlobalProperties.Add("Platform", "SP2013");
#elif SP2016
            TelemetryClient.Context.GlobalProperties.Add("Platform", "SP2016");
#elif SP2019
            TelemetryClient.Context.GlobalProperties.Add("Platform", "SP2019");
#else
#if !PNPPSCORE
                TelemetryClient.Context.GlobalProperties.Add("Platform", "SPO");
#else
                TelemetryClient.Context.Properties.Add("Platform", "SPO");
#endif
#endif
                TelemetryClient.TrackEvent("Connect-PnPOnline");
            }
        }
    }
}