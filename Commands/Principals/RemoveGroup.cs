using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.Remove, "PnPGroup", DefaultParameterSetName = "All")]
    [CmdletHelp("Removes a group from a web.",
        Category = CmdletHelpCategory.Principals)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPGroup -Identity ""My Users""",
        SortOrder = 1,
        Remarks = @"Removes the group ""My Users""")]
    public class RemoveGroup : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "A group object, an ID or a name of a group to remove")]
        public GroupPipeBind Identity = new GroupPipeBind();

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            Group group = Identity.GetGroup(SelectedWeb);
            if (Force || ShouldContinue(string.Format(Properties.Resources.RemoveGroup0, group.Title), Properties.Resources.Confirm))
            {
                SelectedWeb.SiteGroups.Remove(group);

                ClientContext.ExecuteQueryRetry();
            }
        }
    }



}
