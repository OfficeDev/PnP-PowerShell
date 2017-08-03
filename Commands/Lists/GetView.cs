using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Get, "PnPView")]
    [CmdletHelp("Returns one or all views from a list",
        Category = CmdletHelpCategory.Lists,
        OutputType = typeof(View),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.view.aspx")]
    [CmdletExample(
        Code = @"Get-PnPView -List ""Demo List""",
        Remarks = @"Returns all views associated from the list titled ""Demo List""",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"Get-PnPView -List ""Demo List"" -Identity ""Demo View""",
        Remarks = @"Returns the view called ""Demo View"" from the list titled ""Demo List""",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"Get-PnPView -List ""Demo List"" -Identity ""5275148a-6c6c-43d8-999a-d2186989a661""",
        Remarks = @"Returns the view with the Id ""5275148a-6c6c-43d8-999a-d2186989a661"" from the list titled ""Demo List""",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"Get-PnPList -Identity ""Demo List"" | Get-PnPView -Identity ""Demo View""",
        Remarks = @"Returns the view called ""Demo View"" from the list titled ""Demo List""",
        SortOrder = 4)]
    public class GetView : PnPWebRetrievalsCmdlet<View>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The Id, Title or Url of the list")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "The Id, Title or instance of the view")]
        public ViewPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (List != null)
            {
                var list = List.GetList(SelectedWeb);
                if (list == null)
                {
                    throw new PSArgumentException("List provided through the List argument does not exist", "List");
                }

                View view = null;
                IEnumerable<View> views = null;
                if (Identity != null)
                {
                    if (Identity.Id != Guid.Empty)
                    {
                        view = list.GetViewById(Identity.Id);
                    }
                    else if (!string.IsNullOrEmpty(Identity.Title))
                    {
                        view = list.GetViewByName(Identity.Title);
                    }
                    if(view == null)
                    {
                        throw new PSArgumentException("View provided through the Identity argument does not exist", "Identity");
                    }
                }
                else
                {
                    views = ClientContext.LoadQuery(list.Views);
                    ClientContext.ExecuteQueryRetry();
                }
                if (views != null && views.Any())
                {
                    var records = Utilities.PSObjectConverter.ConvertGenericObjects(views, this);

                    // Add a new property indicating the parent list Id of the view so the ViewPipeBind can use that to regenerate the view
                    records.Select(r => { r.Properties.Add(new PSVariableProperty(new PSVariable("List", list.Id))); return r; }).ToList();

                    WriteObject(records, true);
                }
                else if (view != null)
                {
                    var record = Utilities.PSObjectConverter.ConvertGenericObject(view, this);

                    // Add a new property indicating the parent list Id of the view so the ViewPipeBind can use that to regenerate the view
                    record.Properties.Add(new PSVariableProperty(new PSVariable("List", list.Id)));

                    WriteObject(record);
                }
            }
        }
    }
}
