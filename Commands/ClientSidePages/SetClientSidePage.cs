#if !SP2013 && !SP2016
using OfficeDevPnP.Core.Pages;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;

namespace PnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Set, "PnPClientSidePage")]
    [CmdletHelp("Sets parameters of a Client-Side Page",
      Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(
        Code = @"PS:> Set-PnPClientSidePage -Identity ""MyPage"" -LayoutType Home -Title ""My Page""",
        Remarks = "Updates the properties of the Client-Side page named 'MyPage'",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPClientSidePage -Identity ""MyPage"" -CommentsEnabled",
        Remarks = "Enables the comments on the Client-Side page named 'MyPage'",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Set-PnPClientSidePage -Identity ""MyPage"" -CommentsEnabled:$false",
        Remarks = "Disables the comments on the Client-Side page named 'MyPage'",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Set-PnPClientSidePage -Identity ""MyPage"" -HeaderType Default",
        Remarks = "Sets the header of the page to the default header",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Set-PnPClientSidePage -Identity ""MyPage"" -HeaderType None",
        Remarks = "Removes the header of the page",
        SortOrder = 5)]
    [CmdletExample(
        Code = @"PS:> Set-PnPClientSidePage -Identity ""MyPage"" -HeaderType Custom -ServerRelativeImageUrl ""/sites/demo1/assets/myimage.png"" -TranslateX 10.5 -TranslateY 11.0",
        Remarks = "Sets the header of the page to custom header, using the specified image and translates the location of the image in the header given the values specified",
        SortOrder = 6)]
    [CmdletAdditionalParameter(ParameterType = typeof(string), ParameterName = "ServerRelativeImageUrl", HelpMessage = "The URL of the image to show in the header", ParameterSetName = ParameterSet_CUSTOMHEADER)]
    [CmdletAdditionalParameter(ParameterType = typeof(double), ParameterName = "TranslateX", HelpMessage = "A value defining how to translate the image on the x-axis", ParameterSetName = ParameterSet_CUSTOMHEADER)]
    [CmdletAdditionalParameter(ParameterType = typeof(double), ParameterName = "TranslateY", HelpMessage = "A value defining how to translate the image on the y-axis", ParameterSetName = ParameterSet_CUSTOMHEADER)]
    public class SetClientSidePage : PnPWebCmdlet, IDynamicParameters
    {
        const string ParameterSet_CUSTOMHEADER = "Custom Header";

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name/identity of the page")]
        public ClientSidePagePipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Sets the name of the page.")]
        public string Name = null;

        [Parameter(Mandatory = false, HelpMessage = "Sets the title of the page.")]
        public string Title = null;

        [Parameter(Mandatory = false, HelpMessage = "Sets the layout type of the page. (Default = Article)")]
        public ClientSidePageLayoutType LayoutType = ClientSidePageLayoutType.Article;

        [Parameter(Mandatory = false, HelpMessage = "Allows to promote the page for a specific purpose (None | HomePage | NewsArticle | Template)")]
        public ClientSidePagePromoteType PromoteAs = ClientSidePagePromoteType.None;

        [Parameter(Mandatory = false, HelpMessage = "Enables or Disables the comments on the page")]
        public SwitchParameter CommentsEnabled = false;

        [Parameter(Mandatory = false, HelpMessage = "Publishes the page once it is saved.")]
        public SwitchParameter Publish;

        [Parameter(Mandatory = false, HelpMessage = "Sets the page header type")]
        public ClientSidePageHeaderType HeaderType;

        [Parameter(Mandatory = false, HelpMessage = "Specify either the name, ID or an actual content type.")]
        public ContentTypePipeBind ContentType;

        [Parameter(Mandatory = false, HelpMessage = "Thumbnail Url")]
        public string ThumbnailUrl;

        [Obsolete("This parameter value will be ignored")]
        [Parameter(Mandatory = false, HelpMessage = "Sets the message for publishing the page.")]
        public string PublishMessage = string.Empty;

        private CustomHeaderDynamicParameters customHeaderParameters;

        public object GetDynamicParameters()
        {
            if (HeaderType == ClientSidePageHeaderType.Custom)
            {
                customHeaderParameters = new CustomHeaderDynamicParameters();
                return customHeaderParameters;
            }
            return null;
        }

        protected override void ExecuteCmdlet()
        {

            ClientSidePage clientSidePage = Identity?.GetPage(ClientContext);

            if (clientSidePage == null)
                // If the client side page object cannot be found
                throw new Exception($"Page {Identity?.Name} cannot be found.");

            // We need to have the page name, if not found, raise an error
            string name = ClientSidePageUtilities.EnsureCorrectPageName(Name ?? Identity?.Name);
            if (name == null)
                throw new Exception("Insufficient arguments to update a client side page");

            clientSidePage.LayoutType = LayoutType;

            if (Title != null)
            {
                clientSidePage.PageTitle = Title;
            }

            if(ThumbnailUrl != null)
            {
                clientSidePage.ThumbnailUrl = ThumbnailUrl;
            }

            if (ParameterSpecified(nameof(HeaderType)))
            {
                switch (HeaderType)
                {
                    case ClientSidePageHeaderType.Default:
                        {
                            clientSidePage.SetDefaultPageHeader();
                            break;
                        }
                    case ClientSidePageHeaderType.Custom:
                        {
                            clientSidePage.SetCustomPageHeader(customHeaderParameters.ServerRelativeImageUrl, customHeaderParameters.TranslateX, customHeaderParameters.TranslateY);
                            break;
                        }
                    case ClientSidePageHeaderType.None:
                        {
                            clientSidePage.RemovePageHeader();
                            break;
                        }
                }
            }

            if (PromoteAs == ClientSidePagePromoteType.Template)
            {
                clientSidePage.SaveAsTemplate(name);
            } else
            {
                clientSidePage.Save(name);
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

            if(ParameterSpecified(nameof(ContentType)))
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

            if (Publish)
            {
                clientSidePage.Publish();
            }

            WriteObject(clientSidePage);
        }

        public class CustomHeaderDynamicParameters
        {
            [Parameter(Mandatory = true)]
            public string ServerRelativeImageUrl { get; set; }

            [Parameter(Mandatory = false)]
            public double TranslateX { get; set; } = 0.0;

            [Parameter(Mandatory = false)]
            public double TranslateY { get; set; } = 0.0;
        }
    }
}
#endif
