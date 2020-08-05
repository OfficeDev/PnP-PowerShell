#if !SP2013 && !SP2016
using OfficeDevPnP.Core.Pages;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Add, "PnPClientSideText")]
    [CmdletHelp("Adds a text element to a client-side page.",
        "Adds a new text element to a section on a client-side page.",
      Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(
        Code = @"PS:> Add-PnPClientSideText -Page ""MyPage"" -Text ""Hello World!""",
        Remarks = "Adds the text 'Hello World!' to the Client-Side Page 'MyPage'",
        SortOrder = 1)]
    public class AddClientSideText : PnPWebCmdlet
    {
        private const string ParameterSet_DEFAULT = "Default";
        private const string ParameterSet_POSITIONED = "Positioned";

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page.", ParameterSetName = ParameterSet_DEFAULT)]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page.", ParameterSetName = ParameterSet_POSITIONED)]
        public ClientSidePagePipeBind Page;

        [Parameter(Mandatory = true, HelpMessage = "Specifies the text to display in the text area.", ParameterSetName = ParameterSet_DEFAULT)]
        [Parameter(Mandatory = true, HelpMessage = "Specifies the text to display in the text area.", ParameterSetName = ParameterSet_POSITIONED)]
        public string Text;

        [Parameter(Mandatory = false, HelpMessage = "Sets the order of the text control. (Default = 1)", ParameterSetName = ParameterSet_DEFAULT)]
        [Parameter(Mandatory = false, HelpMessage = "Sets the order of the text control. (Default = 1)", ParameterSetName = ParameterSet_POSITIONED)]
        public int Order = 1;

        [Parameter(Mandatory = true, HelpMessage = "Sets the section where to insert the text control.", ParameterSetName = ParameterSet_POSITIONED)]
        public int Section;

        [Parameter(Mandatory = true, HelpMessage = "Sets the column where to insert the text control.", ParameterSetName = ParameterSet_POSITIONED)]
        public int Column;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSpecified(nameof(Section)) && Section == 0)
            {
                throw new Exception("Section value should be at least 1 or higher");
            }

            if (ParameterSpecified(nameof(Column)) && Column == 0)
            {
                throw new Exception("Column value should be at least 1 or higher");
            }

            var clientSidePage = Page.GetPage(ClientContext);

            if (clientSidePage == null)
                // If the client side page object cannot be found
                throw new Exception($"Page {Page} cannot be found.");

            var textControl = new ClientSideText() { Text = Text };
            if (ParameterSpecified(nameof(Section)))
            {
                if (ParameterSpecified(nameof(Section)))
                {
                    clientSidePage.AddControl(textControl,
                    clientSidePage.Sections[Section - 1].Columns[Column - 1], Order);
                }
                else
                {
                    clientSidePage.AddControl(textControl, clientSidePage.Sections[Section - 1], Order);
                }
            }
            else
            {
                clientSidePage.AddControl(textControl, Order);
            }

            // Save the page
            clientSidePage.Save();

            WriteObject(textControl);
        }
    }
}
#endif