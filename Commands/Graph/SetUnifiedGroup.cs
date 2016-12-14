using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Graph;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Set, "PnPUnifiedGroup")]
    [CmdletHelp("Sets Office 365 Group (aka Unified Group) properties",
        Category = CmdletHelpCategory.Graph)]
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
    public class SetUnifiedGroup : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The Identity of the Office 365 Group.", ValueFromPipeline = true)]
        public UnifiedGroupPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "The DisplayName of the group to set.")]
        public string DisplayName;

        [Parameter(Mandatory = false, HelpMessage = "The Description of the group to set.")]
        public string Description;

        [Parameter(Mandatory = false, HelpMessage = "The path to the logo file of to set.")]
        public string GroupLogoPath;

        protected override void ExecuteCmdlet()
        {
            UnifiedGroupEntity group = null;

            if (Identity != null)
            {
                // We have to retrieve a specific group
                if (Identity.Group != null)
                {
                    group = UnifiedGroupsUtility.GetUnifiedGroup(Identity.Group.GroupId, AccessToken);
                }
                else if (!String.IsNullOrEmpty(Identity.GroupId))
                {
                    group = UnifiedGroupsUtility.GetUnifiedGroup(Identity.GroupId, AccessToken);
                }
            }

            Stream groupLogoStream = null;

            if (group != null)
            {
                if (GroupLogoPath != null)
                {
                    if (!System.IO.Path.IsPathRooted(GroupLogoPath))
                    {
                        GroupLogoPath = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, GroupLogoPath);
                    }
                    groupLogoStream = new FileStream(GroupLogoPath, FileMode.Open, FileAccess.Read);
                }
                UnifiedGroupsUtility.UpdateUnifiedGroup(group.GroupId, AccessToken, displayName: DisplayName,
                    description: Description, groupLogo: groupLogoStream);
            }
        }
    }
}