using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Enums;

namespace SharePointPnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.Set, "PnPGroup")]
    [CmdletHelp("Updates a group",
        Category = CmdletHelpCategory.Principals)]
    [CmdletExample(
        Code = @"PS:> Set-PnPGroup -Identity 'My Site Members' -SetAssociatedGroup Members",
        Remarks = "Sets the SharePoint group with the name 'My Site Members' as the associated members group",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPGroup -Identity 'My Site Members' -Owner 'site owners'",
        Remarks = "Sets the SharePoint group with the name 'site owners' as the owner of the SharePoint group with the name 'My Site Members'",
        SortOrder = 2)]
    public class SetGroup : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "A group object, an ID or a name of a group")]
        public GroupPipeBind Identity = new GroupPipeBind();

        [Parameter(Mandatory = false, HelpMessage = "One of the associated group types (Visitors, Members, Owners")]
        public AssociatedGroupType SetAssociatedGroup = AssociatedGroupType.None;

        [Parameter(Mandatory = false, HelpMessage = "Name of the permission set to add to this SharePoint group")]
        public string AddRole = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Name of the permission set to remove from this SharePoint group")]
        public string RemoveRole = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "The title for the group")]
        public string Title = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "The owner for the group, which can be a user or another group")]
        public string Owner;

        [Parameter(Mandatory = false, HelpMessage = "The description for the group")]
        public string Description;

        [Parameter(Mandatory = false, HelpMessage = "A switch parameter that specifies whether to allow users to request membership in the group and to allow users to request to leave the group")]
        public bool AllowRequestToJoinLeave;

        [Parameter(Mandatory = false, HelpMessage = "A switch parameter that specifies whether users are automatically added or removed when they make a request")]
        public bool AutoAcceptRequestToJoinLeave;

        [Parameter(Mandatory = false, HelpMessage = "A switch parameter that specifies whether group members can modify membership in the group")]
        public bool AllowMembersEditMembership;

        [Parameter(Mandatory = false, HelpMessage = "A switch parameter that specifies whether only group members are allowed to view the list of members in the group")]
        public bool OnlyAllowMembersViewMembership;

        [Parameter(Mandatory = false, HelpMessage = "The e-mail address to which membership requests are sent")]
        public string RequestToJoinEmail;

        protected override void ExecuteCmdlet()
        {
            var group = Identity.GetGroup(SelectedWeb);

            ClientContext.Load(group, 
                g => g.AllowMembersEditMembership, 
                g => g.AllowRequestToJoinLeave, 
                g => g.AutoAcceptRequestToJoinLeave,
                g => g.OnlyAllowMembersViewMembership,
                g => g.RequestToJoinLeaveEmailSetting);
            ClientContext.ExecuteQueryRetry();
            
            if (SetAssociatedGroup != AssociatedGroupType.None)
            {
                switch (SetAssociatedGroup)
                {
                    case AssociatedGroupType.Visitors:
                        {
                            SelectedWeb.AssociateDefaultGroups(null, null, group);
                            break;
                        }
                    case AssociatedGroupType.Members:
                        {
                            SelectedWeb.AssociateDefaultGroups(null, group, null);
                            break;
                        }
                    case AssociatedGroupType.Owners:
                        {
                            SelectedWeb.AssociateDefaultGroups(group, null, null);
                            break;
                        }
                }
            }
            if(!string.IsNullOrEmpty(AddRole))
            {
                var roleDefinition = SelectedWeb.RoleDefinitions.GetByName(AddRole);
                var roleDefinitionBindings = new RoleDefinitionBindingCollection(ClientContext);
                roleDefinitionBindings.Add(roleDefinition);
                var roleAssignments = SelectedWeb.RoleAssignments;
                roleAssignments.Add(group,roleDefinitionBindings);
                ClientContext.Load(roleAssignments);
                ClientContext.ExecuteQueryRetry();
            }
            if(!string.IsNullOrEmpty(RemoveRole))
            {
                var roleAssignment = SelectedWeb.RoleAssignments.GetByPrincipal(group);
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

            var dirty = false;
            if (!string.IsNullOrEmpty(Title))
            {
                group.Title = Title;
                dirty = true;
            }
            if (!string.IsNullOrEmpty(Description))
            {
                group.Description = Description;
                dirty = true;
            }
            if (MyInvocation.BoundParameters.ContainsKey("AllowRequestToJoinLeave") && AllowRequestToJoinLeave != group.AllowRequestToJoinLeave)
            {
                group.AllowRequestToJoinLeave = AllowRequestToJoinLeave;
                dirty = true;
            } 

            if (MyInvocation.BoundParameters.ContainsKey("AutoAcceptRequestToJoinLeave") && AutoAcceptRequestToJoinLeave != group.AutoAcceptRequestToJoinLeave)
            {
                group.AutoAcceptRequestToJoinLeave = AutoAcceptRequestToJoinLeave;
                dirty = true;
            }
            if (MyInvocation.BoundParameters.ContainsKey("AllowMembersEditMembership") && AllowMembersEditMembership != group.AllowMembersEditMembership)
            {
                group.AllowMembersEditMembership = AllowMembersEditMembership;
                dirty = true;
            }
            if (MyInvocation.BoundParameters.ContainsKey("OnlyAllowMembersViewMembership") && OnlyAllowMembersViewMembership != group.OnlyAllowMembersViewMembership)
            {
                group.OnlyAllowMembersViewMembership = OnlyAllowMembersViewMembership;
                dirty = true;
            }
            if (RequestToJoinEmail != group.RequestToJoinLeaveEmailSetting)
            {
                group.RequestToJoinLeaveEmailSetting = RequestToJoinEmail;
                dirty = true;
            }
            if(dirty)
            {
                group.Update();
                ClientContext.ExecuteQueryRetry();
            }


            if (!string.IsNullOrEmpty(Owner))
            {
                Principal groupOwner;

                try
                {
                    groupOwner = SelectedWeb.EnsureUser(Owner);
                    group.Owner = groupOwner;
                    group.Update();
                    ClientContext.ExecuteQueryRetry();
                }
                catch
                {
                    groupOwner = SelectedWeb.SiteGroups.GetByName(Owner);
                    group.Owner = groupOwner;
                    group.Update();
                    ClientContext.ExecuteQueryRetry();
                }
            }
            
        }
    }
}
