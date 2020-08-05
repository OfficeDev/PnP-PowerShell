#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.InformationManagement
{
    [Cmdlet(VerbsCommon.Reset, "PnPLabel")]
    [CmdletHelp("Resets a retention label on the specified list or library to None", 
        DetailedDescription = "Removes the retention label on a list or library and its items. Does not work for sensitivity labels.",
        Category = CmdletHelpCategory.InformationManagement, 
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Reset-PnPLabel -List ""Demo List""",
       Remarks = @"This resets an O365 label on the specified list or library to None", SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Reset-PnPLabel -List ""Demo List"" -SyncToItems $true",
       Remarks = @"This resets an O365 label on the specified list or library to None and resets the label on all the items in the list and library except Folders and where the label has been manually or previously automatically assigned", SortOrder = 2)]

    public class ResetLabel : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID or Url of the list")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "Reset label on existing items in the library")]
        public bool SyncToItems;

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);
            if (list != null)
            {
                var listUrl = list.RootFolder.ServerRelativeUrl;
                Microsoft.SharePoint.Client.CompliancePolicy.SPPolicyStoreProxy.SetListComplianceTag(ClientContext, listUrl, string.Empty, false, false, SyncToItems);

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
