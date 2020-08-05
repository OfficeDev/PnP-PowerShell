using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.Get, "PnPGroupPermissions")]
    [CmdletHelp("Returns the permissions for a specific SharePoint group",
        Category = CmdletHelpCategory.Principals,
        OutputType = typeof(RoleDefinitionBindingCollection),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.roledefinitionbindingcollection.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPGroupPermissions -Identity 'My Site Members'", 
        Remarks = "Returns the permissions for the SharePoint group with the name 'My Site Members'",
        SortOrder = 0)]
    public class GetGroupPermissions : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "ByName", HelpMessage = "Get the permissions of a specific group by name")]
        [Alias("Name")]
        public GroupPipeBind Identity = new GroupPipeBind();

        protected override void ExecuteCmdlet()
        {
            var group = Identity.GetGroup(SelectedWeb);
            var roleAssignment = SelectedWeb.RoleAssignments.GetByPrincipal(group);
            var roleDefinitionBindings = roleAssignment.RoleDefinitionBindings;
            ClientContext.Load(roleDefinitionBindings);
            ClientContext.ExecuteQueryRetry();

            WriteObject(roleDefinitionBindings, true);
        }
    }
}
