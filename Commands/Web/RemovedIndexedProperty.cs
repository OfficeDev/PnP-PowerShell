using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Remove, "PnPIndexedProperty")]
    [CmdletHelp("Removes a key from propertybag to be indexed by search. The key and it's value remain in the propertybag, however it will not be indexed anymore.",
        Category = CmdletHelpCategory.Webs)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPIndexedProperty -key ""MyIndexProperty""",
        Remarks = @"Removes the Indexed property ""MyIndexProperty"" from the current web",
        SortOrder = 1)]
    public class RemovedIndexedProperty : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = @"Key of the property bag value to be removed from indexing")]
        public string Key;

        [Parameter(Mandatory = false, ValueFromPipeline = true, HelpMessage = "The list object or name from where to remove the indexed properties")]
        public ListPipeBind List;
        protected override void ExecuteCmdlet()
        {
            if (!string.IsNullOrEmpty(Key))
            {
                if (List != null)
                {
                    var list = List.GetList(SelectedWeb);
                    if (list != null)
                    {
                        list.RemoveIndexedPropertyBagKey(Key);
                    }
                }
                else
                {
                    SelectedWeb.RemoveIndexedPropertyBagKey(Key);
                }
            }
        }
    }
}
