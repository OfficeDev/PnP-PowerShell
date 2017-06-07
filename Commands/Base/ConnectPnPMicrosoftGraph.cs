using Microsoft.Identity.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Management.Automation;
using AuthenticationResult = Microsoft.Identity.Client.AuthenticationResult;
using ClientCredential = Microsoft.Identity.Client.ClientCredential;
using ADAL = Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet("Connect", "PnPMicrosoftGraph", DefaultParameterSetName = "Scope")]
    [CmdletHelp("Uses the Microsoft Authentication Library (Preview) to connect to Azure AD and to get an OAuth 2.0 Access Token to consume the Microsoft Graph API",
        Category = CmdletHelpCategory.Graph)]
    [CmdletExample(
       Code = "PS:> Connect-PnPMicrosoftGraph -Scopes $arrayOfScopes",
       Remarks = "Connects to Azure AD and gets and OAuth 2.0 Access Token to consume the Microsoft Graph API including the declared permission scopes. The available permission scopes are defined at the following URL: https://graph.microsoft.io/en-us/docs/authorization/permission_scopes",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Connect-PnPMicrosoftGraph -AppId '<id>' -AppSecret '<secrect>' -AADDomain 'contoso.onmicrosoft.com'",
       Remarks = "Connects to the Microsoft Graph API using application permissions via an app's declared permission scopes. See https://github.com/SharePoint/PnP-PowerShell/tree/master/Samples/Graph.ConnectUsingAppPermissions for a sample on how to get started.",
       SortOrder = 2)]
    [CmdletExample(
       Code = "PS:> Connect-PnPMicrosoftGraph -AppId '<id>' -AppSecret '<secrect>' -UseADAL -AADDomain 'contoso.onmicrosoft.com'",
       Remarks = "Connects to the Microsoft Graph API using application permissions via an app's declared permission in an ADAL application.",
       SortOrder = 3)]
    public class ConnectPnPMicrosoftGraph : PSCmdlet
    {
        private const string MSALPnPPowerShellClientId = "bb0c5778-9d5c-41ea-a4a8-8cd417b3ab71";
        private const string RedirectUri = "urn:ietf:wg:oauth:2.0:oob";
        const string GraphResourceId = "https://graph.microsoft.com"; // Microsoft Graph End-point
        private static readonly Uri AADLogin = new Uri("https://login.microsoftonline.com/");
        private static readonly Uri ADALLogin = new Uri("https://login.windows.net/");
        private static readonly string[] DefaultScope = { "https://graph.microsoft.com/.default" };

        [Parameter(Mandatory = true, HelpMessage = "The array of permission scopes for the Microsoft Graph API.", ParameterSetName = "Scope")]
        public string[] Scopes;

        [Parameter(Mandatory = true, HelpMessage = "The client id of the app which gives you access to the Microsoft Graph API.", ParameterSetName = "AAD")]
        public string AppId;

        [Parameter(Mandatory = true, HelpMessage = "The app key of the app which gives you access to the Microsoft Graph API.", ParameterSetName = "AAD")]
        public string AppSecret;

        [Parameter(Mandatory = true, HelpMessage = "The AAD where the O365 app is registred. Eg.: contoso.com, or contoso.onmicrosoft.com.", ParameterSetName = "AAD")]
        public string AADDomain;

        [Parameter(Mandatory = false, HelpMessage = "Use an ADAL instead of MSAL.", ParameterSetName = "AAD")]
        public SwitchParameter UseADAL;

        protected override void ProcessRecord()
        {

            if (Scopes != null)
            {
                var clientApplication = new PublicClientApplication(MSALPnPPowerShellClientId);
                // Acquire an access token for the given scope
                AuthenticationResult authenticationResult = clientApplication.AcquireTokenAsync(Scopes).GetAwaiter().GetResult();
                PnPAzureADConnection.AuthenticationResult = authenticationResult;
            }
            else if (UseADAL == false)
            {
                var appCredentials = new ClientCredential(AppSecret);
                var authority = new Uri(AADLogin, AADDomain).AbsoluteUri;

                var clientApplication = new ConfidentialClientApplication(authority, AppId, RedirectUri, appCredentials, null);
                AuthenticationResult authenticationResult = clientApplication.AcquireTokenForClient(DefaultScope, null).GetAwaiter().GetResult();
                // Get back the Access Token and the Refresh Token
                PnPAzureADConnection.AuthenticationResult = authenticationResult;
            }
            else
            {
                var authenticationContext = new ADAL.AuthenticationContext(ADALLogin + AADDomain);
                var clientCredential = new ADAL.ClientCredential(AppId, AppSecret);
                ADAL.AuthenticationResult authenticationResult = authenticationContext.AcquireTokenAsync(GraphResourceId, clientCredential).GetAwaiter().GetResult();
                PnPAzureADConnection.ADALAuthenticationResult = authenticationResult;
            }
        }
    }
}