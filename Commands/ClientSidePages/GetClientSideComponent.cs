#if !SP2013 && !SP2016
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Get, "PnPClientSideComponent")]
    [CmdletHelp("Retrieve one or more Client-Side components from a site page",
        DetailedDescription = "This command allows the retrieval of the components placed on a modern sitepage along with its properties. Note that for a newly created modern site, the Home.aspx page will not be returning any components. This is because the underlying CanvasContent1 will not be populated until the homepage has been edited and published. The reason for this behavior is to allow for the default homepage to be able to be updated by Microsoft as long as it hasn't been modified. For any other site page or after editing and publishing the homepage, this command will return the correct components as they are positioned on the site page.",
        Category = CmdletHelpCategory.WebParts,
        SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(
        Code = @"PS:> Get-PnPClientSideComponent -Page Home",
        Remarks = @"Returns all controls defined on the given page.", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPClientSideComponent -Page Home -InstanceId a2875399-d6ff-43a0-96da-be6ae5875f82",
        Remarks = @"Returns a specific control defined on the given page.", SortOrder = 2)]
    public class GetClientSideControl : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page")]
        public ClientSidePagePipeBind Page;

        [Parameter(Mandatory = false, ValueFromPipeline = true, HelpMessage = "The instance id of the component")]
        public GuidPipeBind InstanceId;

        protected override void ExecuteCmdlet()
        {
            var clientSidePage = Page.GetPage(ClientContext);

            if (clientSidePage == null)
                throw new Exception($"Page '{Page?.Name}' does not exist");

            if(!ParameterSpecified(nameof(InstanceId)))
            {
                WriteObject(clientSidePage.Controls, true);
            } else
            {
                WriteObject(clientSidePage.Controls.FirstOrDefault(c => c.InstanceId == InstanceId.Id));
            }

        }
    }
}
#endif