using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Enums;
using System;

namespace PnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.New, "PnPGroup")]
    [CmdletHelp("Adds group to the Site Groups List and returns a group object",
        Category = CmdletHelpCategory.Principals,
        OutputType = typeof(Group),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.group.aspx")]
    [CmdletExample(
        Code = @"PS:> New-PnPGroup -Title ""My Site Users""",
        SortOrder = 1)]
    public class NewGroup : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The Title of the group")]
        public string Title = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "The description for the group")]
        public string Description;

        [Parameter(Mandatory = false, HelpMessage = "The owner for the group, which can be a user or another group")]
        public string Owner;

        [Parameter(Mandatory = false, HelpMessage = "A switch parameter that specifies whether to allow users to request membership in the group and to allow users to request to leave the group")]
        public SwitchParameter AllowRequestToJoinLeave;

        [Parameter(Mandatory = false, HelpMessage = "A switch parameter that specifies whether users are automatically added or removed when they make a request")]
        public SwitchParameter AutoAcceptRequestToJoinLeave;

        [Parameter(Mandatory = false, HelpMessage = "A switch parameter that specifies whether group members can modify membership in the group")]
        public SwitchParameter AllowMembersEditMembership;

        [Parameter(Mandatory = false, HelpMessage = "A switch parameter that specifies whether only group members are allowed to view the list of members in the group")]
        [Obsolete("This is done by default. Use DisallowMembersViewMembership to disallow group members viewing membership")]
        public SwitchParameter OnlyAllowMembersViewMembership;

        [Parameter(Mandatory = false, HelpMessage = "A switch parameter that disallows group members to view membership.")]
        public SwitchParameter DisallowMembersViewMembership;

        [Parameter(Mandatory = false, HelpMessage = "The e-mail address to which membership requests are sent")]
        public string RequestToJoinEmail;

        [Parameter(Mandatory = false)] // Not promoted to use anymore. Use Set-PnPGroup
        [Obsolete("Use Set-PnPGroup.")]
        public AssociatedGroupType SetAssociatedGroup = AssociatedGroupType.None;

        protected override void ExecuteCmdlet()
        {
            var web = SelectedWeb;

            var groupCI = new GroupCreationInformation { Title = Title, Description = Description };

            var group = web.SiteGroups.Add(groupCI);

            ClientContext.Load(group);
            ClientContext.Load(group.Users);
            ClientContext.ExecuteQueryRetry();
            var dirty = false;
            if (AllowRequestToJoinLeave)
            {
                group.AllowRequestToJoinLeave = true;
                dirty = true;
            }

            if (AutoAcceptRequestToJoinLeave)
            {
                group.AutoAcceptRequestToJoinLeave = true;
                dirty = true;
            }
            if (AllowMembersEditMembership)
            {
                group.AllowMembersEditMembership = true;
                dirty = true;
            }
#pragma warning disable 618
            if (OnlyAllowMembersViewMembership)
#pragma warning restore 618
            {
                group.OnlyAllowMembersViewMembership = true;
                dirty = true;
            }
            if (DisallowMembersViewMembership)
            {
                group.OnlyAllowMembersViewMembership = false;
                dirty = true;
            }
            if (!string.IsNullOrEmpty(RequestToJoinEmail))
            {
                group.RequestToJoinLeaveEmailSetting = RequestToJoinEmail;
                dirty = true;
            }

            if (dirty)
            {
                group.Update();
                ClientContext.ExecuteQueryRetry();
            }

            if (!string.IsNullOrEmpty(Owner))
            {
                Principal groupOwner;

                try
                {
                    groupOwner = web.EnsureUser(Owner);
                    group.Owner = groupOwner;
                    group.Update();
                    ClientContext.ExecuteQueryRetry();
                }
                catch
                {
                    groupOwner = web.SiteGroups.GetByName(Owner);
                    group.Owner = groupOwner;
                    group.Update();
                    ClientContext.ExecuteQueryRetry();
                }
            }


#pragma warning disable CS0618 // Type or member is obsolete
            if (SetAssociatedGroup != AssociatedGroupType.None)

            {
                switch (SetAssociatedGroup)
                {
                    case AssociatedGroupType.Visitors:
                        {
                            web.AssociateDefaultGroups(null, null, group);
                            break;
                        }
                    case AssociatedGroupType.Members:
                        {
                            web.AssociateDefaultGroups(null, group, null);
                            break;
                        }
                    case AssociatedGroupType.Owners:
                        {
                            web.AssociateDefaultGroups(group, null, null);
                            break;
                        }
                }
            }
#pragma warning restore CS0618 // Type or member is obsolete

            ClientContext.ExecuteQueryRetry();
            WriteObject(group);
        }
    }
}
