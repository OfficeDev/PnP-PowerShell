using Microsoft.SharePoint.Client;
using System.Management.Automation;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsLifecycle.Request, "PnPReIndexWeb")]
    [CmdletHelp("Marks the web for full indexing during the next incremental crawl",
        Category = CmdletHelpCategory.Webs)]
    public class RequestReIndexWeb : PnPWebCmdlet
    {

        protected override void ExecuteCmdlet()
        {
            SelectedWeb.ReIndexWeb();
        }
    }
}
