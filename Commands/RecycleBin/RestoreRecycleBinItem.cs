using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.RecycleBin
{
    [Cmdlet(VerbsData.Restore, "PnpRecycleBinItem")]
    [CmdletHelp("Restores the provided recycle bin item to its original location",
        Category = CmdletHelpCategory.RecycleBin)]
    [CmdletExample(
        Code = @"PS:> Restore-PnpRecycleBinItem -Identity 72e4d749-d750-4989-b727-523d6726e442",
        Remarks = "Restores the recycle bin item with Id 72e4d749-d750-4989-b727-523d6726e442 to its original location",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPRecycleBinItems | ? FileLeafName -like ""*.docx"" | Restore-PnpRecycleBinItem",
        Remarks = "Restores all the items in the first and second stage recycle bins to their original location of which the filename ends with the .docx extension",
        SortOrder = 2)]
    
    public class RestoreRecycleBinItem : SPOCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Id of the recycle bin item or the recycle bin item object itself to restore", ValueFromPipeline = true, ParameterSetName = "Identity")]
        public RecycleBinItemPipeBind Identity;

        [Parameter(Mandatory = true, HelpMessage = "If provided all items will be stored ", ValueFromPipeline = true, ParameterSetName = "All")]
        public SwitchParameter All;

        [Parameter(Mandatory = false, HelpMessage = "If provided, no confirmation will be asked to restore the recycle bin item")]
        public SwitchParameter Force;


        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == "Identity")
            {
                var recycleBinItem = Identity.GetRecycleBinItem(ClientContext.Site);

                if (Force ||
                    ShouldContinue(string.Format(Resources.RestoreRecycleBinItem, recycleBinItem.LeafName),
                        Resources.Confirm))
                {
                    recycleBinItem.Restore();
                    ClientContext.ExecuteQueryRetry();
                }
            }
            else
            {
                if (Force || ShouldContinue(Resources.RestoreRecycleBinItems, Resources.Confirm))
                {
                    ClientContext.Site.RecycleBin.RestoreAll();
                    ClientContext.ExecuteQueryRetry();
                }
            }
        }
    }
}
