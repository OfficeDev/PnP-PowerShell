using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Enums;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Get, "PnPNavigationNode", DefaultParameterSetName = ParameterSet_ALLBYLOCATION)]
    [CmdletHelp("Returns all or a specific navigation node",
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(
        Code = @"PS:> Get-PnPNavigationNode",
        Remarks = @"Returns all navigation nodes in the quicklaunch navigation",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPNavigationNode -QuickLaunch",
        Remarks = @"Returns all navigation nodes in the quicklaunch navigation",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPNavigationNode -TopNavigationBar",
        Remarks = @"Returns all navigation nodes in the top navigation bar",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> $node = Get-PnPNavigationNode -Id 2030
PS> $children = $node.Children",
        Remarks = @"Returns the selected navigation node and retrieves any children",
        SortOrder = 3)]
    public class GetNavigationNode : PnPWebCmdlet
    {
        private const string ParameterSet_ALLBYLOCATION = "All nodes by location";
        private const string ParameterSet_BYID = "A single node by ID";

        [Parameter(Mandatory = false, HelpMessage = "The location of the nodes to retrieve. Either TopNavigationBar, QuickLaunch", ParameterSetName = ParameterSet_ALLBYLOCATION)]
        public NavigationType Location = NavigationType.QuickLaunch;

        [Parameter(Mandatory = false, HelpMessage = "The Id of the node to retrieve", ParameterSetName = ParameterSet_BYID)]
        public int Id;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == ParameterSet_ALLBYLOCATION)
            {
                NavigationNodeCollection nodes = null;
                switch (Location)
                {
                    case NavigationType.QuickLaunch:
                        {
                            nodes = SelectedWeb.Navigation.QuickLaunch;
                            break;
                        }
                    case NavigationType.TopNavigationBar:
                        {
                            nodes = SelectedWeb.Navigation.TopNavigationBar;
                            break;
                        }
                    case NavigationType.SearchNav:
                        {
                            nodes = SelectedWeb.Navigation.GetNodeById(1040).Children;
                            break;
                        }
                }
                ClientContext.Load(nodes);
                ClientContext.ExecuteQueryRetry();
                WriteObject(nodes, true);
            }
            if (MyInvocation.BoundParameters.ContainsKey("Id"))
            {
                var node = SelectedWeb.Navigation.GetNodeById(Id);
                ClientContext.Load(node);
                ClientContext.Load(node, n => n.Children.IncludeWithDefaultProperties());
                ClientContext.ExecuteQueryRetry();
                WriteObject(node);
            }
        }
    }
}
