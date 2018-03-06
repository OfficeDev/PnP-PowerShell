#if !ONPREMISES
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Pages;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Add, "PnPClientSidePage")]
    [CmdletHelp("Adds a Client-Side Page",
      Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Add-PnPClientSidePage -Name ""NewPage""",
        Remarks = "Creates a new Client-Side page named 'NewPage'",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Add-PnPClientSidePage ""NewPage""",
        Remarks = "Creates a new Client-Side page named 'NewPage'",
        SortOrder = 2)]
    public class AddClientSidePage : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Specifies the name of the page.")]
        public string Name = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the layout type of the page.")]
        public ClientSidePageLayoutType LayoutType = ClientSidePageLayoutType.Article;

        [Parameter(Mandatory = false, HelpMessage = "Allows to promote the page for a specific purpose (HomePage | NewsPage)")]
        public ClientSidePagePromoteType PromoteAs = ClientSidePagePromoteType.None;

        [Parameter(Mandatory = false, HelpMessage = "Enables or Disables the comments on the page")]
        public SwitchParameter CommentsEnabled = false;

        [Parameter(Mandatory = false, HelpMessage = "Publishes the page once it is saved. Applicable to libraries set to create major and minor versions.")]
        public SwitchParameter Publish;

        [Obsolete("This parameter will be ignored")]
        [Parameter(Mandatory = false, HelpMessage = "Sets the message for publishing the page.")]
        public string PublishMessage = string.Empty;

        protected override void ExecuteCmdlet()
        {

            ClientSidePage clientSidePage = null;

            // Check if the page exists

            string name = ClientSidePageUtilities.EnsureCorrectPageName(Name);

            bool pageExists = false;
            try
            {
                ClientSidePage.Load(ClientContext, name);
                pageExists = true;
            }
            catch { }

            if(pageExists)
            {
                throw new Exception($"Page {name} already exists");
            }

            // Create a page that persists immediately
            clientSidePage = SelectedWeb.AddClientSidePage(name);
            clientSidePage.LayoutType = LayoutType;
            clientSidePage.Save(name);

            // If a specific promote type is specified, promote the page as Home or Article or ...
            switch (PromoteAs)
            {
                case ClientSidePagePromoteType.HomePage:
                    clientSidePage.PromoteAsHomePage();
                    break;
                case ClientSidePagePromoteType.NewsArticle:
                    clientSidePage.PromoteAsNewsArticle();
                    break;
                case ClientSidePagePromoteType.None:
                default:
                    break;
            }


            if (MyInvocation.BoundParameters.ContainsKey("CommentsEnabled"))
            {
                if (CommentsEnabled)
                {
                    clientSidePage.EnableComments();
                }
                else
                {
                    clientSidePage.DisableComments();
                }
            }

            if (Publish)
            {
                clientSidePage.Publish();
            }

            WriteObject(clientSidePage);
        }
    }
}
#endif