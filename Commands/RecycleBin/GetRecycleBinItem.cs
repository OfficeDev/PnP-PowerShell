using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.RecycleBin
{
    [Cmdlet(VerbsCommon.Get, "PnPRecycleBinItem", DefaultParameterSetName = ParameterSet_ALL)]
    [CmdletHelp("Returns the items in the recycle bin from the context",
        Category = CmdletHelpCategory.RecycleBin,
        OutputType = typeof(RecycleBinItem),
        SupportedPlatform = CmdletSupportedPlatform.All,
        OutputTypeLink = "https://docs.microsoft.com/previous-versions/office/sharepoint-server/ee541897(v=office.15)")]
    [CmdletExample(
        Code = @"PS:> Get-PnPRecycleBinItem",
        Remarks = "Returns all items in both the first and the second stage recycle bins in the current site collection",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPRecycleBinItem -Identity f3ef6195-9400-4121-9d1c-c997fb5b86c2",
        Remarks = "Returns all a specific recycle bin item by id",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPRecycleBinItem -FirstStage",
        Remarks = "Returns all items in only the first stage recycle bin in the current site collection",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-PnPRecycleBinItem -SecondStage",
        Remarks = "Returns all items in only the second stage recycle bin in the current site collection",
        SortOrder = 4)]
#if !SP2013
    [CmdletExample(
        Code = @"PS:> Get-PnPRecycleBinItem -RowLimit 10000",
        Remarks = "Returns items in recycle bin limited by number of results",
        SortOrder = 5)]
#endif
    public class GetRecycleBinItems : PnPRetrievalsCmdlet<RecycleBinItem>
    {
        private const string ParameterSet_ALL = "All";
        private const string ParameterSet_IDENTITY = "Identity";
        private const string ParameterSet_FIRSTSTAGE = "FirstStage";
        private const string ParameterSet_SECONDSTAGE = "SecondStage";

        [Parameter(Mandatory = false, HelpMessage = "Returns a recycle bin item with a specific identity", ParameterSetName = ParameterSet_IDENTITY)]
        public GuidPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Return all items in the first stage recycle bin", ParameterSetName = ParameterSet_FIRSTSTAGE)]
        public SwitchParameter FirstStage;

        [Parameter(Mandatory = false, HelpMessage = "Return all items in the second stage recycle bin", ParameterSetName = ParameterSet_SECONDSTAGE)]
        public SwitchParameter SecondStage;

#if !SP2013
        [Parameter(Mandatory = false, HelpMessage = "Limits return results to specified amount", ParameterSetName = ParameterSet_FIRSTSTAGE)]
        [Parameter(Mandatory = false, HelpMessage = "Limits return results to specified amount", ParameterSetName = ParameterSet_SECONDSTAGE)]
        [Parameter(Mandatory = false, HelpMessage = "Limits return results to specified amount", ParameterSetName = ParameterSet_ALL)]
        public int RowLimit;
#endif
        protected override void ExecuteCmdlet()
        {
            DefaultRetrievalExpressions = new Expression<Func<RecycleBinItem, object>>[] {r => r.Id, r => r.Title, r => r.ItemType, r => r.LeafName, r => r.DirName};
            if (ParameterSetName == ParameterSet_IDENTITY)
            {
                RecycleBinItem item = ClientContext.Site.RecycleBin.GetById(Identity.Id);
                
                ClientContext.Load(item, RetrievalExpressions);
                ClientContext.ExecuteQueryRetry();
                WriteObject(item);
            }
            else
            {
#if !SP2013
                if (ParameterSpecified(nameof(RowLimit)))
                {
                    RecycleBinItemState recycleBinStage;
                    switch (ParameterSetName)
                    {
                        case ParameterSet_FIRSTSTAGE:
                            recycleBinStage = RecycleBinItemState.FirstStageRecycleBin;
                            break;
                        case ParameterSet_SECONDSTAGE:
                            recycleBinStage = RecycleBinItemState.SecondStageRecycleBin;
                            break;
                        default:
                            recycleBinStage = RecycleBinItemState.None;
                            break;
                    }

                    RecycleBinItemCollection items = ClientContext.Site.GetRecycleBinItems(null, RowLimit, false, RecycleBinOrderBy.DeletedDate, recycleBinStage);
                    ClientContext.Load(items);
                    ClientContext.ExecuteQueryRetry();

                    List<RecycleBinItem> recycleBinItemList = items.ToList();
                    WriteObject(recycleBinItemList, true);
                }
                else
                {
                    ClientContext.Site.Context.Load(ClientContext.Site.RecycleBin, r => r.IncludeWithDefaultProperties(RetrievalExpressions));
                    ClientContext.Site.Context.ExecuteQueryRetry();

                    List<RecycleBinItem> recycleBinItemList = ClientContext.Site.RecycleBin.ToList();

                    switch (ParameterSetName)
                    {
                        case ParameterSet_FIRSTSTAGE:
                            WriteObject(
                                recycleBinItemList.Where(i => i.ItemState == RecycleBinItemState.FirstStageRecycleBin), true);
                            break;
                        case ParameterSet_SECONDSTAGE:
                            WriteObject(
                                recycleBinItemList.Where(i => i.ItemState == RecycleBinItemState.SecondStageRecycleBin),
                                true);
                            break;
                        default:
                            WriteObject(recycleBinItemList, true);
                            break;
                    }
                }
#else
                ClientContext.Site.Context.Load(ClientContext.Site.RecycleBin, r => r.IncludeWithDefaultProperties(RetrievalExpressions));
                ClientContext.Site.Context.ExecuteQueryRetry();
                
                List<RecycleBinItem> recycleBinItemList = ClientContext.Site.RecycleBin.ToList();

                switch (ParameterSetName)
                {

                    case ParameterSet_FIRSTSTAGE:
                        WriteObject(
                            recycleBinItemList.Where(i => i.ItemState == RecycleBinItemState.FirstStageRecycleBin), true);
                        break;
                    case ParameterSet_SECONDSTAGE:
                        WriteObject(
                            recycleBinItemList.Where(i => i.ItemState == RecycleBinItemState.SecondStageRecycleBin),
                            true);
                        break;
                    default:
                        WriteObject(recycleBinItemList, true);
                        break;
                }
#endif
            }
        }
    }
}
