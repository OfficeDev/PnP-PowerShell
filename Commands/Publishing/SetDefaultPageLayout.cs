using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Publishing
{
    [Cmdlet(VerbsCommon.Set, "PnPDefaultPageLayout")]
    [CmdletHelp("Sets a specific page layout to be the default page layout for a publishing site",
        Category = CmdletHelpCategory.Publishing)]
    [CmdletExample(
        Code = @"PS:> Set-PnPDefaultPageLayout -Title projectpage.aspx",
        Remarks = "Sets projectpage.aspx to be the default page layout for the current web",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPDefaultPageLayout -Title test/testpage.aspx",
        Remarks = "Sets a page layout in a folder in the Master Page & Page Layout gallery, such as _catalog/masterpage/test/testpage.aspx, to be the default page layout for the current web",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Set-PnPDefaultPageLayout -InheritFromParentSite",
        Remarks = "Sets the default page layout to be inherited from the parent site",
        SortOrder = 3)]
    public class SetDefaultPageLayout : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "TITLE", HelpMessage = "Title of the page layout")]
        public string Title = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = "INHERIT", HelpMessage = "Set the default page layout to be inherited from the parent site.")]
        public SwitchParameter InheritFromParentSite;

        protected override void ExecuteCmdlet()
        {
            if (InheritFromParentSite.IsPresent)
            {
                SelectedWeb.SetSiteToInheritPageLayouts();
            }
            else
            {
                var rootWeb = ClientContext.Site.RootWeb;
                SelectedWeb.SetDefaultPageLayoutForSite(rootWeb, Title);
            }
        }
    }
}
