using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Add, "PnPView")]
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
    [CmdletExample(
        Code = @"Add-PnPView -List ""Demo List"" -Title ""Demo View"" -Fields ""Title"",""Address"" -Aggregations ""<FieldRef Name='Title' Type='COUNT'/>""",
        Remarks = @"Adds a view named ""Demo view"" to the ""Demo List"" list and sets the totals (aggregations) to Count on the Title field.",
        SortOrder = 3)]
    public class AddView : PnPWebCmdlet
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

        [Parameter(Mandatory = false, HelpMessage = "A valid XML fragment containing one or more Aggregations")]
        public string Aggregations;

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);
            if (list != null)
            {
                var view = list.CreateView(Title, ViewType, Fields, RowLimit, SetAsDefault, Query, Personal, Paged);

                if(ParameterSpecified(nameof(Aggregations)))
                {
                    view.Aggregations = Aggregations;
                    view.Update();
                    list.Context.Load(view);
                    list.Context.ExecuteQueryRetry();
                }
                WriteObject(view);
            }
        }
    }
}
