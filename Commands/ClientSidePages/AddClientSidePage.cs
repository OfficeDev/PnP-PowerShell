#if !SP2013 && !SP2016
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Pages;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Add, "PnPClientSidePage")]
    [CmdletHelp("Adds a Client-Side Page",
      Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(
        Code = @"PS:> Add-PnPClientSidePage -Name ""NewPage""",
        Remarks = "Creates a new Client-Side page named 'NewPage'",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Add-PnPClientSidePage -Name ""NewPage"" -ContentType ""MyPageContentType""",
        Remarks = "Creates a new Client-Side page named 'NewPage' and sets the content type to the content type specified",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Add-PnPClientSidePage -Name ""NewPageTemplate"" -PromoteAs Template",
        Remarks = "Creates a new Client-Side page named 'NewPage' and saves as a template to the site.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Add-PnPClientSidePage -Name ""Folder/NewPage""",
        Remarks = "Creates a new Client-Side page named 'NewPage' under 'Folder' folder and saves as a template to the site.",
        SortOrder = 3)]
#if !ONPREMISES
    [CmdletExample(
        Code = @"PS:> Add-PnPClientSidePage -Name ""NewPage"" -HeaderLayoutType ColorBlock",
        Remarks = "Creates a new Client-Side page named 'NewPage' using the ColorBlock header layout",
        SortOrder = 4)]
#endif
    public class AddClientSidePage : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Specifies the name of the page.")]
        public string Name = null;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the layout type of the page.")]
        public ClientSidePageLayoutType LayoutType = ClientSidePageLayoutType.Article;

        [Parameter(Mandatory = false, HelpMessage = "Allows to promote the page for a specific purpose (HomePage | NewsPage)")]
        public ClientSidePagePromoteType PromoteAs = ClientSidePagePromoteType.None;

        [Parameter(Mandatory = false, HelpMessage = "Specify either the name, ID or an actual content type.")]
        public ContentTypePipeBind ContentType;

        [Parameter(Mandatory = false, HelpMessage = "Enables or Disables the comments on the page")]
        public SwitchParameter CommentsEnabled = false;

        [Parameter(Mandatory = false, HelpMessage = "Publishes the page once it is saved. Applicable to libraries set to create major and minor versions.")]
        public SwitchParameter Publish;

#if !ONPREMISES
        [Parameter(Mandatory = false, HelpMessage = "Type of layout used for the header")]
        public ClientSidePageHeaderLayoutType HeaderLayoutType = ClientSidePageHeaderLayoutType.FullWidthImage;
#endif
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

#if !ONPREMISES
            clientSidePage.PageHeader.LayoutType = HeaderLayoutType;
#endif
            if (PromoteAs == ClientSidePagePromoteType.Template)
            {
                clientSidePage.SaveAsTemplate(name);
            }
            else
            {
                clientSidePage.Save(name);
            }

            if (ParameterSpecified(nameof(ContentType)))
            {
                ContentType ct = null;
                if (ContentType.ContentType == null)
                {
                    if (ContentType.Id != null)
                    {
                        ct = SelectedWeb.GetContentTypeById(ContentType.Id, true);
                    }
                    else if (ContentType.Name != null)
                    {
                        ct = SelectedWeb.GetContentTypeByName(ContentType.Name, true);
                    }
                }
                else
                {
                    ct = ContentType.ContentType;
                }
                if (ct != null)
                {
                    ct.EnsureProperty(w => w.StringId);

                    clientSidePage.PageListItem["ContentTypeId"] = ct.StringId;
                    clientSidePage.PageListItem.SystemUpdate();
                    ClientContext.ExecuteQueryRetry();
                }
            }

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

            if (ParameterSpecified(nameof(CommentsEnabled)))
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
