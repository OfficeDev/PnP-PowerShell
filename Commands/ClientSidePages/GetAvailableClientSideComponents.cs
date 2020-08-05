#if !SP2013 && !SP2016
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Get, "PnPAvailableClientSideComponents")]
    [CmdletHelp("Gets the available client side components on a particular page",
      Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(
        Code = @"PS:> Get-PnPAvailableClientSideComponents -Page ""MyPage.aspx""",
        Remarks = "Gets the list of available client side components on the page 'MyPage.aspx'",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPAvailableClientSideComponents $page",
        Remarks = "Gets the list of available client side components on the page contained in the $page variable",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPAvailableClientSideComponents -Page ""MyPage.aspx"" -ComponentName ""HelloWorld""",
        Remarks = "Gets the client side component 'HelloWorld' if available on the page 'MyPage.aspx'",
        SortOrder = 3)]
    public class GetAvailableClientSideComponents : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page.")]
        public ClientSidePagePipeBind Page;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the component instance or Id to look for.")]
        public ClientSideComponentPipeBind Component;

        protected override void ExecuteCmdlet()
        {
            var clientSidePage = Page.GetPage(ClientContext);
            if (clientSidePage == null)
                throw new PSArgumentException($"Page '{Page}' does not exist", "List");

            if (Component == null)
            {
                var allComponents = clientSidePage.AvailableClientSideComponents().Where(c => c.ComponentType == 1);
                WriteObject(allComponents, true);
            }
            else
            {
                WriteObject(Component.GetComponent(clientSidePage));
            }
        }
    }
}
#endif
