using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.PowerShell.Commands.Enums;
using System;

namespace OfficeDevPnP.PowerShell.Commands.Principals
{
    [Cmdlet("New", "SPOGroup")]
    [CmdletHelp("Adds a user to the build-in Site User Info List and returns a user object",
        Category = CmdletHelpCategory.Principals)]
    [CmdletExample(
        Code = @"PS:> New-SPOUser -LogonName user@company.com",
        SortOrder = 1)]
    public class NewGroup : SPOWebCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Title = string.Empty;

        [Parameter(Mandatory = false)]
        public string Description;

        [Parameter(Mandatory = false)]
        public string Owner;

        [Parameter(Mandatory = false)]
        public SwitchParameter AllowRequestToJoinLeave;

        [Parameter(Mandatory = false)]
        public SwitchParameter AutoAcceptRequestToJoinLeave;

        [Parameter(Mandatory = false)]
        public SwitchParameter AllowMembersEditMembership;

        [Parameter(Mandatory = false)]
        public SwitchParameter OnlyAllowMembersViewMembership;

        [Parameter(Mandatory = false)]
        public string RequestToJoinEmail;

        [Parameter(Mandatory = false)] // Not promoted to use anymore. Use Set-SPOGroup
        [Obsolete("Use Set-SPOGroup")]
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
            if (OnlyAllowMembersViewMembership)
            {
                group.OnlyAllowMembersViewMembership = true;
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
