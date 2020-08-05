#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.RecordsManagement
{
    [Cmdlet(VerbsDiagnostic.Test, "PnPListItemIsRecord")]
    [CmdletHelp("Checks if a list item is a record",
        Category = CmdletHelpCategory.RecordsManagement, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Test-PnPListItemIsRecord -List ""Documents"" -Identity 4",
        Remarks = "Returns true if the document in the documents library with id 4 is a record",
        SortOrder = 1)]
    public class TestListItemIsRecord : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Title or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The ID of the listitem, or actual ListItem object")]
        public ListItemPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);
            if (list == null)
                throw new PSArgumentException($"No list found with id, title or url '{List}'", "List");

            var item = Identity.GetListItem(list);

            var returnValue = Microsoft.SharePoint.Client.RecordsRepository.Records.IsRecord(ClientContext, item);

            ClientContext.ExecuteQueryRetry();

            WriteObject(returnValue.Value);
        }
    }
}
#endif