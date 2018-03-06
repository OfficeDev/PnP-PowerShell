using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Enums;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Remove, "PnPNavigationNode", DefaultParameterSetName = ParameterSet_BYID, SupportsShouldProcess = true)]
    [CmdletHelp("Removes a menu item from either the quicklaunch or top navigation",
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPNavigationNode -Identity 1032",
        Remarks = @"Removes the navigation node with the specified id",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> $nodes = Get-PnPNavigationNode -QuickLaunch
PS:>$nodes | Select-Object -First 1 | Remove-PnPNavigationNode -Force",
        Remarks = @"Retrieves all navigation nodes from the Quick Launch navigation, then removes the first node in the list and it will not ask for a confirmation",
        SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Remove-PnPNavigationNode -Title Recent -Location QuickLaunch",
        Remarks = "Will remove the recent navigation node from the quick launch in the current web.",
        SortOrder = 3)]
    [CmdletExample(Code = @"PS:> Remove-PnPNavigationNode -Title Home -Location TopNavigationBar -Force",
        Remarks = "Will remove the home navigation node from the top navigation bar without prompting for a confirmation in the current web.",
        SortOrder = 4)]
    [CmdletExample(Code = @"PS:> Remove-PnPNavigationNode -Location QuickLaunch -All",
        Remarks = "Will remove all the navigation nodes from the quick launch bar in the current web.",
        SortOrder = 5)]
    public class RemoveNavigationNode : PnPWebCmdlet
    {
        private const string ParameterSet_BYNAME = "Remove node by Title";
        private const string ParameterSet_BYID = "Remove a node by ID";
        private const string ParameterSet_REMOVEALLNODES = "All Nodes";

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "The Id or node object to delete", ParameterSetName = ParameterSet_BYID)]
        public NavigationNodePipeBind Identity;

        [Obsolete("Use -Identity with an Id instead.")]
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "The location from where to remove the node (QuickLaunch, TopNavigationBar", ParameterSetName = ParameterSet_BYNAME)]
        public NavigationType Location;

        [Obsolete("Use -Identity with an Id instead.")]
        [Parameter(Mandatory = true, HelpMessage = "The title of the node that needs to be removed", ParameterSetName = ParameterSet_BYNAME)]
        public string Title;

        [Obsolete("Use -Identity with an Id instead.")]
        [Parameter(Mandatory = false, HelpMessage = "The header where the node is located", ParameterSetName = ParameterSet_BYNAME)]
        public string Header;

        [Parameter(Mandatory = true, HelpMessage = "Specifying the All parameter will remove all the nodes from specifed Location.", ParameterSetName = ParameterSet_REMOVEALLNODES)]
        public SwitchParameter All;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;


        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == ParameterSet_REMOVEALLNODES)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                if (Force || ShouldContinue(string.Format(Resources.RemoveNavigationNodeInLocation, Location), Resources.Confirm))
                {
                    SelectedWeb.DeleteAllNavigationNodes(Location);
                }
#pragma warning restore CS0618 // Type or member is obsolete
            }
            else
            {
                if (Force || ShouldContinue("Remove node?", Resources.Confirm))
                {
                    if (ParameterSetName == ParameterSet_BYID)
                    {
                        var node = SelectedWeb.Navigation.GetNodeById(Identity.Id);
                        node.DeleteObject();
                        ClientContext.ExecuteQueryRetry();
                    }
                    else
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        SelectedWeb.DeleteNavigationNode(Title, Header, Location);
#pragma warning restore CS0618 // Type or member is obsolete
                    }
                }
            }
        }

    }
}
