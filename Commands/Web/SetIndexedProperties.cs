using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "PnPIndexedProperties")]
    [CmdletAlias("Set-SPOIndexedProperties")]
    [CmdletHelp("Marks values of the propertybag to be indexed by search. Notice that this will overwrite the existing flags, i.e. only the properties you define with the cmdlet will be indexed.",
        Category = CmdletHelpCategory.Webs)]
    public class SetIndexedProperties : PnPWebCmdlet
    {
        [Parameter(Mandatory = true)]
        public List<string> Keys;

        protected override void ExecuteCmdlet()
        {
            if (Keys != null && Keys.Count > 0)
            {
                SelectedWeb.RemovePropertyBagValue("vti_indexedpropertykeys");

                foreach (var key in Keys)
                {
                    SelectedWeb.AddIndexedPropertyBagKey(key);
                }
            }
        }
    }
}
