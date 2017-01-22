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
    [CmdletAlias("Get-SPOView")]
    [CmdletHelp("Returns one or all views from a list",
        Category = CmdletHelpCategory.Lists,
        OutputType = typeof(View),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.view.aspx")]
    [CmdletExample(
        Code = @"Get-PnPView -List ""Demo List""",
        Remarks = @"Returns all views associated from the specified list",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"Get-PnPView -List ""Demo List"" -Identity ""Demo View""",
        Remarks = @"Returns the view called ""Demo View"" from the specified list",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"Get-PnPView -List ""Demo List"" -Identity ""5275148a-6c6c-43d8-999a-d2186989a661""",
        Remarks = @"Returns the view with the specified ID from the specified list",
        SortOrder = 3)]
    public class GetView : PnPWebRetrievalCmdlet<View>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "The ID or name of the view")]
        public ViewPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            AlwaysLoadProperties = new[] { "ViewFields" };

            if (List != null)
            {
                var list = List.GetList(SelectedWeb);
                if (list != null)
                {
                    View view = null;
                    IEnumerable<View> views = null;
                    if (Identity != null)
                    {
                        if (Identity.Id != Guid.Empty)
                        {
                            view = list.GetViewById(Identity.Id);
                            view.EnsureProperties(Expressions);

                        }
                        else if (!string.IsNullOrEmpty(Identity.Title))
                        {
                            view = list.GetViewByName(Identity.Title);
                            view.EnsureProperties(Expressions);
                        }
                    }
                    else
                    {
                        views = ClientContext.LoadQuery(list.Views.IncludeWithDefaultProperties(Expressions));
                        ClientContext.ExecuteQueryRetry();

                    }
                    if (views != null && views.Any())
                    {
                        WriteObject(views, true);
                    }
                    else if (view != null)
                    {
                        WriteObject(view);
                    }
                }
            }

        }
    }

}
