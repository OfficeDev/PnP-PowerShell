using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Move, "PnPListItemToRecycleBin", SupportsShouldProcess = true)]
    [CmdletHelp("Moves an item from a list to the Recycle Bin",
        Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
        Code = @"PS:> Move-PnPListItemToRecycleBin -List ""Demo List"" -Identity ""1"" -Force",
        SortOrder = 1,
        Remarks = @"Moves the listitem with id ""1"" from the ""Demo List"" list to the Recycle Bin.")]
    public class MoveListItemToRecycleBin : PnPWebCmdlet
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
            if (list == null)
                throw new PSArgumentException($"No list found with id, title or url '{List}'", "List");
            if (Identity != null)
            {
                var item = Identity.GetListItem(list);
                if (Force || ShouldContinue(string.Format(Properties.Resources.MoveListItemWithId0ToRecycleBin,item.Id), Properties.Resources.Confirm))
                {
                    item.Recycle();
                    ClientContext.ExecuteQueryRetry();
                }
            }
        }
    }
}
