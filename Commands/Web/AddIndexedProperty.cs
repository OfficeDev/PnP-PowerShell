using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Add, "PnPIndexedProperty")]
    [CmdletAlias("Add-SPOIndexedProperty")]
    [CmdletHelp("Marks the value of the propertybag key specified to be indexed by search.",
        Category = CmdletHelpCategory.Webs)]
    public class AddIndexedProperty : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = @"Key of the property bag value to be indexed")]
        public string Key;

        protected override void ExecuteCmdlet()
        {
            if (!string.IsNullOrEmpty(Key))
            {
                SelectedWeb.AddIndexedPropertyBagKey(Key);
            }
        }
    }
}
