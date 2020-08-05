#if !SP2013 && !SP2016
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Get, "PnPClientSidePage")]
    [CmdletHelp("Gets a modern site page",
      DetailedDescription = "This command allows the retrieval of a modern sitepage along with its properties and contents on it. Note that for a newly created modern site, the Columns and Sections of the Home.aspx page will not be filled according to the actual site page contents. This is because the underlying CanvasContent1 will not be populated until the homepage has been edited and published. The reason for this behavior is to allow for the default homepage to be able to be updated by Microsoft as long as it hasn't been modified. For any other site page or after editing and publishing the homepage, this command will return the correct columns and sections as they are positioned on the site page.",
      Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
     [CmdletExample(
        Code = @"PS:> Get-PnPClientSidePage -Identity ""MyPage.aspx""",
        Remarks = "Gets the Modern Page (Client-Side) named 'MyPage.aspx' in the current SharePoint site",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPClientSidePage ""MyPage""",
        Remarks = "Gets the Modern Page (Client-Side) named 'MyPage.aspx' in the current SharePoint site",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPClientSidePage ""Templates/MyPageTemplate""",
        Remarks = "Gets the Modern Page (Client-Side) named 'MyPageTemplate.aspx' from the templates folder of the Page Library in the current SharePoint site",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-PnPClientSidePage -Identity ""MyPage.aspx"" -Web (Get-PnPWeb -Identity ""Subsite1"")",
        Remarks = "Gets the Modern Page (Client-Side) named 'MyPage.aspx' from the subsite named 'Subsite1'",
        SortOrder = 4)]
    public class GetClientSidePage : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page")]
        public ClientSidePagePipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var clientSidePage = Identity.GetPage((Microsoft.SharePoint.Client.ClientContext)SelectedWeb.Context);

            if (clientSidePage == null)
                throw new Exception($"Page '{Identity?.Name}' does not exist");

            WriteObject(clientSidePage);
        }
    }
}
#endif