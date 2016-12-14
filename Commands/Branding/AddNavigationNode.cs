using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Enums;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Add, "PnPNavigationNode")]
    [CmdletAlias("Add-SPONavigationNode")]
    [CmdletHelp("Adds a menu item to either the quicklaunch or top navigation", 
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(
        Code = @"PS:> Add-PnPNavigationNode -Title ""Contoso"" -Url ""http://contoso.sharepoint.com/sites/contoso/"" -Location ""QuickLaunch""",
        Remarks = @"Adds a navigation node to the quicklaunch. The navigation node will have the title ""Contoso"" and will link to the url ""http://contoso.sharepoint.com/sites/contoso/""",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Add-PnPNavigationNode -Title ""Contoso USA"" -Url ""http://contoso.sharepoint.com/sites/contoso/usa/"" -Location ""QuickLaunch"" -Header ""Contoso""",
        Remarks = @"Adds a navigation node to the quicklaunch. The navigation node will have the title ""Contoso USA"", will link to the url ""http://contoso.sharepoint.com/sites/contoso/usa/"" and will have ""Contoso"" as a parent navigation node.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Add-PnPNavigationNode -Title ""Contoso"" -Url ""http://contoso.sharepoint.com/sites/contoso/"" -Location ""QuickLaunch"" -First",
        Remarks = @"Adds a navigation node to the quicklaunch, as the first item. The navigation node will have the title ""Contoso"" and will link to the url ""http://contoso.sharepoint.com/sites/contoso/""",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Add-PnPNavigationNode -Title ""Contoso Pharmaceuticals"" -Url ""http://contoso.sharepoint.com/sites/contosopharma/"" -Location ""QuickLaunch"" -External",
        Remarks = @"Adds a navigation node to the quicklaunch. The navigation node will have the title ""Contoso Pharmaceuticals"" and will link to the external url ""http://contoso.sharepoint.com/sites/contosopharma/""",
        SortOrder = 4)]
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
        
        [Parameter(Mandatory = false, HelpMessage = "Add the new menu item to beginning of the collection.")]
        public SwitchParameter First;
        
        [Parameter(Mandatory = false, HelpMessage = "Indicates the destination URL is outside of the site collection.")]
        public SwitchParameter External;

        protected override void ExecuteCmdlet()
        {
            if (Url == null)
            {
                ClientContext.Load(SelectedWeb, w => w.Url);
                ClientContext.ExecuteQueryRetry();
                Url = SelectedWeb.Url;
            }
            SelectedWeb.AddNavigationNode(Title, new Uri(Url, UriKind.RelativeOrAbsolute), Header, Location, External.IsPresent, !First.IsPresent);
        }
    }
}
