#if !SP2013 && !SP2016
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using OfficeDevPnP.Core.Pages;

namespace PnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Set, "PnPClientSideText")]
    [CmdletHelp("Set Client-Side Text Component properties",
        SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019,
        DetailedDescription = "Sets the rendered text in existing client side text component",
        Category = CmdletHelpCategory.WebParts)]
    [CmdletExample(
        Code = @"PS:> Set-PnPSetClientSideText -Page Home -InstanceId a2875399-d6ff-43a0-96da-be6ae5875f82 -Text ""MyText""",
        Remarks = @"Sets the text of the client side text component.", SortOrder = 1)]
    public class SetClientSideText : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page")]
        public ClientSidePagePipeBind Page;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The instance id of the text component")]
        public GuidPipeBind InstanceId;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Text to set")]
        public string Text;

        protected override void ExecuteCmdlet()
        {
            var clientSidePage = Page.GetPage(ClientContext);

            if (clientSidePage == null)
                throw new Exception($"Page '{Page?.Name}' does not exist");

            var control = clientSidePage.Controls.FirstOrDefault(c => c.InstanceId == InstanceId.Id);
            if (control != null)
            {
                var textControl = control as ClientSideText;
                textControl.Text = Text;
                clientSidePage.Save();
            }
            else
            {
                throw new Exception($"Component does not exist");
            }
        }
    }
}
#endif