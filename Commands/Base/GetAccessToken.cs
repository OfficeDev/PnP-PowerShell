using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Collections.Generic;
#if NETSTANDARD2_0
using System.IdentityModel.Tokens.Jwt;
#else
using System.IdentityModel.Tokens;
#endif
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPAccessToken")]
    [CmdletHelp("Returns the current OAuth Access token",
        "Gets the OAuth 2.0 Access Token to consume the Microsoft Graph API",
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
       Code = "PS:> Get-PnPAccessToken",
       Remarks = "Gets the OAuth 2.0 Access Token to consume the Microsoft Graph API",
       SortOrder = 1)]
    public class GetPnPAccessToken : PnPGraphCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Returns the access token in a decoded manner")]
        public SwitchParameter Decoded;
        protected override void ExecuteCmdlet()
        {
            if (Decoded.IsPresent)
            {
                var decodedToken = new JwtSecurityToken(AccessToken);
                WriteObject(decodedToken);
            }
            else
            {
                WriteObject(AccessToken);
            }
        }
    }
}
