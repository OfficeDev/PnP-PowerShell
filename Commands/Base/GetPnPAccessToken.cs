using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet("Get", "PnPAccessToken")]
    [CmdletHelp("Gets the OAuth 2.0 Access Token to consume the Microsoft Graph API",
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
       Code = "PS:> Get-PnPAccessToken",
       Remarks = "Gets the OAuth 2.0 Access Token to consume the Microsoft Graph API",
       SortOrder = 1)]
    public class GetPnPAccessToken : PnPGraphCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            WriteObject(AccessToken);
        }
    }
}
