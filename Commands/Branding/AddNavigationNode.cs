using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Enums;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Add, "PnPNavigationNode")]
    [CmdletHelp("Adds an item to a navigation element",
        "Adds a menu item to either the quicklaunch or top navigation",
        OutputType = typeof(NavigationNode),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.NavigationNode.aspx",
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
    [CmdletExample(
        Code = @"PS:> Add-PnPNavigationNode -Title ""Wiki"" -Location ""QuickLaunch"" -Url ""wiki/""",
        Remarks = @"Adds a navigation node to the quicklaunch. The navigation node will have the title ""Wiki"" and will link to Wiki library on the selected Web.",
        SortOrder = 5)]
    public class AddNavigationNode : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The location of the node to add. Either TopNavigationBar, QuickLaunch or SearchNav")]
        public NavigationType Location;

        [Parameter(Mandatory = true, HelpMessage = "The title of the node to add")]
        public string Title;

        [Parameter(Mandatory = false, HelpMessage = "The url to navigate to when clicking the new menu item. This can either be absolute or relative to the Web. Fragments are not supported.")]
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

            var newNavNode = SelectedWeb.AddNavigationNode(Title, new Uri(Url, UriKind.RelativeOrAbsolute), Header, Location, External.IsPresent, !First.IsPresent);
            WriteObject(newNavNode);
        }
    }
}
