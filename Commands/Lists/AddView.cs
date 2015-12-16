using OfficeDevPnP.PowerShell.Commands.Base.PipeBinds;
using Microsoft.SharePoint.Client;
using System.Management.Automation;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Add, "SPOView")]
    [CmdletHelp("Adds a view to a list",
        Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
        Code = @"Add-SPOView -List ""Demo List"" -Title ""Demo View"" -Fields ""Title"",""Address""",
        SortOrder = 1)]
    public class AddView : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "The title of the view.")]
        public string Title;

        [Parameter(Mandatory = false, HelpMessage = "A valid CAML Query.")]
        public string Query;

        [Parameter(Mandatory = true, HelpMessage = "A list of fields to add.")]
        public string[] Fields;

        [Parameter(Mandatory = false, HelpMessage = "The type of view to add.")]
        public ViewType ViewType = ViewType.None;

        [Parameter(Mandatory = false, HelpMessage = "The row limit for the view. Defaults to 30.")]
        public uint RowLimit = 30;

        [Parameter(Mandatory = false, HelpMessage = "If specified, a personal view will be created.")]
        public SwitchParameter Personal;

        [Parameter(Mandatory = false, HelpMessage = "If specified, the view will be set as the default view for the list.")]
        public SwitchParameter SetAsDefault;
        
        [Parameter(Mandatory = false, HelpMessage = "If specified, the view will have paging.")]
        public SwitchParameter Paged;        

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);
            if (list != null)
            {
                var view = list.CreateView(Title, ViewType, Fields, RowLimit, SetAsDefault, Query, Paged, Personal);

                WriteObject(view);
            }
        }
    }
}
