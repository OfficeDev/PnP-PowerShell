#if !ONPREMISES
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Graph;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Get, "PnPUnifiedGroupOwners")]
    [CmdletHelp("Gets owners of a paricular Office 365 Group (aka Unified Group)",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Get-PnPUnifiedGroupOwners -Identity $groupId",
       Remarks = "Retrieves all the owners of a specific Office 365 Group based on its ID",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Get-PnPUnifiedGroupOwners -Identity $group",
       Remarks = "Retrieves all the owners of a specific Office 365 Group based on the group's object instance",
       SortOrder = 2)]
    public class GetUnifiedGroupOwners : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The Identity of the Office 365 Group.")]
        public UnifiedGroupPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            UnifiedGroupEntity group = null;

            if (Identity != null)
            {
                group = Identity.GetGroup(AccessToken);
            }

            if (group != null)
            {
                // Get Owners of the group.

                List<UnifiedGroupUser> owners = UnifiedGroupsUtility.GetUnifiedGroupOwners(group, AccessToken);
                WriteObject(owners);
            }

        }
    }
}
#endif