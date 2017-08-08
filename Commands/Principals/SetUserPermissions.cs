using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;

namespace SharePointPnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.Set, "PnPUserPermissions")]
    [CmdletHelp("Adds and/or removes permissions of a specific SharePoint user",
        Category = CmdletHelpCategory.Principals)]
    [CmdletExample(
        Code = @"PS:> Set-PnPUserPermissions -Identity 'Everyone' -AddRole Contribute",
        Remarks = "Adds the 'Contribute' permission to the SharePoint user with the login 'Everyone'",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPUserPermissions -Identity 'Everyone' -RemoveRole 'Full Control' -AddRole 'Read'",
        Remarks = "Removes the 'Full Control' from and adds the 'Contribute' permissions to the SharePoint user with the login 'Everyone'",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Set-PnPUserPermissions -Identity 'Everyone' -AddRole @('Contribute', 'Design')",
        Remarks = "Adds the 'Contribute' and 'Design' permissions to the SharePoint user with the login 'Everyone'",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Set-PnPUserPermissions -Identity 'Everyone' -RemoveRole @('Contribute', 'Design')",
        Remarks = "Removes the 'Contribute' and 'Design' permissions from the SharePoint user with the login 'Everyone'",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Set-PnPUserPermissions -Identity 'Everyone' -List 'MyList' -RemoveRole @('Contribute')",
        Remarks = "Removes the 'Contribute' permissions from the list 'MyList' for the user with the login 'Everyone'",
        SortOrder = 5)]
    public class SetUserPermissions : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "ByName", HelpMessage = "Get a specific user by login")]
        [Alias("Name")]
        public UserPipeBind Identity = new UserPipeBind();

        [Parameter(Mandatory = false, HelpMessage = "The list to apply the command to.")]
        public ListPipeBind List = new ListPipeBind();

        [Parameter(Mandatory = false, HelpMessage = "Name of the permission set to add to this SharePoint group")]
        public string[] AddRole = null;

        [Parameter(Mandatory = false, HelpMessage = "Name of the permission set to remove from this SharePoint group")]
        public string[] RemoveRole = null;

        protected override void ExecuteCmdlet()
        {
            var user = Identity.GetUser(SelectedWeb);

            List list = List.GetList(SelectedWeb);
            if (list == null && !string.IsNullOrEmpty(List.Title))
            {
                throw new Exception($"List with Title {List.Title} not found");
            }
            else if (list == null && List.Id != Guid.Empty)
            {
                throw new Exception($"List with Id {List.Id} not found");
            }

            if (AddRole != null)
            {
                foreach (var role in AddRole)
                {
                    var roleDefinition = SelectedWeb.RoleDefinitions.GetByName(role);
                    var roleDefinitionBindings = new RoleDefinitionBindingCollection(ClientContext) { roleDefinition };

                    RoleAssignmentCollection roleAssignments;
                    if (list != null)
                    {
                        roleAssignments = list.RoleAssignments;
                    }
                    else
                    {
                        roleAssignments = SelectedWeb.RoleAssignments;
                    }

                    roleAssignments.Add(user, roleDefinitionBindings);
                    ClientContext.Load(roleAssignments);
                    ClientContext.ExecuteQueryRetry();
                }
            }
            if (RemoveRole != null)
            {
                foreach (var role in RemoveRole)
                {
                    RoleAssignment roleAssignment;
                    if (list != null)
                    {
                        roleAssignment = list.RoleAssignments.GetByPrincipal(user);
                    }
                    else
                    {
                        roleAssignment = SelectedWeb.RoleAssignments.GetByPrincipal(user);
                    }
                    var roleDefinitionBindings = roleAssignment.RoleDefinitionBindings;
                    ClientContext.Load(roleDefinitionBindings);
                    ClientContext.ExecuteQueryRetry();
                    foreach (var roleDefinition in roleDefinitionBindings.Where(roleDefinition => roleDefinition.Name == role))
                    {
                        roleDefinitionBindings.Remove(roleDefinition);
                        roleAssignment.Update();
                        ClientContext.ExecuteQueryRetry();
                        break;
                    }
                }
            }
        }
    }
}
