using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using Resources = PnP.PowerShell.Commands.Properties.Resources;

namespace PnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Remove, "PnPListItem", SupportsShouldProcess = true)]
    [CmdletHelp("Deletes an item from a list",
        Category = CmdletHelpCategory.Lists,
        SupportedPlatform = CmdletSupportedPlatform.All)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPListItem -List ""Demo List"" -Identity ""1"" -Force",
        SortOrder = 1,
        Remarks = @"Removes the listitem with id ""1"" from the ""Demo List"" list")]
    [CmdletExample(
        Code = @"PS:> Remove-PnPListItem -List ""Demo List"" -Identity ""1"" -Force -Recycle",
        SortOrder = 2,
        Remarks = @"Removes the listitem with id ""1"" from the ""Demo List"" list and saves it in the Recycle Bin")]
    public class RemoveListItem : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Title or Url of the list")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "The ID of the listitem, or actual ListItem object")]
        public ListItemPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "When provided, items will be sent to the recycle bin. When omitted, items will permanently be deleted.")]
        public SwitchParameter Recycle;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);
            if (list == null)
            {
                throw new PSArgumentException(string.Format(Resources.ListNotFound, List.ToString()));
            }
            if (Identity != null)
            {
                var item = Identity.GetListItem(list);
                if (Force || ShouldContinue(string.Format(Resources.RemoveListItemWithId0, item.Id), Resources.Confirm))
                {
                    if (Recycle)
                    {
                        item.Recycle();
                    }
                    else
                    {
                        item.DeleteObject();
                    }
                    ClientContext.ExecuteQueryRetry();
                }
            }
        }
    }
}
