using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Remove, "PnPIndexedProperty")]
    [CmdletAlias("Remove-SPOIndexedProperty")]
    [CmdletHelp("Removes a key from propertybag to be indexed by search. The key and it's value remain in the propertybag, however it will not be indexed anymore.",
        Category = CmdletHelpCategory.Webs)]
    public class RemovedIndexedProperty : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = @"Key of the property bag value to be removed from indexing")]
        public string Key;

        protected override void ExecuteCmdlet()
        {
            if (!string.IsNullOrEmpty(Key))
            {
                SelectedWeb.RemoveIndexedPropertyBagKey(Key);
            }
        }
    }
}
