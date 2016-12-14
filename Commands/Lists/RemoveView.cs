using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Remove, "PnPView", SupportsShouldProcess = true)]
    [CmdletAlias("Remove-SPOView")]
    [CmdletHelp("Deletes a view from a list",
        Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPView -List ""Demo List"" -Identity ""All Items""",
        SortOrder = 1,
        Remarks = @"Removes the view with title ""All Items"" from the ""Demo List"" list.")]
    public class RemoveView : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID or Title of the view.")]
        public ViewPipeBind Identity = new ViewPipeBind();

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 1, HelpMessage = "The ID or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (List != null)
            {
                var list = List.GetList(SelectedWeb);

                if (list != null)
                {
                    View view = null;
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
                        else if (Identity.View != null)
                        {
                            view = Identity.View;
                        }
                        if (view != null)
                        {
                            if (Force || ShouldContinue(string.Format(Properties.Resources.RemoveView0, view.Title), Properties.Resources.Confirm))
                            {
                                view.DeleteObject();
                                ClientContext.ExecuteQueryRetry();
                            }
                        }
                    }
                }
            }
        }
    }

}
