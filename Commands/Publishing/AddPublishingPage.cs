using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Publishing
{
    [Cmdlet(VerbsCommon.Add, "PnPPublishingPage")]
    [CmdletAlias("Add-SPOPublishingPage")]
    [CmdletHelp("Adds a publishing page",
      Category = CmdletHelpCategory.Publishing)]
    [CmdletExample(
        Code = @"PS:> Add-PnPPublishingPage -PageName 'OurNewPage' -Title 'Our new page' -PageTemplateName 'ArticleLeft'",
        Remarks = "Creates a new page based on the pagelayout 'ArticleLeft'",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Add-PnPPublishingPage -PageName 'OurNewPage' -Title 'Our new page' -PageTemplateName 'ArticleLeft' -Folder '/Pages/folder'",
        Remarks = "Creates a new page based on the pagelayout 'ArticleLeft' with a site relative folder path",
        SortOrder = 2)]

    public class AddPublishingPage : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The name of the page to be added as an aspx file")]
        [Alias("Name")]
        public string PageName = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The site relative folder path of the page to be added")]
        [Alias("Folder")]
        public string FolderPath = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The name of the page layout you want to use. Specify without the .aspx extension. So 'ArticleLeft' or 'BlankWebPartPage'")]
        public string PageTemplateName = null;

        [Parameter(Mandatory = false, ParameterSetName = "WithTitle", HelpMessage = "The title of the page")]
        public string Title;

        [Parameter(Mandatory = false, HelpMessage = "Publishes the page. Also Approves it if moderation is enabled on the Pages library.")]
        public SwitchParameter Publish;

        protected override void ExecuteCmdlet()
        {
            Folder pageFolder = null;
            if(!string.IsNullOrEmpty(FolderPath))
            {
                pageFolder = SelectedWeb.EnsureFolderPath(FolderPath);
            }

            switch (ParameterSetName)
            {
                case "WithTitle":
                    {
                        SelectedWeb.AddPublishingPage(PageName, PageTemplateName, Title, publish: Publish, folder: pageFolder);
                        break;
                    }
                default:
                    {
                        SelectedWeb.AddPublishingPage(PageName, PageTemplateName, publish: Publish, folder: pageFolder);
                        break;
                    }
            }
        }
    }
}
