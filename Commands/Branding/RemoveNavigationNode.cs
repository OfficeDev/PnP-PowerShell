using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Enums;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Remove, "PnPNavigationNode", SupportsShouldProcess = true)]
    [CmdletAlias("Remove-SPONavigationNode")]
    [CmdletHelp("Removes a menu item from either the quicklaunch or top navigation", 
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = @"PS:> Remove-PnPNavigationNode -Title Recent -Location QuickLaunch",
        Remarks = "Will remove the recent navigation node from the quick launch in the current web.",
        SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Remove-PnPNavigationNode -Title Home -Location TopNavigationBar -Force",
        Remarks = "Will remove the home navigation node from the top navigation bar without prompting for a confirmation in the current web.",
        SortOrder = 2)]
    public class RemoveNavigationNode : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The location from where to remove the node (QuickLaunch, TopNavigationBar")]
        public NavigationType Location;

        [Parameter(Mandatory = true, HelpMessage = "The title of the node that needs to be removed")]
        public string Title;

        [Parameter(Mandatory = false, HelpMessage = "The header where the node is located")]
        public string Header;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (Force || ShouldContinue(string.Format(Resources.RemoveNavigationNode0, Title), Resources.Confirm))
            {
                SelectedWeb.DeleteNavigationNode(Title, Header, Location);
            }
        }

    }


}
