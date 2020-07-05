using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace SharePointPnP.PowerShell.Commands.Model
{
    /// <summary>
    /// Contains an Office 365 Management API JWT oAuth token
    /// </summary>
    public class OfficeManagementApiToken : GenericToken
    {
        private static IPublicClientApplication publicClientApplication;
        private static IConfidentialClientApplication confidentialClientApplication;

        /// <summary>
        /// The resource identifier for Microsoft Office 365 Management API tokens
        /// </summary>
        public const string ResourceIdentifier = "https://manage.office.com";

        /// <summary>
        /// The name of the default scope
        /// </summary>
        private const string DefaultScope = ".default";

        /// <summary>
        /// The base URL to request a token from
        /// </summary>
        private const string OAuthBaseUrl = "https://login.microsoftonline.com/";

        /// <summary>
        /// Instantiates a new Office 365 Management API token
        /// </summary>
        /// <param name="accesstoken">Accesstoken of which to instantiate a new token</param>
        public OfficeManagementApiToken(string accesstoken) : base(accesstoken)
        {
            TokenAudience = Enums.TokenAudience.MicrosoftGraph;
        }

        /// <summary>
        /// Tries to acquire an application Office 365 Management API Access Token
        /// </summary>
        /// <param name="tenant">Name or id of the tenant to acquire the token for (i.e. contoso.onmicrosoft.com). Required.</param>
        /// <param name="clientId">ClientId to use to acquire the token. Required.</param>
        /// <param name="certificate">Certificate to use to acquire the token. Required.</param>
        /// <returns><see cref="OfficeManagementApiToken"/> instance with the token</returns>
        public static GenericToken AcquireApplicationToken(string tenant, string clientId, X509Certificate2 certificate)
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

            if (confidentialClientApplication == null)
            {
                confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(clientId).WithAuthority($"{OAuthBaseUrl}{tenant}").WithCertificate(certificate).Build();
            }
            var accounts = confidentialClientApplication.GetAccountsAsync().GetAwaiter().GetResult();

            AuthenticationResult tokenResult = null;

            try
            {
                tokenResult = confidentialClientApplication.AcquireTokenSilent(new[] { $"{ResourceIdentifier}/{DefaultScope}" }, accounts.First()).ExecuteAsync().GetAwaiter().GetResult();
            }
            catch
            {
                tokenResult = confidentialClientApplication.AcquireTokenForClient(new[] { $"{ResourceIdentifier}/{DefaultScope}" }).ExecuteAsync().GetAwaiter().GetResult();
            }

            return new OfficeManagementApiToken(tokenResult.AccessToken);
        }

        /// <summary>
        /// Tries to acquire an application Office 365 Management API Access Token
        /// </summary>
        /// <param name="tenant">Name or id of the tenant to acquire the token for (i.e. contoso.onmicrosoft.com). Required.</param>
        /// <param name="clientId">ClientId to use to acquire the token. Required.</param>
        /// <param name="clientSecret">Client Secret to use to acquire the token. Required.</param>
        /// <returns><see cref="OfficeManagementApiToken"/> instance with the token</returns>
        public static GenericToken AcquireApplicationToken(string tenant, string clientId, string clientSecret)
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

            if (confidentialClientApplication == null)
            {
                confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(clientId).WithAuthority($"{OAuthBaseUrl}{tenant}").WithClientSecret(clientSecret).Build();
            }

            var accounts = confidentialClientApplication.GetAccountsAsync().GetAwaiter().GetResult();

            AuthenticationResult tokenResult = null;

            try
            {
                tokenResult = confidentialClientApplication.AcquireTokenSilent(new[] { $"{ResourceIdentifier}/{DefaultScope}" }, accounts.First()).ExecuteAsync().GetAwaiter().GetResult();
            }
            catch
            {
                tokenResult = confidentialClientApplication.AcquireTokenForClient(new[] { $"{ResourceIdentifier}/{DefaultScope}" }).ExecuteAsync().GetAwaiter().GetResult();
            }

            return new OfficeManagementApiToken(tokenResult.AccessToken);
        }

        /// <summary>
        /// Tries to acquire an application Office 365 Management API Access Token for the provided scopes interactively by allowing the user to log in
        /// </summary>
        /// <param name="clientId">ClientId to use to acquire the token. Required.</param>
        /// <param name="scopes">Array with scopes that should be requested access to. Required.</param>
        /// <returns><see cref="OfficeManagementApiToken"/> instance with the token</returns>
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
                publicClientApplication = PublicClientApplicationBuilder.Create(clientId).Build();
            }

            var accounts = confidentialClientApplication.GetAccountsAsync().GetAwaiter().GetResult();

            AuthenticationResult tokenResult = null;

            try
            {
                tokenResult = publicClientApplication.AcquireTokenSilent(new[] { $"{ResourceIdentifier}/{DefaultScope}" }, accounts.First()).ExecuteAsync().GetAwaiter().GetResult();
            }
            catch
            {
                tokenResult = publicClientApplication.AcquireTokenInteractive(scopes.Select(s => $"{ResourceIdentifier}/{s}").ToArray()).ExecuteAsync().GetAwaiter().GetResult();
            }

            return new OfficeManagementApiToken(tokenResult.AccessToken);
        }

        /// <summary>
        /// Tries to acquire a delegated Office 365 Management API Access Token for the provided scopes using the provided credentials
        /// </summary>
        /// <param name="clientId">ClientId to use to acquire the token. Required.</param>
        /// <param name="scopes">Array with scopes that should be requested access to. Required.</param>
        /// <param name="username">The username to authenticate with. Required.</param>
        /// <param name="securePassword">The password to authenticate with. Required.</param>
        /// <returns><see cref="OfficeManagementApiToken"/> instance with the token</returns>
        public static GenericToken AcquireDelegatedTokenWithCredentials(string clientId, string[] scopes, string username, SecureString securePassword)
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


            if (publicClientApplication == null)
            {
                publicClientApplication = PublicClientApplicationBuilder.Create(clientId)
                // Delegated Graph token using credentials is only possible against organizational tenants
                .WithAuthority($"{OAuthBaseUrl}organizations/")
                .Build();
            }

            var accounts = confidentialClientApplication.GetAccountsAsync().GetAwaiter().GetResult();

            AuthenticationResult tokenResult = null;

            try
            {
                tokenResult = publicClientApplication.AcquireTokenSilent(new[] { $"{ResourceIdentifier}/{DefaultScope}" }, accounts.First()).ExecuteAsync().GetAwaiter().GetResult();
            }
            catch
            {
                tokenResult = publicClientApplication.AcquireTokenByUsernamePassword(scopes.Select(s => $"{ResourceIdentifier}/{s}").ToArray(), username, securePassword).ExecuteAsync().GetAwaiter().GetResult();
            }
            return new GraphToken(tokenResult.AccessToken);
        }
    }
}
