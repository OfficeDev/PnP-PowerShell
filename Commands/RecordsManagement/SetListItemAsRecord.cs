#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;

namespace PnP.PowerShell.Commands.RecordsManagement
{
    [Cmdlet(VerbsCommon.Set, "PnPListItemAsRecord")]
    [CmdletHelp("Declares a list item as a record, for more information, see https://docs.microsoft.com/sharepoint/governance/records-management-in-sharepoint-server",
        Category = CmdletHelpCategory.RecordsManagement, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Set-PnPListItemAsRecord -List ""Documents"" -Identity 4",
        Remarks = "Declares the document in the documents library with id 4 as a record",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPListItemAsRecord -List ""Documents"" -Identity 4 -DeclarationDate $date",
        Remarks = "Declares the document in the documents library with id as a record",
        SortOrder = 2)]
    public class SetListItemAsRecord : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Title or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The ID of the listitem, or actual ListItem object")]
        public ListItemPipeBind Identity;

        [Parameter(Mandatory = false, ValueFromPipeline = false, HelpMessage = "The declaration date")]
        public DateTime DeclarationDate;

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);
            if (list == null)
                throw new PSArgumentException($"No list found with id, title or url '{List}'", "List");

            var item = Identity.GetListItem(list);

            if (!ParameterSpecified(nameof(DeclarationDate)))
            {
                Microsoft.SharePoint.Client.RecordsRepository.Records.DeclareItemAsRecord(ClientContext, item);
            }
            else
            {
                Microsoft.SharePoint.Client.RecordsRepository.Records.DeclareItemAsRecordWithDeclarationDate(ClientContext, item, DeclarationDate);
            }
            ClientContext.ExecuteQueryRetry();
        }

    }
}
#endif
