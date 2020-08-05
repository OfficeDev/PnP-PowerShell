using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsLifecycle.Request, "PnPReIndexList")]
    [CmdletHelp("Marks the list for full indexing during the next incremental crawl",
        Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
        Code = @"PS:> Request-PnPReIndexList -Identity ""Demo List""",
        SortOrder = 1)]
    public class RequestReIndexList : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Title or Url of the list.")]
        public ListPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var list = Identity.GetList(SelectedWeb);

            if (list != null)
            {
                list.ReIndexList();
            }
         
        }
    }
}
