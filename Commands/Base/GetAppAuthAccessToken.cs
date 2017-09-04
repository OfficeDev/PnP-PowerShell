using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using SharePointPnP.PowerShell.Commands.Properties;
using Microsoft.SharePoint.Client;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPAppAuthAccessToken")]
    [CmdletHelp("Returns the access token from the current client context (In App authentication mode only)",
        Category = CmdletHelpCategory.Base,
        OutputType = typeof(string),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/system.string.aspx")]
    [CmdletExample(
        Code = @"PS:> $accessToken = Get-PnPAppAuthAccessToken",
        Remarks = @"This will put the access token from current context in the $accessToken variable. Will only work in App authentication flow (App+user or App-Only)",
        SortOrder = 1)]        
    public class GetPnPAppAuthAccessToken : PnPCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            WriteObject(ClientContext.GetAccessToken());
        }
    }
}
