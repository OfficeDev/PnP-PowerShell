using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Publishing
{
    [Cmdlet(VerbsCommon.Add, "PnPWikiPage")]
    [CmdletAlias("Add-SPOWikiPage")]
    [CmdletHelp("Adds a wiki page",
        Category = CmdletHelpCategory.Publishing)]
    [CmdletExample(
        Code = @"PS:> Add-PnPWikiPage -PageUrl '/sites/demo1/pages/wikipage.aspx' -Content 'New WikiPage'",
        Remarks = "Creates a new wiki page '/sites/demo1/pages/wikipage.aspx' and sets the content to 'New WikiPage'",
        SortOrder = 1)]
    public class AddWikiPage : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The server relative page URL")]
        [Alias("PageUrl")]
        public string ServerRelativePageUrl = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = "WithContent")]
        public string Content = null;

        [Parameter(Mandatory = true, ParameterSetName = "WithLayout")]
        public WikiPageLayout Layout;

        protected override void ExecuteCmdlet()
        {
            switch (ParameterSetName)
            {
                case "WithContent":
                    {
                        SelectedWeb.AddWikiPageByUrl(ServerRelativePageUrl, Content);
                        break;
                    }
                case "WithLayout":
                    {
                        SelectedWeb.AddWikiPageByUrl(ServerRelativePageUrl);
                        SelectedWeb.AddLayoutToWikiPage(Layout, ServerRelativePageUrl);
                        break;
                    }
                default:
                    {
                        SelectedWeb.AddWikiPageByUrl(ServerRelativePageUrl);
                        break;
                    }
            }
        }
    }
}
