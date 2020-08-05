#if !ONPREMISES
using PnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Collections.Generic;
#if PNPPSCORE
using System.IdentityModel.Tokens.Jwt;
#else
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
#endif
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPAccessToken")]
    [CmdletHelp("Returns the current OAuth Access token",
        DetailedDescription = "Gets the OAuth 2.0 Access Token to consume the Microsoft Graph API. Doesn't work with all Connect-PnPOnline options.",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPAccessToken",
       Remarks = "Gets the OAuth 2.0 Access Token to consume the Microsoft Graph API",
       SortOrder = 1)]
    [Obsolete("Use Get-PnPGraphAccessToken instead")]
    public class GetPnPAccessToken : PnPGraphCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Returns the details from the access token in a decoded manner")]
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