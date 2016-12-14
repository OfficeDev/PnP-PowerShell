using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Remove, "PnPListItem", SupportsShouldProcess = true)]
    [CmdletAlias("Remove-SPOListItem")]
    [CmdletHelp("Deletes an item from a list",
        Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPListItem -List ""Demo List"" -Identity ""1"" -Force",
        SortOrder = 1,
        Remarks = @"Removes the listitem with id ""1"" from the ""Demo List"" list.")]
    public class RemoveListItem : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Title or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "The ID of the listitem, or actual ListItem object")]
        public ListItemPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);
            if (Identity != null)
            {
                var item = Identity.GetListItem(list);
                if (Force || ShouldContinue(string.Format(Properties.Resources.RemoveListItemWithId0,item.Id), Properties.Resources.Confirm))
                {
                    item.DeleteObject();
                    ClientContext.ExecuteQueryRetry();
                }
            }
        }
    }
}
