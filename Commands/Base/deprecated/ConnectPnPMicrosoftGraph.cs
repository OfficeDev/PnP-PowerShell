using Microsoft.Identity.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Management.Automation;
using AuthenticationResult = Microsoft.Identity.Client.AuthenticationResult;
using ClientCredential = Microsoft.Identity.Client.ClientCredential;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Obsolete("This cmdlet has been deprecated and will be removed in the April 2018 release")]
    [Cmdlet("Connect", "PnPMicrosoftGraph", DefaultParameterSetName = ParameterSet_SCOPE)]
    [CmdletHelp("Connect to the Microsoft Graph",
        "Uses the Microsoft Authentication Library (Preview) to connect to Azure AD and to get an OAuth 2.0 Access Token to consume the Microsoft Graph API",
        Category = CmdletHelpCategory.Graph)]
    [CmdletExample(
       Code = "PS:> Connect-PnPMicrosoftGraph -Scopes $arrayOfScopes",
       Remarks = "Connects to Azure AD and gets and OAuth 2.0 Access Token to consume the Microsoft Graph API including the declared permission scopes. The available permission scopes are defined at the following URL: https://graph.microsoft.io/en-us/docs/authorization/permission_scopes",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Connect-PnPMicrosoftGraph -AppId '<id>' -AppSecret '<secrect>' -AADDomain 'contoso.onmicrosoft.com'",
       Remarks = "Connects to the Microsoft Graph API using application permissions via an app's declared permission scopes. See https://github.com/SharePoint/PnP-PowerShell/tree/master/Samples/Graph.ConnectUsingAppPermissions for a sample on how to get started.",
       SortOrder = 2)]
    public class ConnectPnPMicrosoftGraph : PSCmdlet
    {
        private const string ParameterSet_SCOPE = "Scope";
        private const string ParameterSet_AAD = "AAD";
        private const string MSALPnPPowerShellClientId = "bb0c5778-9d5c-41ea-a4a8-8cd417b3ab71";
        private const string RedirectUri = "urn:ietf:wg:oauth:2.0:oob";
        private static readonly Uri AADLogin = new Uri("https://login.microsoftonline.com/");
        private static readonly string[] DefaultScope = { "https://graph.microsoft.com/.default" };

        [Parameter(Mandatory = true, HelpMessage = "The array of permission scopes for the Microsoft Graph API.", ParameterSetName = ParameterSet_SCOPE)]
        public string[] Scopes;

        [Parameter(Mandatory = true, HelpMessage = "The client id of the app which gives you access to the Microsoft Graph API.", ParameterSetName = ParameterSet_AAD)]
        public string AppId;

        [Parameter(Mandatory = true, HelpMessage = "The app key of the app which gives you access to the Microsoft Graph API.", ParameterSetName = ParameterSet_AAD)]
        public string AppSecret;

        [Parameter(Mandatory = true, HelpMessage = "The AAD where the O365 app is registred. Eg.: contoso.com, or contoso.onmicrosoft.com.", ParameterSetName = ParameterSet_AAD)]
        public string AADDomain;

        protected override void ProcessRecord()
        {
            AuthenticationResult authenticationResult;
            if (Scopes != null)
            {
                var clientApplication = new PublicClientApplication(MSALPnPPowerShellClientId);
                // Acquire an access token for the given scope
                authenticationResult = clientApplication.AcquireTokenAsync(Scopes).GetAwaiter().GetResult();
            }
            else
            {
                var appCredentials = new ClientCredential(AppSecret);
                var authority = new Uri(AADLogin, AADDomain).AbsoluteUri;
                var clientApplication = new ConfidentialClientApplication(authority, AppId, RedirectUri, appCredentials, null);
                authenticationResult = clientApplication.AcquireTokenForClient(DefaultScope, null).GetAwaiter().GetResult();
            }

            // Get back the Access Token and the Refresh Token
            SPOnlineConnection.AuthenticationResult = authenticationResult;
        }
    }
}