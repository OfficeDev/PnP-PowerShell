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
    [Cmdlet("Get", "PnPUnifiedGroup")]
    [CmdletHelp("Gets one Office 365 Group (aka Unified Group) or a list of Office 365 Groups",
        Category = CmdletHelpCategory.Graph)]
    [CmdletExample(
       Code = "PS:> Get-PnPUnifiedGroup",
       Remarks = "Retrieves all the Office 365 Groups",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPUnifiedGroup -Identity $groupId",
       Remarks = "Retrieves a specific Office 365 Group based on its ID",
       SortOrder = 2)]
    [CmdletExample(
       Code = "PS:> Get-PnPUnifiedGroup -Identity $groupDisplayName",
       Remarks = "Retrieves a specific Office 365 Group based on its DisplayName",
       SortOrder = 3)]
    [CmdletExample(
       Code = "PS:> Get-PnPUnifiedGroup -Identity $groupSiteUrl",
       Remarks = "Retrieves a specific Office 365 Group based on the URL of its Modern SharePoint site",
       SortOrder = 4)]
    [CmdletExample(
       Code = "PS:> Get-PnPUnifiedGroup -Identity $group",
       Remarks = "Retrieves a specific Office 365 Group based on its object instance",
       SortOrder = 5)]
    public class GetUnifiedGroup : PnPGraphCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The Identity of the Office 365 Group.")]
        public UnifiedGroupPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            UnifiedGroupEntity group = null;
            List<UnifiedGroupEntity> groups = null;

            if (Identity != null)
            {
                // We have to retrieve a specific group
                if (Identity.Group != null)
                {
                    group = UnifiedGroupsUtility.GetUnifiedGroup(Identity.Group.GroupId, AccessToken);
                }
                else if (!String.IsNullOrEmpty(Identity.DisplayName))
                {
                    groups = UnifiedGroupsUtility.ListUnifiedGroups(AccessToken, Identity.DisplayName);
                }
                else if (!String.IsNullOrEmpty(Identity.GroupId))
                {
                    group = UnifiedGroupsUtility.GetUnifiedGroup(Identity.GroupId, AccessToken);
                }
            }
            else
            {
                // Retrieve all the groups
                groups = UnifiedGroupsUtility.ListUnifiedGroups(AccessToken);
            }

            if (group != null)
            {
                WriteObject(group);
            }
            else if (groups != null)
            {
                WriteObject(groups, true);
            }
        }
    }
}
