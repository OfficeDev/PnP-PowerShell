using OfficeDevPnP.PowerShell.Commands.Base.PipeBinds;
using Microsoft.SharePoint.Client;
using System;
using System.Linq;
using System.Management.Automation;
using System.Collections.Generic;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "SPOView")]
    [CmdletHelp("Returns one or all views from a list",
        Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
        Code = @"Get-SPOView -List ""Demo List""",
        Remarks = @"Returns all views associated from the specified list",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"Get-SPOView -List ""Demo List"" -Identity ""Demo View""",
        Remarks = @"Returns the view called ""Demo View"" from the specified list",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"Get-SPOView -List ""Demo List"" -Identity ""5275148a-6c6c-43d8-999a-d2186989a661""",
        Remarks = @"Returns the view with the specified ID from the specified list",
        SortOrder = 3)]
    public class GetView : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = false)]
        public ViewPipeBind Identity;

        protected override void ExecuteCmdlet()
        {

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
                            view.EnsureProperty(v => v.ViewFields);

                        }
                        else if (!string.IsNullOrEmpty(Identity.Title))
                        {
                            view = list.GetViewByName(Identity.Title);
                            view.EnsureProperty(v => v.ViewFields);
                        }
                    }
                    else
                    {
                        views = ClientContext.LoadQuery(list.Views.IncludeWithDefaultProperties(v => v.ViewFields));
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
