#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.InformationManagement
{
    [Cmdlet(VerbsCommon.Set, "PnPLabel")]
    [CmdletHelp("Sets a retention label on the specified list or library. Use Reset-PnPLabel to remove the label again.",
        DetailedDescription = "Allows setting a retention label on a list or library and its items. Does not work for sensitivity labels.",
        Category = CmdletHelpCategory.InformationManagement, 
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Set-PnPLabel -List ""Demo List"" -Label ""Project Documentation""",
       Remarks = @"This sets an O365 label on the specified list or library. ", SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Set-PnPLabel -List ""Demo List"" -Label ""Project Documentation"" -SyncToItems $true",
       Remarks = @"This sets an O365 label on the specified list or library and sets the label to all the items in the list and library as well.", SortOrder = 2)]

    [CmdletExample(
       Code = @"PS:> Set-PnPLabel -List ""Demo List"" -Label ""Project Documentation"" -BlockDelete $true -BlockEdit $true",
       Remarks = @"This sets an O365 label on the specified list or library. Next, it also blocks the ability to either edit or delete the item. ", SortOrder = 3)]
    public class SetLabel : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "The name of the retention label")]
        public string Label;

        [Parameter(Mandatory = false, HelpMessage = "Apply label to existing items in the library")]
        public bool SyncToItems;

        [Parameter(Mandatory = false, HelpMessage = "Block deletion of items in the library")]
        public bool BlockDeletion;

        [Parameter(Mandatory = false, HelpMessage = "Block editing of items in the library")]
        public bool BlockEdit;

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);
            if (list != null)
            {
                var listUrl = list.RootFolder.ServerRelativeUrl;
                Microsoft.SharePoint.Client.CompliancePolicy.SPPolicyStoreProxy.SetListComplianceTag(ClientContext, listUrl, Label, BlockDeletion, BlockEdit, SyncToItems);

                try
                {
                    ClientContext.ExecuteQueryRetry();
                }
                catch (System.Exception error)
                {
                    WriteWarning(error.Message.ToString());
                }
            }
            else
            {
                WriteWarning("List or library not found.");
            }
        }
    }
}
#endif
