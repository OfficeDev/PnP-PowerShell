using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Extensions;

namespace SharePointPnP.PowerShell.Commands.RecycleBin
{
    [Cmdlet(VerbsCommon.Get, "PnPRecycleBinItem", DefaultParameterSetName = "All")]
    [CmdletHelp("Returns the items in the recycle bin from the context",
        Category = CmdletHelpCategory.RecycleBin,
        OutputType = typeof(RecycleBinItem),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.recyclebinitem.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPRecycleBinItem",
        Remarks = "Returns all items in both the first and the second stage recycle bins in the current site collection",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPRecycleBinItem -Identity f3ef6195-9400-4121-9d1c-c997fb5b86c2",
        Remarks = "Returns all a specific recycle bin item by id",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPRecycleBinItem -FirstStage",
        Remarks = "Returns all items in only the first stage recycle bin in the current site collection",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-PnPRecycleBinItem -SecondStage",
        Remarks = "Returns all items in only the second stage recycle bin in the current site collection",
        SortOrder = 4)]
    public class GetRecycleBinItems : SPOCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Returns a recycle bin item with a specific identity", ParameterSetName = "Identity")]
        public GuidPipeBind Identity;
        [Parameter(Mandatory = false, HelpMessage = "Return all items in the first stage recycle bin", ParameterSetName = "FirstStage")]
        public SwitchParameter FirstStage;
        [Parameter(Mandatory = false, HelpMessage = "Return all items in the second stage recycle bin", ParameterSetName = "SecondStage")]
        public SwitchParameter SecondStage;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == "Identity")
            {
                var item = ClientContext.Site.RecycleBin.GetById(Identity.Id);
                ClientContext.Load(item);
                ClientContext.ExecuteQueryRetry();
                WriteObject(item);
            }
            else
            {
                ClientContext.Site.EnsureProperty(s => s.RecycleBin);

                var recycleBinItemList = ClientContext.Site.RecycleBin.ToList();

                switch (ParameterSetName)
                {

                    case "FirstStage":
                        WriteObject(
                            recycleBinItemList.Where(i => i.ItemState == RecycleBinItemState.FirstStageRecycleBin), true);
                        break;
                    case "SecondStage":
                        WriteObject(
                            recycleBinItemList.Where(i => i.ItemState == RecycleBinItemState.SecondStageRecycleBin),
                            true);
                        break;
                    default:
                        WriteObject(recycleBinItemList, true);
                        break;
                }
            }
        }
    }
}
