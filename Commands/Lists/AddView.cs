using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Add, "PnPView")]
    [CmdletAlias("Add-SPOView")]
    [CmdletHelp("Adds a view to a list",
        Category = CmdletHelpCategory.Lists,
          OutputType = typeof(View),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.view.aspx")]
    [CmdletExample(
        Code = @"Add-PnPView -List ""Demo List"" -Title ""Demo View"" -Fields ""Title"",""Address""",
        Remarks = @"Adds a view named ""Demo view"" to the ""Demo List"" list.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"Add-PnPView -List ""Demo List"" -Title ""Demo View"" -Fields ""Title"",""Address"" -Paged",
        Remarks = @"Adds a view named ""Demo view"" to the ""Demo List"" list and makes sure there's paging on this view.",        
        SortOrder = 2)]        
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
                var view = list.CreateView(Title, ViewType, Fields, RowLimit, SetAsDefault, Query, Personal, Paged);

                WriteObject(view);
            }
        }
    }
}
