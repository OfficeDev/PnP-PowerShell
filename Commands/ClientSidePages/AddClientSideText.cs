#if !ONPREMISES
using OfficeDevPnP.Core.Pages;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Add, "PnPClientSideText")]
    [CmdletHelp("Adds a Client-Side Page",
      Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Add-PnPClientSideText -Page ""MyPage"" -Text ""Hello World!""",
        Remarks = "Adds the text 'Hello World!' to the Client-Side Page 'MyPage'",
        SortOrder = 1)]
    public class AddClientSideText : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page.", ParameterSetName = "Default")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page.", ParameterSetName = "Positioned")]
        public ClientSidePagePipeBind Page;

        [Parameter(Mandatory = true, HelpMessage = "Specifies the text to display in the text area.", ParameterSetName = "Default")]
        [Parameter(Mandatory = true, HelpMessage = "Specifies the text to display in the text area.", ParameterSetName = "Positioned")]
        public string Text;

        [Parameter(Mandatory = false, HelpMessage = "Sets the order of the text control. (Default = 1)", ParameterSetName = "Default")]
        [Parameter(Mandatory = false, HelpMessage = "Sets the order of the text control. (Default = 1)", ParameterSetName = "Positioned")]
        public int Order = 1;

        [Parameter(Mandatory = true, HelpMessage = "Sets the section where to insert the text control.", ParameterSetName = "Positioned")]
        public int Section;

        [Parameter(Mandatory = true, HelpMessage = "Sets the column where to insert the text control.", ParameterSetName = "Positioned")]
        public int Column;

        protected override void ExecuteCmdlet()
        {
            if (MyInvocation.BoundParameters.ContainsKey("Section") && Section == 0)
            {
                throw new Exception("Section value should be at least 1 or higher");
            }

            if (MyInvocation.BoundParameters.ContainsKey("Column") && Column == 0)
            {
                throw new Exception("Column value should be at least 1 or higher");
            }

            var clientSidePage = Page.GetPage(ClientContext);

            if (clientSidePage == null)
                // If the client side page object cannot be found
                throw new Exception($"Page {Page} cannot be found.");

            var text = new ClientSideText() { Text = Text };
            if (MyInvocation.BoundParameters.ContainsKey("Section"))
            {
                if (MyInvocation.BoundParameters.ContainsKey("Section"))
                {
                    clientSidePage.AddControl(text,
                    clientSidePage.Sections[Section - 1].Columns[Column - 1], Order);
                }
                else
                {
                    clientSidePage.AddControl(text, clientSidePage.Sections[Section - 1], Order);
                }
            }
            else
            {
                clientSidePage.AddControl(text, Order);
            }

            // Save the page
            clientSidePage.Save();
        }
    }
}
#endif