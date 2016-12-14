using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
