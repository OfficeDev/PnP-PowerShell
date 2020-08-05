using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPIndexedPropertyKeys")]
    [CmdletHelp("Returns the keys of the property bag values that have been marked for indexing by search",
        Category = CmdletHelpCategory.Webs)]
    public class GetIndexedProperties : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, HelpMessage = "The list object or name from where to get the indexed properties")]
        public ListPipeBind List;

        protected override void ExecuteCmdlet()
        {
            if (List != null)
            {
                var list = List.GetList(SelectedWeb);
                if (list != null)
                {
                    var keys = list.GetIndexedPropertyBagKeys();
                    WriteObject(keys);
                }
            }
            else
            {
                var keys = SelectedWeb.GetIndexedPropertyBagKeys();
                WriteObject(keys);
            }
        }
    }
}
