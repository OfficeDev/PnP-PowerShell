using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Set, "PnPListItemPermission", DefaultParameterSetName = "User")]
    [CmdletHelp("Sets list item permissions. Use Get-PnPRoleDefinition to retrieve all available roles you can add or remove using this cmdlet.",
        Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
        Code = "PS:> Set-PnPListItemPermission -List 'Documents' -Identity 1 -User 'user@contoso.com' -AddRole 'Contribute'",
        Remarks = "Adds the 'Contribute' permission to the user 'user@contoso.com' for listitem with id 1 in the list 'Documents'",
        SortOrder = 1)]
    [CmdletExample(
        Code = "PS:> Set-PnPListItemPermission -List 'Documents' -Identity 1 -User 'user@contoso.com' -RemoveRole 'Contribute'",
        Remarks = "Removes the 'Contribute' permission to the user 'user@contoso.com' for listitem with id 1 in the list 'Documents'",
        SortOrder = 2)]
    [CmdletExample(
        Code = "PS:> Set-PnPListItemPermission -List 'Documents' -Identity 1 -User 'user@contoso.com' -AddRole 'Contribute' -ClearExisting",
        Remarks = "Adds the 'Contribute' permission to the user 'user@contoso.com' for listitem with id 1 in the list 'Documents' and removes all other permissions",
        SortOrder = 3)]
    [CmdletExample(
        Code = "PS:> Set-PnPListItemPermission -List 'Documents' -Identity 1 -InheritPermissions",
        Remarks = "Resets permissions for listitem with id 1 to inherit permissions from the list 'Documents'",
        SortOrder = 4)]
    public class SetListItemPermission : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Title or Url of the list.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public ListPipeBind List;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The ID of the listitem, or actual ListItem object", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public ListItemPipeBind Identity;

        [Parameter(Mandatory = true, ParameterSetName = "Group")]
        public GroupPipeBind Group;

        [Parameter(Mandatory = true, ParameterSetName = "User")]
        public string User;

        [Parameter(Mandatory = false, HelpMessage = "The role that must be assigned to the group or user", ParameterSetName = "User")]
        [Parameter(Mandatory = false, HelpMessage = "The role that must be assigned to the group or user", ParameterSetName = "Group")]
        public string AddRole = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "The role that must be removed from the group or user", ParameterSetName = "User")]
        [Parameter(Mandatory = false, HelpMessage = "The role that must be removed from the group or user", ParameterSetName = "Group")]
        public string RemoveRole = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Clear all existing permissions", ParameterSetName = "User")]
        [Parameter(Mandatory = false, HelpMessage = "Clear all existing permissions", ParameterSetName = "Group")]
        public SwitchParameter ClearExisting;

        [Parameter(Mandatory = false, HelpMessage = "Inherit permissions from the list, removing unique permissions", ParameterSetName = "Inherit")]
        public SwitchParameter InheritPermissions;

#if !ONPREMISES
        [Parameter(Mandatory = false, HelpMessage = "Update the item permissions without creating a new version or triggering MS Flow.")]
        public SwitchParameter SystemUpdate;
#endif

        protected override void ExecuteCmdlet()
        {
            List list = null;
            if (List != null)
            {
                list = List.GetList(SelectedWeb);
            }
            if (list != null)
            {
                var item = Identity.GetListItem(list);
                if (item != null)
                {
                    item.EnsureProperties(i => i.HasUniqueRoleAssignments);
                    if (item.HasUniqueRoleAssignments && InheritPermissions.IsPresent)
                    {
                        item.ResetRoleInheritance();
                    }
                    else if (!item.HasUniqueRoleAssignments)
                    {
                        item.BreakRoleInheritance(!ClearExisting.IsPresent, true);
                    }
                    else if (ClearExisting.IsPresent)
                    {
                        item.ResetRoleInheritance();
                        item.BreakRoleInheritance(!ClearExisting.IsPresent, true);
                    }

#if !ONPREMISES
                    if (SystemUpdate.IsPresent)
                    {
                        item.SystemUpdate();
                    }
                    else
                    {
                        item.Update();
                    }
#else
                    item.Update();
#endif
                    ClientContext.ExecuteQueryRetry();
                    if (ParameterSetName == "Inherit")
                    {
                        // no processing of user/group needed
                        return;
                    }

                    Principal principal = null;
                    if (ParameterSetName == "Group")
                    {
                        if (Group.Id != -1)
                        {
                            principal = SelectedWeb.SiteGroups.GetById(Group.Id);
                        }
                        else if (!string.IsNullOrEmpty(Group.Name))
                        {
                            principal = SelectedWeb.SiteGroups.GetByName(Group.Name);
                        }
                        else if (Group.Group != null)
                        {
                            principal = Group.Group;
                        }
                    }
                    else
                    {
                        principal = SelectedWeb.EnsureUser(User);
                        ClientContext.ExecuteQueryRetry();
                    }
                    if (principal != null)
                    {
                        if (!string.IsNullOrEmpty(AddRole))
                        {
                            var roleDefinition = SelectedWeb.RoleDefinitions.GetByName(AddRole);
                            var roleDefinitionBindings = new RoleDefinitionBindingCollection(ClientContext)
                            {
                                roleDefinition
                            };
                            var roleAssignments = item.RoleAssignments;
                            roleAssignments.Add(principal, roleDefinitionBindings);
                            ClientContext.Load(roleAssignments);
                            ClientContext.ExecuteQueryRetry();
                        }
                        if (!string.IsNullOrEmpty(RemoveRole))
                        {
                            var roleAssignment = item.RoleAssignments.GetByPrincipal(principal);
                            var roleDefinitionBindings = roleAssignment.RoleDefinitionBindings;
                            ClientContext.Load(roleDefinitionBindings);
                            ClientContext.ExecuteQueryRetry();
                            foreach (var roleDefinition in roleDefinitionBindings.Where(roleDefinition => roleDefinition.Name == RemoveRole))
                            {
                                roleDefinitionBindings.Remove(roleDefinition);
                                roleAssignment.Update();
                                ClientContext.ExecuteQueryRetry();
                                break;
                            }
                        }
                    }
                    else
                    {
                        WriteError(new ErrorRecord(new Exception("Principal not found"), "1", ErrorCategory.ObjectNotFound, null));
                    }
                }
            }
        }
    }
}