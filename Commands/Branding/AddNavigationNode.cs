using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.Commands.Enums;
using System;
using System.Management.Automation;
using OfficeDevPnP.Core.Enums;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Add, "SPONavigationNode")]
    [CmdletHelp("Adds a menu item to either the quicklaunch or top navigation", 
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(
        Code = @"PS:> Add-SPONavigationNode -Title ""Contoso"" -Url ""http://contoso.sharepoint.com/sites/contoso/"" -Location ""QuickLaunch""",
        Remarks = @"Adds a navigation node to the quicklaunch. The navigation node will have the title ""Contoso"" and will link to the url ""http://contoso.sharepoint.com/sites/contoso/""",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Add-SPONavigationNode -Title ""Contoso USA"" -Url ""http://contoso.sharepoint.com/sites/contoso/usa/"" -Location ""QuickLaunch"" -Header ""Contoso""",
        Remarks = @"Adds a navigation node to the quicklaunch. The navigation node will have the title ""Contoso USA"", will link to the url ""http://contoso.sharepoint.com/sites/contoso/usa/"" and will have ""Contoso"" as a parent navigation node.",
        SortOrder = 2)]
    public class AddNavigationNode : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The location of the node to add. Either TopNavigationBar, QuickLaunch or SearchNav")]
        public NavigationType Location;

        [Parameter(Mandatory = true, HelpMessage = "The title of the node to add")]
        public string Title;

        [Parameter(Mandatory = false, HelpMessage = "The url to navigate to when clicking the new menu item.")]
        public string Url;

        [Parameter(Mandatory = false, HelpMessage = "Optionally value of a header entry to add the menu item to.")]
        public string Header;

        protected override void ExecuteCmdlet()
        {
            if (Url == null)
            {
                ClientContext.Load(SelectedWeb, w => w.Url);
                ClientContext.ExecuteQueryRetry();
                Url = SelectedWeb.Url;
            }
            SelectedWeb.AddNavigationNode(Title, new Uri(Url), Header, Location);
        }

    }


}
