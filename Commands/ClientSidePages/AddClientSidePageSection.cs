#if !SP2013 && !SP2016
using OfficeDevPnP.Core.Pages;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Add, "PnPClientSidePageSection")]
    [CmdletHelp("Adds a new section to a Client-Side page",
      Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(
        Code = @"PS:> Add-PnPClientSidePageSection -Page ""MyPage"" -SectionTemplate OneColumn",
        Remarks = "Adds a new one-column section to the Client-Side page 'MyPage'",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Add-PnPClientSidePageSection -Page ""MyPage"" -SectionTemplate ThreeColumn -Order 10",
        Remarks = "Adds a new Three columns section to the Client-Side page 'MyPage' with an order index of 10",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> $page = Add-PnPClientSidePage -Name ""MyPage""
PS> Add-PnPClientSidePageSection -Page $page -SectionTemplate OneColumn",
        Remarks = "Adds a new one column section to the Client-Side page 'MyPage'",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> $page = Add-PnPClientSidePage -Name ""MyPage""
PS> Add-PnPClientSidePageSection -Page $page -SectionTemplate OneColumn -ZoneEmphasis 2",
        Remarks = "Adds a new one column section to the Client-Side page 'MyPage' and sets the background to 2 (0 is no background, 3 is highest emphasis)",
        SortOrder = 4)]
    public class AddClientSidePageSection : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page")]
        public ClientSidePagePipeBind Page;

        [Parameter(Mandatory = true, HelpMessage = "Specifies the columns template to use for the section.")]
        public CanvasSectionTemplate SectionTemplate;

        [Parameter(Mandatory = false, HelpMessage = "Sets the order of the section. (Default = 1)")]
        public int Order = 1;

        [Parameter(Mandatory = false, HelpMessage = "Sets the background of the section (default = 0)")]
        public int ZoneEmphasis = 0;


        protected override void ExecuteCmdlet()
        {
            var clientSidePage = Page?.GetPage(ClientContext);

            if (clientSidePage != null)
            {
                clientSidePage.AddSection(new CanvasSection(clientSidePage, SectionTemplate, Order) { ZoneEmphasis = ZoneEmphasis });
                clientSidePage.Save();
            }
            else
            {
                // If the client side page object cannot be found
                throw new Exception($"Page {Page} cannot be found.");
            }

        }
    }
}
#endif
