using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Get, "PnPList")]
    [CmdletAlias("Get-SPOList")]
    [CmdletHelp("Returns a List object",
        Category = CmdletHelpCategory.Lists,
        OutputType = typeof(List),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.list.aspx")]
    [CmdletExample(
        Code = "PS:> Get-PnPList",
        Remarks = "Returns all lists in the current web",
        SortOrder = 1)]
    [CmdletExample(
        Code = "PS:> Get-PnPList -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe",
        Remarks = "Returns a list with the given id.",
        SortOrder = 2)]
    [CmdletExample(
        Code = "PS:> Get-PnPList -Identity Lists/Announcements",
        Remarks = "Returns a list with the given url.",
        SortOrder = 3)]
    public class GetList : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, name or Url (Lists/MyList) of the list.")]
        public ListPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (Identity != null)
            {
                var list = Identity.GetList(SelectedWeb);
                WriteObject(list);

            }
            else
            {
                var lists = ClientContext.LoadQuery(SelectedWeb.Lists.IncludeWithDefaultProperties(l => l.Id, l => l.BaseTemplate, l => l.OnQuickLaunch, l => l.DefaultViewUrl, l => l.Title, l => l.Hidden, l => l.RootFolder.ServerRelativeUrl));
                ClientContext.ExecuteQueryRetry();
                WriteObject(lists, true);
            }
        }
    }

}
