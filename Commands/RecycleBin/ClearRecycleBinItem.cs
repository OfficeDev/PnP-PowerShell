using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.RecycleBin
{
    [Cmdlet(VerbsCommon.Clear, "PnPRecycleBinItem", DefaultParameterSetName = PARAMETERSET_ALL)]
    [CmdletHelp("Permanently deletes all or a specific recycle bin item",
        SupportedPlatform = CmdletSupportedPlatform.All,
        Category = CmdletHelpCategory.RecycleBin)]
    [CmdletExample(
        Code = @"PS:> Get-PnPRecycleBinItem | ? FileLeafName -like ""*.docx"" | Clear-PnpRecycleBinItem",
        Remarks = "Permanently deletes all the items in the first and second stage recycle bins of which the file names have the .docx extension",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Clear-PnpRecycleBinItem -Identity 72e4d749-d750-4989-b727-523d6726e442",
        Remarks = "Permanently deletes the recycle bin item with Id 72e4d749-d750-4989-b727-523d6726e442 from the recycle bin",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Clear-PnpRecycleBinItem -Identity $item -Force",
        Remarks = "Permanently deletes the recycle bin item stored under variable $item from the recycle bin without asking for confirmation from the end user first",
        SortOrder = 3)]
#if !SP2013
    [CmdletExample(
        Code = @"PS:> Clear-PnPRecycleBinItem -All -RowLimit 10000",
        Remarks = "Permanently deletes up to 10,000 items in the recycle bin",
        SortOrder = 4)]
#endif

    public class ClearRecycleBinItem : PnPSharePointCmdlet
    {
        const string PARAMETERSET_ALL = "All";
        const string PARAMETERSET_IDENTITY = "Identity";

        [Parameter(Mandatory = true, HelpMessage = "Id of the recycle bin item or the recycle bin item itself to permanently delete", ValueFromPipeline = true, ParameterSetName = PARAMETERSET_IDENTITY)]
        public RecycleBinItemPipeBind Identity;

        [Parameter(Mandatory = false, ParameterSetName = PARAMETERSET_ALL, HelpMessage = "Clears all items")]
        public SwitchParameter All;

#if !ONPREMISES
        [Parameter(Mandatory = false, HelpMessage = "If provided, only all the items in the second stage recycle bin will be cleared", ParameterSetName = PARAMETERSET_ALL)]
        public SwitchParameter SecondStageOnly = false;
#endif
        [Parameter(Mandatory = false, HelpMessage = "If provided, no confirmation will be asked to restore the recycle bin item", ParameterSetName = PARAMETERSET_IDENTITY)]
        [Parameter(Mandatory = false, HelpMessage = "If provided, no confirmation will be asked to restore the recycle bin item", ParameterSetName = PARAMETERSET_ALL)]
        public SwitchParameter Force;

#if !SP2013
        [Parameter(Mandatory = false, HelpMessage = "Limits deletion to specified number of items", ParameterSetName = PARAMETERSET_ALL)]
        public int RowLimit;
#endif

        protected override void ExecuteCmdlet()
        {
            switch (ParameterSetName)
            {
                case PARAMETERSET_IDENTITY:
                    var recycleBinItem = Identity.GetRecycleBinItem(ClientContext.Site);

                    if (Force || ShouldContinue(string.Format(Resources.ClearRecycleBinItem, recycleBinItem.LeafName), Resources.Confirm))
                    {
                        recycleBinItem.DeleteObject();
                        ClientContext.ExecuteQueryRetry();
                    }
                    break;
                case PARAMETERSET_ALL:
#if !ONPREMISES
                    if (ParameterSpecified(nameof(RowLimit)))
                    {
                        if (Force || ShouldContinue(SecondStageOnly ? Resources.ClearSecondStageRecycleBin : Resources.ClearBothRecycleBins, Resources.Confirm))
                        { 
                            RecycleBinItemState recycleBinStage = SecondStageOnly ? RecycleBinItemState.SecondStageRecycleBin : RecycleBinItemState.None;

                            RecycleBinItemCollection items = ClientContext.Site.GetRecycleBinItems(null, RowLimit, false, RecycleBinOrderBy.DeletedDate, recycleBinStage);
                            ClientContext.Load(items);
                            ClientContext.ExecuteQueryRetry();

                            items.DeleteAll();
                            ClientContext.ExecuteQueryRetry();
                        }
                    }
                    else
#endif
                    {
#if !ONPREMISES
                        if (SecondStageOnly)
                        {
                            if (Force || ShouldContinue(Resources.ClearSecondStageRecycleBin, Resources.Confirm))
                            {
                                ClientContext.Site.RecycleBin.DeleteAllSecondStageItems();
                                ClientContext.ExecuteQueryRetry();
                            }
                        }
                        else
                        {
                            if (Force || ShouldContinue(Resources.ClearBothRecycleBins, Resources.Confirm))
                            {
                                ClientContext.Site.RecycleBin.DeleteAll();
                                ClientContext.ExecuteQueryRetry();
                            }
                        }
                    }
                    break;
#else
                        if (Force || ShouldContinue(Resources.ClearBothRecycleBins, Resources.Confirm))
                        {
                            ClientContext.Site.RecycleBin.DeleteAll();
                            ClientContext.ExecuteQueryRetry();
                        }
                    }
                    break;
#endif
            }
        }
    }
}
