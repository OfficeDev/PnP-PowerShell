using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace PnP.PowerShell.Commands.Model
{
    /// <summary>
    /// Contains an Office 365 Management API JWT oAuth token
    /// </summary>
    public class OfficeManagementApiToken : GenericToken
    {
        /// <summary>
        /// The resource identifier for Microsoft Office 365 Management API tokens
        /// </summary>
        public const string ResourceIdentifier = "https://manage.office.com";

        /// <summary>
        /// Instantiates a new Office 365 Management API token
        /// </summary>
        /// <param name="accesstoken">Accesstoken of which to instantiate a new token</param>
        public OfficeManagementApiToken(string accesstoken) : base(accesstoken)
        {
            TokenAudience = Enums.TokenAudience.OfficeManagementApi;
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
            return new OfficeManagementApiToken(GenericToken.AcquireApplicationToken(tenant, clientId, $"{BaseAuthority}{tenant}", new[] { $"{ResourceIdentifier}/{DefaultScope}" }, certificate).AccessToken);
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
            return new OfficeManagementApiToken(GenericToken.AcquireApplicationToken(tenant, clientId, $"{BaseAuthority}{tenant}", new[] { $"{ResourceIdentifier}/{DefaultScope}" }, clientSecret).AccessToken);
        }

        /// <summary>
        /// Tries to acquire an application Office 365 Management API Access Token for the provided scopes interactively by allowing the user to log in
        /// </summary>
        /// <param name="clientId">ClientId to use to acquire the token. Required.</param>
        /// <param name="scopes">Array with scopes that should be requested access to. Required.</param>
        /// <returns><see cref="OfficeManagementApiToken"/> instance with the token</returns>
        public static new GenericToken AcquireApplicationTokenInteractive(string clientId, string[] scopes)
        {
            return new OfficeManagementApiToken(GenericToken.AcquireApplicationTokenInteractive(clientId, scopes.Select(s => $"{ResourceIdentifier}/{s}").ToArray()).AccessToken);
        }

        public static GraphToken AcquireApplicationTokenDeviceLogin(string clientId, string[] scopes, Action<DeviceCodeResult> callBackAction)
        {
            return new GraphToken(AcquireApplicationTokenDeviceLogin(clientId, scopes.Select(s => $"{ResourceIdentifier}/{s}").ToArray(), $"{BaseAuthority}organizations", callBackAction).AccessToken);
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
            return new OfficeManagementApiToken(GenericToken.AcquireDelegatedTokenWithCredentials(clientId, scopes.Select(s => $"{ResourceIdentifier}/{s}").ToArray(), $"{BaseAuthority}organizations/", username, securePassword).AccessToken);
        }
    }
}

