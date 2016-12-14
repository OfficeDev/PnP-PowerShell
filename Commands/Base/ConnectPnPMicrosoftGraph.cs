using Microsoft.Identity.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet("Connect", "PnPMicrosoftGraph")]
    [CmdletHelp("Uses the Microsoft Authentication Library (Preview) to connect to Azure AD and to get an OAuth 2.0 Access Token to consume the Microsoft Graph API",
        Category = CmdletHelpCategory.Graph)]
    [CmdletExample(
       Code = "PS:> Connect-PnPMicrosoftGraph -Scopes $arrayOfScopes",
       Remarks = "Connects to Azure AD and gets and OAuth 2.0 Access Token to consume the Microsoft Graph API including the declared permission scopes. The available permission scopes are defined at the following URL: https://graph.microsoft.io/en-us/docs/authorization/permission_scopes",
       SortOrder = 1)]
    public class ConnectPnPMicrosoftGraph : PSCmdlet
    {
        private const string MSALPnPPowerShellClientId = "bb0c5778-9d5c-41ea-a4a8-8cd417b3ab71";

        [Parameter(Mandatory = true, HelpMessage = "The array of permission scopes for the Microsoft Graph API.")]
        public String[] Scopes;

        protected override void ProcessRecord()
        {
            PublicClientApplication clientApplication =
                new PublicClientApplication(MSALPnPPowerShellClientId);

            // Acquire an access token for the given scope
            var authenticationResult = clientApplication.AcquireTokenAsync(Scopes).GetAwaiter().GetResult();

            // Get back the Access Token and the Refresh Token
            PnPAzureADConnection.AuthenticationResult = authenticationResult;
        }
    }
}
