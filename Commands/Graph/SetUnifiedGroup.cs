using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Graph;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Set, "PnPUnifiedGroup")]
    [CmdletHelp("Sets Office 365 Group (aka Unified Group) properties",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Set-PnPUnifiedGroup -Identity $group -DisplayName ""My Displayname""",
       Remarks = "Sets the display name of the group where $group is a Group entity",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Set-PnPUnifiedGroup -Identity $groupId -Descriptions ""My Description"" -DisplayName ""My DisplayName""",
       Remarks = "Sets the display name and description of a group based upon its ID",
       SortOrder = 2)]
    [CmdletExample(
       Code = @"PS:> Set-PnPUnifiedGroup -Identity $group -GroupLogoPath "".\MyLogo.png""",
       Remarks = "Sets a specific Office 365 Group logo.",
       SortOrder = 3)]
    [CmdletExample(
       Code = @"PS:> Set-PnPUnifiedGroup -Identity $group -IsPrivate:$false",
       Remarks = "Sets a group to be Public if previously Private.",
       SortOrder = 4)]
    [CmdletExample(
       Code = @"PS:> Set-PnPUnifiedGroup -Identity $group -Owners demo@contoso.com",
       Remarks = "Adds demo@contoso.com as an additional owner to the group.",
       SortOrder = 5)]
    public class SetUnifiedGroup : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The Identity of the Office 365 Group.", ValueFromPipeline = true)]
        public UnifiedGroupPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "The DisplayName of the group to set.")]
        public string DisplayName;

        [Parameter(Mandatory = false, HelpMessage = "The Description of the group to set.")]
        public string Description;

        [Parameter(Mandatory = false, HelpMessage = "The array UPN values of owners to add to the group.")]
        public String[] Owners;

        [Parameter(Mandatory = false, HelpMessage = "The array UPN values of members to add to the group.")]
        public String[] Members;

        [Parameter(Mandatory = false, HelpMessage = "Makes the group private when selected.")]
        public SwitchParameter IsPrivate;

        [Parameter(Mandatory = false, HelpMessage = "The path to the logo file of to set.")]
        public string GroupLogoPath;

        protected override void ExecuteCmdlet()
        {
            UnifiedGroupEntity group = null;

            if (Identity != null)
            {
                group = Identity.GetGroup(AccessToken);
            }

            Stream groupLogoStream = null;

            if (group != null)
            {
                if (GroupLogoPath != null)
                {
                    if (!Path.IsPathRooted(GroupLogoPath))
                    {
                        GroupLogoPath = Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, GroupLogoPath);
                    }
                    groupLogoStream = new FileStream(GroupLogoPath, FileMode.Open, FileAccess.Read);
                }

                UnifiedGroupsUtility.UpdateUnifiedGroup(group.GroupId, AccessToken, displayName: DisplayName,
                    description: Description, owners: Owners, members: Members, groupLogo: groupLogoStream, isPrivate: IsPrivate);
            } else
            {
                WriteError(new ErrorRecord(new Exception("Group not found"), "GROUPNOTFOUND", ErrorCategory.ObjectNotFound, this));
            }
        }
    }
}