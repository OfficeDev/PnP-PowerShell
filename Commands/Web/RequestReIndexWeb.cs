using Microsoft.SharePoint.Client;
using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsLifecycle.Request, "PnPReIndexWeb")]
    [CmdletAlias("Request-PnPReIndexWeb")]
    [CmdletHelp("Marks the web for full indexing during the next incremental crawl",
        Category = CmdletHelpCategory.Webs)]
    public class RequestReIndexWeb : SPOWebCmdlet
    {

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.ReIndexWeb();
        }
    }
}
