#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPGraphAccessToken")]
    [CmdletHelp("Returns the current OAuth Access token for the Microsoft Graph API",
        DetailedDescription = "Gets the OAuth 2.0 Access Token to consume the Microsoft Graph API",
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
       Code = "PS:> Get-PnPAccessToken",
       Remarks = "Gets the OAuth 2.0 Access Token to consume the Microsoft Graph API",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPAccessToken -Decoded",
       Remarks = "Gets the full OAuth 2.0 Token to consume the Microsoft Graph API",
       SortOrder = 2)]
    public class GetGraphAccessToken : PnPGraphCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Returns the access token in a decoded manner")]
        public SwitchParameter Decoded;

        protected override void ExecuteCmdlet()
        {
            if (Decoded.IsPresent)
            {
                WriteObject(Token.ParsedToken);
            }
            else
            {
                WriteObject(AccessToken);
            }
        }
    }
}
#endif