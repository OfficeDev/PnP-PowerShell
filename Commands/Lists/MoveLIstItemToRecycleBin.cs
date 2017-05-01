using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Move, "PnPListItemToRecycleBin", SupportsShouldProcess = true)]
    [CmdletAlias("Move-SPOListItemToRecycleBin")]
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
