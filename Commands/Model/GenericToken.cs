using OfficeDevPnP.Core.Utilities;
using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using PnP.PowerShell.Core.Attributes;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Identity.Client;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Security;

namespace PnP.PowerShell.Commands.Model
{
    /// <summary>
    /// Contains a JWT oAuth token
    /// </summary>
    public class GenericToken
    {
        private static IPublicClientApplication publicClientApplication;
        private static IConfidentialClientApplication confidentialClientApplication;

        /// <summary>
        /// The default authority
        /// </summary>
        public static string BaseAuthority = "https://login.microsoftonline.com/";

        /// <summary>
        /// The name of the default scope
        /// </summary>
        public static string DefaultScope = ".default";

        /// <summary>
        /// Token which can be used to access the API
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; private set; }

        /// <summary>
        /// Token that can be used to retrieve a new access token
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; protected set; }

        /// <summary>
        /// Date and time at which the current access token will no longer be valid to be used
        /// </summary>
        public DateTime ExpiresOn { get; protected set; }

        /// <summary>
        /// The APIs being targeted (i.e. https://graph.microsoft.com)
        /// </summary>
        public string[] Audiences { get; protected set; }

        /// <summary>
        /// The roles being given to the token (i.e. Group.ReadWrite.All)
        /// </summary>
        public string[] Roles { get; protected set; }

        /// <summary>
        /// The parsed access token into JWT
        /// </summary>
        public System.IdentityModel.Tokens.Jwt.JwtSecurityToken ParsedToken { get; private set; }

        public Enums.TokenAudience TokenAudience { get; protected set; }

        /// <summary>
        /// The unique identifier of the tenant to which the token belongs
        /// </summary>
        public Guid? TenantId { get; protected set; }

        /// <summary>
        /// The type of the token, e.g. Application or Generic
        /// </summary>
        public TokenType TokenType { get; protected set; }
        /// <summary>
        /// Instantiates a new token
        /// </summary>
        /// <param name="accesstoken">Accesstoken of which to instantiate a new token</param>
        public GenericToken(string accesstoken)
        {
            if (string.IsNullOrWhiteSpace(accesstoken))
            {
                throw new ArgumentNullException(nameof(accesstoken));
            }

            ParsedToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(accesstoken);
            TokenAudience = Enums.TokenAudience.Other;
            AccessToken = accesstoken;

            ExpiresOn = ParsedToken.ValidTo.ToLocalTime();
            Audiences = ParsedToken.Audiences.ToArray();
            TenantId = Guid.TryParse(ParsedToken.Claims.FirstOrDefault(c => c.Type == "tid").Value, out Guid tenandIdGuid) ? (Guid?)tenandIdGuid : null;

            var rolesList = new List<string>();
            rolesList.AddRange(ParsedToken.Claims.Where(c => c.Type.Equals("roles", StringComparison.InvariantCultureIgnoreCase)).Select(c => c.Value));
            foreach (var scope in ParsedToken.Claims.Where(c => c.Type.Equals("scp", StringComparison.InvariantCultureIgnoreCase)))
            {
                rolesList.AddRange(scope.Value.Split(' '));
            }
            Roles = rolesList.ToArray();

            TokenType = ParsedToken.Claims.FirstOrDefault(c => c.Type == "upn") != null ? TokenType.Delegate : TokenType.Application;
        }

        private object List<T>()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tries to acquire a generic Microsoft Identity V1 Token
        /// </summary>
        /// <param name="tenant">Name of the tenant to acquire the token for (i.e. contoso.onmicrosoft.com)</param>
        /// <param name="clientId">ClientId to use to acquire the token</param>
        /// <param name="username">Username to use to acquire the token</param>
        /// <param name="password">Password to use to acquire the token</param>
        /// <param name="resource">Resource to acquire the token for (i.e. AllSites.FullControl)</param>
        /// <returns></returns>
        public static GenericToken AcquireV1Token(string tenant, string clientId, string username, string password, string resource)
        {
            var body = $"grant_type=password&client_id={clientId}&username={HttpUtility.UrlEncode(username)}&password={HttpUtility.UrlEncode(password)}&resource={resource}";
            var response = HttpHelper.MakePostRequestForString($"https://login.microsoftonline.com/{tenant}/oauth2/token", body, "application/x-www-form-urlencoded");
            using (var jdoc = JsonDocument.Parse(response))
            {
                return new GenericToken(jdoc.RootElement.GetProperty("access_token").GetString());
            }
            //var json = JToken.Parse(response);
            //var accessToken = json["access_token"].ToString();

            //return new GenericToken(accessToken);
        }

        /// <summary>
        /// Tries to acquire a generic Microsoft Identity V2 Token
        /// </summary>
        /// <param name="tenant">Name of the tenant to acquire the token for (i.e. contoso.onmicrosoft.com)</param>
        /// <param name="clientId">ClientId to use to acquire the token</param>
        /// <param name="username">Username to use to acquire the token</param>
        /// <param name="password">Password to use to acquire the token</param>
        /// <param name="scopes">One or more scopes to acquire the token for (i.e. AllSites.FullControl)</param>
        /// <returns></returns>
        public static GenericToken AcquireV2Token(string tenant, string clientId, string username, string password, string[] scopes)
        {
            var body = $"grant_type=password&client_id={clientId}&username={HttpUtility.UrlEncode(username)}&password={HttpUtility.UrlEncode(password)}&scope={string.Join(" ", scopes)}";
            var response = HttpHelper.MakePostRequestForString($"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token", body, "application/x-www-form-urlencoded");

            using (var jdoc = JsonDocument.Parse(response))
            {
                return new GenericToken(jdoc.RootElement.GetProperty("access_token").GetString());
            }
        }

        public static GenericToken AcquireApplicationToken(string tenant, string clientId, string authority, string[] scopes, X509Certificate2 certificate)
        {
            if (string.IsNullOrEmpty(tenant))
            {
                throw new ArgumentNullException(nameof(tenant));
            }
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }
            AuthenticationResult tokenResult = null;

            if (confidentialClientApplication == null)
            {
                confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(clientId).WithAuthority(authority).WithCertificate(certificate).Build();
            }

            var account = confidentialClientApplication.GetAccountsAsync().GetAwaiter().GetResult();

            try
            {
                tokenResult = confidentialClientApplication.AcquireTokenSilent(scopes, account.First()).ExecuteAsync().GetAwaiter().GetResult();
            }
            catch
            {
                tokenResult = confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync().GetAwaiter().GetResult();
            }

            return new GenericToken(tokenResult.AccessToken);
        }

        /// <summary>
        /// Tries to acquire an application Microsoft Graph Access Token
        /// </summary>
        /// <param name="tenant">Name of the tenant to acquire the token for (i.e. contoso.onmicrosoft.com). Required.</param>
        /// <param name="clientId">ClientId to use to acquire the token. Required.</param>
        /// <param name="clientSecret">Client Secret to use to acquire the token. Required.</param>
        /// <returns><see cref="GraphToken"/> instance with the token</returns>
        public static GenericToken AcquireApplicationToken(string tenant, string clientId, string authority, string[] scopes, string clientSecret)
        {
            if (string.IsNullOrEmpty(tenant))
            {
                throw new ArgumentNullException(nameof(tenant));
            }
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            AuthenticationResult tokenResult = null;

            if (confidentialClientApplication == null)
            {
                confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(clientId).WithAuthority(authority).WithClientSecret(clientSecret).Build();
            }

            var account = confidentialClientApplication.GetAccountsAsync().GetAwaiter().GetResult();

            try
            {
                tokenResult = confidentialClientApplication.AcquireTokenSilent(scopes, account.First()).ExecuteAsync().GetAwaiter().GetResult();
            }
            catch
            {
                tokenResult = confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync().GetAwaiter().GetResult();
            }
            return new GenericToken(tokenResult.AccessToken);
        }

        /// <summary>
        /// Tries to acquire an application Microsoft Graph Access Token for the provided scopes interactively by allowing the user to log in
        /// </summary>
        /// <param name="clientId">ClientId to use to acquire the token. Required.</param>
        /// <param name="scopes">Array with scopes that should be requested access to. Required.</param>
        /// <returns><see cref="GraphToken"/> instance with the token</returns>
        public static GenericToken AcquireApplicationTokenInteractive(string clientId, string[] scopes)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }
            if (scopes == null || scopes.Length == 0)
            {
                throw new ArgumentNullException(nameof(scopes));
            }


            if (publicClientApplication == null)
            {
                publicClientApplication = PublicClientApplicationBuilder.Create(clientId).WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient").Build();
            }

            AuthenticationResult tokenResult = null;

            var account = publicClientApplication.GetAccountsAsync().GetAwaiter().GetResult();

            try
            {
                tokenResult = publicClientApplication.AcquireTokenSilent(scopes, account.First()).ExecuteAsync().GetAwaiter().GetResult();
            }
            catch
            {
                tokenResult = publicClientApplication.AcquireTokenInteractive(scopes).ExecuteAsync().GetAwaiter().GetResult();
            }
            return new GenericToken(tokenResult.AccessToken);
        }

        public static GenericToken AcquireApplicationTokenDeviceLogin(string clientId, string[] scopes, string authority, Action<DeviceCodeResult> callBackAction)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }
            if (scopes == null || scopes.Length == 0)
            {
                throw new ArgumentNullException(nameof(scopes));
            }

            AuthenticationResult tokenResult = null;

            if (publicClientApplication == null)
            {
                publicClientApplication = PublicClientApplicationBuilder.Create(clientId).WithAuthority(authority).Build();

            }
            var account = publicClientApplication.GetAccountsAsync().GetAwaiter().GetResult();

            try
            {
                tokenResult = publicClientApplication.AcquireTokenSilent(scopes, account.First()).ExecuteAsync().GetAwaiter().GetResult();
            }
            catch
            {
                var builder = publicClientApplication.AcquireTokenWithDeviceCode(scopes, result =>
                {
                    if (callBackAction != null)
                    {
                        callBackAction(result);
                    }
                    return Task.FromResult(0);
                });
                tokenResult = builder.ExecuteAsync().GetAwaiter().GetResult();
            }
            return new GenericToken(tokenResult.AccessToken);
        }
        /// <summary>
        /// Tries to acquire a delegated Microsoft Graph Access Token for the provided scopes using the provided credentials
        /// </summary>
        /// <param name="clientId">ClientId to use to acquire the token. Required.</param>
        /// <param name="scopes">Array with scopes that should be requested access to. Required.</param>
        /// <param name="username">The username to authenticate with. Required.</param>
        /// <param name="securePassword">The password to authenticate with. Required.</param>
        /// <returns><see cref="GraphToken"/> instance with the token</returns>
        public static GenericToken AcquireDelegatedTokenWithCredentials(string clientId, string[] scopes, string authority, string username, SecureString securePassword)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }
            if (scopes == null || scopes.Length == 0)
            {
                throw new ArgumentNullException(nameof(scopes));
            }
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (securePassword == null || securePassword.Length == 0)
            {
                throw new ArgumentNullException(nameof(securePassword));
            }

            AuthenticationResult tokenResult = null;

            if (publicClientApplication == null)
            {
                publicClientApplication = PublicClientApplicationBuilder.Create(clientId).WithAuthority(authority).Build();

            }
            var account = publicClientApplication.GetAccountsAsync().GetAwaiter().GetResult();
            try
            {
                tokenResult = publicClientApplication.AcquireTokenSilent(scopes, account.First()).ExecuteAsync().GetAwaiter().GetResult();
            }
            catch
            {
                tokenResult = publicClientApplication.AcquireTokenByUsernamePassword(scopes, username, securePassword).ExecuteAsync().GetAwaiter().GetResult();
            }

            return new GenericToken(tokenResult.AccessToken);
        }

        public static void ClearCaches()
        {
            if (publicClientApplication != null)
            {
                var accounts = publicClientApplication.GetAccountsAsync().GetAwaiter().GetResult().ToList();

                // clear the cache
                while (accounts.Any())
                {
                    publicClientApplication.RemoveAsync(accounts.First());
                    accounts = publicClientApplication.GetAccountsAsync().GetAwaiter().GetResult().ToList();
                }
                publicClientApplication = null;
            }
            if (confidentialClientApplication != null)
            {
                var accounts = confidentialClientApplication.GetAccountsAsync().GetAwaiter().GetResult().ToList();

                // clear the cache
                while (accounts.Any())
                {
                    confidentialClientApplication.RemoveAsync(accounts.First());
                    accounts = confidentialClientApplication.GetAccountsAsync().GetAwaiter().GetResult().ToList();
                }
                confidentialClientApplication = null;
            }
        }
    }
}
