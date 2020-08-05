using Microsoft.Identity.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Model
{
    /// <summary>
    /// Contains a Graph JWT oAuth token
    /// </summary>
    public class GraphToken : GenericToken
    {
        /// <summary>
        /// The resource identifier for Microsoft Graph API tokens
        /// </summary>
        public const string ResourceIdentifier = "https://graph.microsoft.com";

        /// <summary>
        /// Instantiates a new Graph token
        /// </summary>
        /// <param name="accesstoken">Accesstoken of which to instantiate a new token</param>
        public GraphToken(string accesstoken) : base(accesstoken)
        {
            TokenAudience = Enums.TokenAudience.MicrosoftGraph;
        }

        /// <summary>
        /// Tries to acquire an application Microsoft Graph Access Token
        /// </summary>
        /// <param name="tenant">Name of the tenant to acquire the token for (i.e. contoso.onmicrosoft.com). Required.</param>
        /// <param name="clientId">ClientId to use to acquire the token. Required.</param>
        /// <param name="certificate">Certificate to use to acquire the token. Required.</param>
        /// <returns><see cref="GraphToken"/> instance with the token</returns>
        public static GraphToken AcquireApplicationToken(string tenant, string clientId, X509Certificate2 certificate)
        {
            return new GraphToken(GenericToken.AcquireApplicationToken(tenant, clientId, $"{BaseAuthority}{tenant}", new[] { $"{ResourceIdentifier}/{DefaultScope}" }, certificate).AccessToken);
        }

        /// <summary>
        /// Tries to acquire an application Microsoft Graph Access Token
        /// </summary>
        /// <param name="tenant">Name of the tenant to acquire the token for (i.e. contoso.onmicrosoft.com). Required.</param>
        /// <param name="clientId">ClientId to use to acquire the token. Required.</param>
        /// <param name="clientSecret">Client Secret to use to acquire the token. Required.</param>
        /// <returns><see cref="GraphToken"/> instance with the token</returns>
        public static GraphToken AcquireApplicationToken(string tenant, string clientId, string clientSecret)
        {
            return new GraphToken(GenericToken.AcquireApplicationToken(tenant, clientId, $"{BaseAuthority}{tenant}", new[] { $"{ResourceIdentifier}/{DefaultScope}" }, clientSecret).AccessToken);
        }

        /// <summary>
        /// Tries to acquire an application Microsoft Graph Access Token for the provided scopes interactively by allowing the user to log in
        /// </summary>
        /// <param name="clientId">ClientId to use to acquire the token. Required.</param>
        /// <param name="scopes">Array with scopes that should be requested access to. Required.</param>
        /// <returns><see cref="GraphToken"/> instance with the token</returns>
        public static new GraphToken AcquireApplicationTokenInteractive(string clientId, string[] scopes)
        {
            return new GraphToken(GenericToken.AcquireApplicationTokenInteractive(clientId, scopes.Select(s => $"{ResourceIdentifier}/{s}").ToArray()).AccessToken);
        }

        public static GraphToken AcquireApplicationTokenDeviceLogin(string clientId, string[] scopes, Action<DeviceCodeResult> callBackAction)
        {
            var officeManagementApiScopes = Enum.GetNames(typeof(OfficeManagementApiPermission)).Select(s => s.Replace("_", ".")).Intersect(scopes).ToArray();
            // Take the remaining scopes and try requesting them from the Microsoft Graph API
            scopes = scopes.Except(officeManagementApiScopes).ToArray();

            return new GraphToken(AcquireApplicationTokenDeviceLogin(clientId, scopes, $"{BaseAuthority}organizations", callBackAction).AccessToken);
        }
        /// <summary>
        /// Tries to acquire a delegated Microsoft Graph Access Token for the provided scopes using the provided credentials
        /// </summary>
        /// <param name="clientId">ClientId to use to acquire the token. Required.</param>
        /// <param name="scopes">Array with scopes that should be requested access to. Required.</param>
        /// <param name="username">The username to authenticate with. Required.</param>
        /// <param name="securePassword">The password to authenticate with. Required.</param>
        /// <returns><see cref="GraphToken"/> instance with the token</returns>
        public static GraphToken AcquireDelegatedTokenWithCredentials(string clientId, string[] scopes, string username, SecureString securePassword)
        {
            var officeManagementApiScopes = Enum.GetNames(typeof(OfficeManagementApiPermission)).Select(s => s.Replace("_", ".")).Intersect(scopes).ToArray();
            // Take the remaining scopes and try requesting them from the Microsoft Graph API
            scopes = scopes.Except(officeManagementApiScopes).ToArray();
            
            return new GraphToken(AcquireDelegatedTokenWithCredentials(clientId, scopes.Select(s => $"{ResourceIdentifier}/{s}").ToArray(), $"{BaseAuthority}organizations/", username, securePassword).AccessToken);
        }
    }
}
