#if !ONPREMISES
using System.Management.Automation;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;

namespace PnP.PowerShell.Commands.ManagementApi
{
    [Cmdlet(VerbsCommon.Get, "PnPOfficeManagementApiAccessToken")]
    [CmdletHelp("Gets an access token for the Microsoft Office 365 Management API from the current connection",
        Category = CmdletHelpCategory.ManagementApi,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPManagementApiAccessToken",
       Remarks = "Gets the OAuth 2.0 Access Token to consume the Microsoft Office 365 Management API",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPManagementApiAccessToken -Decoded",
       Remarks = "Gets the full OAuth 2.0 Token to consume the Microsoft Office 365 Management API",
       SortOrder = 2)]
    public class GetOfficeManagementApiAccessToken : PnPOfficeManagementApiCmdlet
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