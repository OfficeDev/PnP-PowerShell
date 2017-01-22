using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace SharePointPnP.PowerShell.Commands.Publishing
{
    [Cmdlet(VerbsCommon.Set, "PnPAvailablePageLayouts")]
    [CmdletHelp("Sets the available page layouts for the current site",
        Category = CmdletHelpCategory.Publishing)]
    public class SetAvailablePageLayouts : PnPWebCmdlet
    {
        [Parameter(
            Mandatory = true,
            ParameterSetName = "SPECIFIC",
            HelpMessage = "An array of page layout files to set as available page layouts for the site.")]
        public string[] PageLayouts;

        [Parameter(
            Mandatory = true, 
            ParameterSetName = "ALL",
            HelpMessage = "An array of page layout files to set as available page layouts for the site.")]
        public SwitchParameter AllowAllPageLayouts;

        [Parameter(
            Mandatory = true,
            ParameterSetName = "INHERIT",
            HelpMessage = "Set the available page layouts to inherit from the parent site.")]
        public SwitchParameter InheritPageLayouts;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == "SPECIFIC")
            {
                if (PageLayouts.Length < 1) throw new ArgumentException("You must provide at least 1 page layout.");

                var rootWeb = ClientContext.Site.RootWeb;
                SelectedWeb.SetAvailablePageLayouts(rootWeb, PageLayouts);
            }
            else if (ParameterSetName == "INHERIT")
            {
                SelectedWeb.SetSiteToInheritPageLayouts();
            }
            else
            {
                SelectedWeb.AllowAllPageLayouts();
            }
        }
    }
}
