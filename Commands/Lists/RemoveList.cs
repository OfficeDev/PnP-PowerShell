using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Remove, "PnPList", SupportsShouldProcess = true)]
    [CmdletHelp("Deletes a list",
        Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
        Code = "PS:> Remove-PnPList -Identity Announcements",
        SortOrder = 1,
        Remarks = @"Removes the list named 'Announcements'. Asks for confirmation.")]
    [CmdletExample(
        Code = "PS:> Remove-PnPList -Identity Announcements -Force",
        SortOrder = 2,
        Remarks = @"Removes the list named 'Announcements' without asking for confirmation.")]
    [CmdletExample(
        Code = "PS:> Remove-PnPList -Title Announcements -Recycle",
        SortOrder = 3,
        Remarks = @"Removes the list named 'Announcements' and saves to the Recycle Bin")]
    public class RemoveList : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID or Title of the list.")]
        public ListPipeBind Identity = new ListPipeBind();

        [Parameter(Mandatory = false)]
        public SwitchParameter Recycle;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;
        protected override void ExecuteCmdlet()
        {
            if (Identity != null)
            {
                var list = Identity.GetList(SelectedWeb);
                if (list != null)
                {
                    if (Force || ShouldContinue(Properties.Resources.RemoveList, Properties.Resources.Confirm))
                    {
                        if (Recycle)
                        {
                            list.Recycle();
                        }
                        else
                        {
                            list.DeleteObject();
                        }
                        ClientContext.ExecuteQueryRetry();
                    }
                }
            }
        }
    }

}
