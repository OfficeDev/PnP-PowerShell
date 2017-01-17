#if !ONPREMISES
using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.RecycleBin
{
    [Cmdlet(VerbsCommon.Move, "PnpRecycleBinItem")]
    [CmdletHelp("Moves all items or a specific item in the first stage recycle bin of the current site collection to the second stage recycle bin",
        Category = CmdletHelpCategory.RecycleBin)]
    [CmdletExample(
        Code = @"PS:> Move-PnpRecycleBinItem",
        Remarks = "Moves all the items in the first stage recycle bin of the current site collection to the second stage recycle bin",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Move-PnpRecycleBinItem -Identity 26ffff29-b526-4451-9b6f-7f0e56ba7125",
        Remarks = "Moves the item with the provided ID in the first stage recycle bin of the current site collection to the second stage recycle bin without asking for confirmation first",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Move-PnpRecycleBinItem -Force",
        Remarks = "Moves all the items in the first stage recycle bin of the current context to the second stage recycle bin without asking for confirmation first",
        SortOrder = 3)]
    public class MoveRecycleBinItems : SPOCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "If provided, moves the item with the specific ID to the second stage recycle bin", ValueFromPipeline = true)]
        public RecycleBinItemPipeBind Identity;
        [Parameter(Mandatory = false, HelpMessage = "If provided, no confirmation will be asked to move the first stage recycle bin items to the second stage")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (MyInvocation.BoundParameters.ContainsKey("Identity"))
            {
                var item = Identity.GetRecycleBinItem(ClientContext.Site);
                if (Force || ShouldContinue(string.Format(Resources.MoveRecycleBinItemWithLeaf0ToSecondStage, item.LeafName), Resources.Confirm))
                {
                    item.MoveToSecondStage();
                    ClientContext.ExecuteQueryRetry();
                }
            }
            else
            {
                if (Force || ShouldContinue(Resources.MoveFirstStageRecycleBinItemsToSecondStage, Resources.Confirm))
                {
                    ClientContext.Site.RecycleBin.MoveAllToSecondStage();
                    ClientContext.ExecuteQueryRetry();
                }
            }
        }
    }
}
#endif