#if !ONPREMISES
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.InformationManagement
{
    [Cmdlet(VerbsCommon.Get, "PnPLabel")]
    [CmdletHelp("Gets the Office 365 retention label/tag of the specified list or library (if applicable)", Category = CmdletHelpCategory.InformationManagement, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Get-PnPLabel -List ""Demo List"" -ValuesOnly",
       Remarks = @"This gets the Office 365 retention label which is set to a list or a library", SortOrder = 1)]

    public class GetLabel : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The ID or Url of the list")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "If provided, the results will be returned as values instead of in written text and will include more detailed information")]
        public SwitchParameter ValuesOnly;

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);
            if (list != null)
            {
                var listUrl = list.RootFolder.ServerRelativeUrl;
                var label = Microsoft.SharePoint.Client.CompliancePolicy.SPPolicyStoreProxy.GetListComplianceTag(ClientContext, listUrl);
                ClientContext.ExecuteQueryRetry();

                if (label.Value == null)
                {
                    WriteWarning("No label found for the specified library.");
                }
                else
                {
                    if (ParameterSpecified(nameof(ValuesOnly)))
                    {
                        WriteObject(label.Value);
                    }
                    else
                    {
                        WriteObject("The label '" + label.Value.TagName + "' is set to the specified list or library. ");
                        // There is no property yet that exposes if the SyncToItems is set or not.. :(
                        WriteObject("Block deletion: " + label.Value.BlockDelete.ToString());
                        WriteObject("Block editing: " + label.Value.BlockEdit.ToString());
                    }
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
