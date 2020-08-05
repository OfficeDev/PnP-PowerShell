using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Enums;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Get, "PnPNavigationNode", DefaultParameterSetName = ParameterSet_ALLBYLOCATION)]
    [CmdletHelp("Returns all or a specific navigation node",
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(
        Code = @"PS:> Get-PnPNavigationNode",
        Remarks = @"Returns all navigation nodes in the quicklaunch navigation",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPNavigationNode -Location QuickLaunch",
        Remarks = @"Returns all navigation nodes in the quicklaunch navigation",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPNavigationNode -Location TopNavigationBar",
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

        [Parameter(Mandatory = false, HelpMessage = "The location of the nodes to retrieve. Either TopNavigationBar, QuickLaunch, SearchNav or Footer.", ParameterSetName = ParameterSet_ALLBYLOCATION)]
        public NavigationType Location = NavigationType.QuickLaunch;

        [Parameter(Mandatory = false, HelpMessage = "The Id of the node to retrieve", ParameterSetName = ParameterSet_BYID)]
        public int Id;

        [Parameter(Mandatory = false, HelpMessage = "Show a tree view of all navigation nodes")]
        public SwitchParameter Tree;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == ParameterSet_ALLBYLOCATION)
            {
                if (Tree.IsPresent)
                {
                    NavigationNodeCollection navigationNodes = null;
                    if (Location == NavigationType.SearchNav)
                    {
                        navigationNodes = SelectedWeb.Navigation.GetNodeById(1040).Children;
#if !ONPREMISES
                    }
                    else if (Location == NavigationType.Footer)
                    {
                        navigationNodes = SelectedWeb.LoadFooterNavigation();
#endif
                    }
                    else
                    {
                        navigationNodes = Location == NavigationType.QuickLaunch ? SelectedWeb.Navigation.QuickLaunch : SelectedWeb.Navigation.TopNavigationBar;
                    }
                    if (navigationNodes != null)
                    {
                        var nodesCollection = ClientContext.LoadQuery(navigationNodes);
                        ClientContext.ExecuteQueryRetry();
                        WriteObject(GetTree(nodesCollection, 0));
                    }
                }
                else
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
#if !ONPREMISES
                        case NavigationType.Footer:
                            {
                                nodes = SelectedWeb.LoadFooterNavigation();
                                break;
                            }
#endif
                    }
                    if (nodes != null)
                    {
                        ClientContext.Load(nodes);
                        ClientContext.ExecuteQueryRetry();
                        WriteObject(nodes, true);
                    }
                }
            }
            if (ParameterSpecified(nameof(Id)))
            {
                var node = SelectedWeb.Navigation.GetNodeById(Id);
                ClientContext.Load(node);
                ClientContext.Load(node, n => n.Children.IncludeWithDefaultProperties());
                ClientContext.ExecuteQueryRetry();
                if (Tree.IsPresent)
                {
                    WriteObject(GetTree(new List<NavigationNode>() { node }, 0));
                }
                else
                {
                    WriteObject(node);
                }
            }
        }

        private List<string> GetTree(IEnumerable<NavigationNode> nodes, int level)
        {
            var lines = new List<string>();
            var line = "";
            if (level > 0)
            {
                line = string.Join("", Enumerable.Repeat("  ", level));
            }
            var index = 1;
            foreach (var node in nodes)
            {
                if (!node.IsObjectPropertyInstantiated("Children"))
                {
                    ClientContext.Load(node.Children);
                    ClientContext.ExecuteQueryRetry();
                }

                line += "──";

                line += $" [{node.Id}] - {node.Title} - {node.Url}";
                lines.Add(line);
                if (node.Children != null && node.Children.Any())
                {
                    lines.AddRange(GetTree(node.Children.AsEnumerable(), level + 1));
                }
                index++;
                line = "";
            }
            return lines;
        }
    }
}
