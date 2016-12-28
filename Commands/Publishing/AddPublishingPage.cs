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
        Code = @"PS:> Add-PnPPublishingPage -PageName 'OurNewPage' -Title 'Our new page' -PageTemplateName 'ArticleLeft' -Folder '/Pages/folder/folder2'",
        Remarks = "Creates a new page based on the pagelayout 'ArticleLeft' within a folder",
        SortOrder = 2)]

    public class AddPublishingPage : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The name of the page to be added as an aspx file")]
        [Alias("Name")]
        public string PageName = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The name of the page layout you want to use. Specify without the .aspx extension. So 'ArticleLeft' or 'BlankWebPartPage'")]
        public string PageTemplateName = null;

        [Parameter(Mandatory = false, ParameterSetName = "WithTitle", HelpMessage = "The title of the page")]
        public string Title;

        [Parameter(Mandatory = false, HelpMessage = "The folder of the page")]
        public string Folder = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Publishes the page. Also Approves it if moderation is enabled on the Pages library.")]
        public SwitchParameter Publish;

        protected override void ExecuteCmdlet()
        {
            Folder pageFolder = null;
            if(!string.IsNullOrEmpty(Folder))
            {
                pageFolder = SelectedWeb.EnsureFolderPath(Folder);
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
