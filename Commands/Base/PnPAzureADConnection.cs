using Microsoft.Identity.Client;
using ADAL = Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace SharePointPnP.PowerShell.Commands.Base
{
    /// <summary>
    /// Holds all of the information about the current Azure AD Connection and OAuth 2.0 Access Token
    /// </summary>
    public class PnPAzureADConnection
    {
        /// <summary>
        /// Holds the OAuth 2.0 Authentication Result
        /// </summary>
        public static AuthenticationResult AuthenticationResult;

        /// <summary>
        /// Holds the OAuth 1.0 Authentication Result from ADAL
        /// </summary>
        public static ADAL.AuthenticationResult ADALAuthenticationResult;
    }
}
