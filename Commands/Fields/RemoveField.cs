using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Fields
{
    [Cmdlet(VerbsCommon.Remove, "PnPField", SupportsShouldProcess = true)]
    [CmdletHelp("Removes a field from a list or a site",
        Category = CmdletHelpCategory.Fields)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPField -Identity ""Speakers""",
        Remarks = @"Removes the speakers field from the site columns",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPField -List ""Demo list"" -Identity ""Speakers""",
        Remarks = @"Removes the speakers field from the list Demo list",
        SortOrder = 1)]
    public class RemoveField : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The field object or name to remove")]
        public FieldPipeBind Identity = new FieldPipeBind();

        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 1, HelpMessage = "The list object or name where to remove the field from")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (List != null)
            {
                var list = List.GetList(SelectedWeb);

                var f = Identity.Field;
                if (list != null)
                {
                    if (f == null)
                    {
                        if (Identity.Id != Guid.Empty)
                        {
                            f = list.Fields.GetById(Identity.Id);
                        }
                        else if (!string.IsNullOrEmpty(Identity.Name))
                        {
                            f = list.Fields.GetByInternalNameOrTitle(Identity.Name);
                        }
                    }
                    ClientContext.Load(f);
                    ClientContext.ExecuteQueryRetry();
                    if (f != null && f.IsPropertyAvailable("InternalName"))
                    {
                        if (Force || ShouldContinue(string.Format(Properties.Resources.DeleteField0, f.InternalName), Properties.Resources.Confirm))
                        {
                            f.DeleteObject();
                            ClientContext.ExecuteQueryRetry();
                        }
                    }
                }
            } 
            else
            {
                var f = Identity.Field;

                if (f == null)
                {
                    if (Identity.Id != Guid.Empty)
                    {
                        f = SelectedWeb.Fields.GetById(Identity.Id);
                    }
                    else if (!string.IsNullOrEmpty(Identity.Name))
                    {
                        f = SelectedWeb.Fields.GetByInternalNameOrTitle(Identity.Name);
                    }
                }
                ClientContext.Load(f);
                ClientContext.ExecuteQueryRetry();

                if (f != null && f.IsPropertyAvailable("InternalName"))
                {
                    if (Force || ShouldContinue(string.Format(Properties.Resources.DeleteField0, f.InternalName), Properties.Resources.Confirm))
                    {
                        f.DeleteObject();
                        ClientContext.ExecuteQueryRetry();
                    }
                }
            }
        }
    }

}
