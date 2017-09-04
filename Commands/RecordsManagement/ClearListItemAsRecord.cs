#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.RecordsManagement
{
    [Cmdlet(VerbsCommon.Clear, "PnPListItemAsRecord")]
    [CmdletHelp("Undeclares a list item as a record",
        Category = CmdletHelpCategory.RecordsManagement, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Clear-PnPListItemAsRecord -List ""Documents"" -Identity 4",
        Remarks = "Undeclares the document in the documents library with id 4 as a record",
        SortOrder = 1)]
    public class ClearListItemAsRecord : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Title or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The ID of the listitem, or actual ListItem object")]
        public ListItemPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);

            var item = Identity.GetListItem(list);

            Microsoft.SharePoint.Client.RecordsRepository.Records.UndeclareItemAsRecord(ClientContext, item);

            ClientContext.ExecuteQueryRetry();
        }

    }

}
#endif