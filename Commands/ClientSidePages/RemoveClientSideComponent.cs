#if !SP2013 && !SP2016
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Linq;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Remove, "PnPClientSideComponent")]
    [CmdletHelp("Removes a Client-Side component from a page",
        SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019,
        Category = CmdletHelpCategory.WebParts)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPClientSideComponent -Page Home -InstanceId a2875399-d6ff-43a0-96da-be6ae5875f82",
        Remarks = @"Removes the control specified from the page.", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> $webpart = Get-PnPClientSideComponent -Page ""Home"" | Where-Object { $_.Title -eq ""Site activity"" }
PS:> Remove-PnPClientSideComponent -Page ""Home"" -InstanceId $webpart.InstanceId -Force",
        Remarks = @"Finds a web part with the Title ""Site activity"" on the Home.aspx page, then removes it from the page", SortOrder = 2)]
    public class RemoveClientSideComponent : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page")]
        public ClientSidePagePipeBind Page;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The instance id of the component")]
        public GuidPipeBind InstanceId;

        [Parameter(Mandatory = false, HelpMessage = "If specified you will not receive the confirmation question")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            var clientSidePage = Page.GetPage(ClientContext);

            if (clientSidePage == null)
                throw new Exception($"Page '{Page?.Name}' does not exist");

            var control = clientSidePage.Controls.FirstOrDefault(c => c.InstanceId == InstanceId.Id);
            if(control != null)
            { 
                if (Force || ShouldContinue(string.Format(Properties.Resources.RemoveComponentWithInstanceId0, control.InstanceId), Properties.Resources.Confirm))
                {
                    control.Delete();
                    clientSidePage.Save();
                }
            }
            else
            {
                throw new Exception($"Component with id {InstanceId.Id} does not exist on this page");
            }
        }
    }
}
#endif
