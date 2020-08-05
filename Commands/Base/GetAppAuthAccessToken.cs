using System.Management.Automation;
using PnP.PowerShell.CmdletHelpAttributes;
using System;
using PnP.PowerShell.Commands.Properties;
using Microsoft.SharePoint.Client;

namespace PnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPAppAuthAccessToken")]
    [CmdletHelp("Returns the access token", 
        "Returns the access token from the current client context (only works with App-Only authentication)",
        Category = CmdletHelpCategory.Base,
        OutputType = typeof(string),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/system.string.aspx")]
    [CmdletExample(
        Code = @"PS:> $accessToken = Get-PnPAppAuthAccessToken",
        Remarks = @"This will put the access token from current context in the $accessToken variable. Will only work in App authentication flow (App+user or App-Only)",
        SortOrder = 1)]
    public class GetPnPAppAuthAccessToken : PnPSharePointCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            WriteObject(ClientContext.GetAccessToken());
        }
    }
}
