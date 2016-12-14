using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPMasterPage")]
    [CmdletAlias("Get-SPOMasterPage")]
    [CmdletHelp("Returns the URLs of the default Master Page and the custom Master Page.", 
        Category = CmdletHelpCategory.Branding)]
    public class GetMasterPage : SPOWebCmdlet
    {

        protected override void ExecuteCmdlet()
        {
            ClientContext.Load(SelectedWeb, w => w.MasterUrl, w => w.CustomMasterUrl);
            ClientContext.ExecuteQueryRetry();

            WriteObject(new {SelectedWeb.MasterUrl, SelectedWeb.CustomMasterUrl });
        }
    }
}
