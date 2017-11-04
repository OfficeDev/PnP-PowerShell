using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Graph;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet("Remove", "PnPUnifiedGroup")]
    [CmdletHelp("Removes one Office 365 Group (aka Unified Group) or a list of Office 365 Groups",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Remove-PnPUnifiedGroup -Identity $groupId",
       Remarks = "Removes an Office 365 Groups based on its ID",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Remove-PnPUnifiedGroup -Identity $group",
       Remarks = "Removes the provided Office 365 Groups",
       SortOrder = 2)]
    public class RemoveUnifiedGroup : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The Identity of the Office 365 Group.")]
        public UnifiedGroupPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (Identity != null)
            {
                UnifiedGroupEntity group = null;

                // We have to retrieve a specific group
                if (Identity.Group != null)
                {
                    group = UnifiedGroupsUtility.GetUnifiedGroup(Identity.Group.GroupId, AccessToken, includeSite: false);
                }
                else if (!String.IsNullOrEmpty(Identity.GroupId))
                {
                    group = UnifiedGroupsUtility.GetUnifiedGroup(Identity.GroupId, AccessToken, includeSite: false);
                }

                if (group != null)
                {
                    UnifiedGroupsUtility.DeleteUnifiedGroup(group.GroupId, AccessToken);
                }
            }
        }
    }
}
