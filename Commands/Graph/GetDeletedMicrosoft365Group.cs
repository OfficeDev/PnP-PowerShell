#if !ONPREMISES
using System.Collections.Generic;
using System.Management.Automation;
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Graph;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Get, "PnPDeletedMicrosoft365Group")]
    [Alias("Get-PnPDeletedUnifiedGroup")]
    [CmdletHelp("Gets one deleted Microsoft 365 Group or a list of deleted Microsoft 365 Groups",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = "PS:> Get-PnPDeletedMicrosoft365Group",
        Remarks = "Retrieves all deleted Microsoft 365 Groups",
        SortOrder = 1)]
    [CmdletExample(
        Code = "PS:> Get-PnPDeletedMicrosoft365Group -Identity 38b32e13-e900-4d95-b860-fb52bc07ca7f",
        Remarks = "Retrieves a specific deleted Microsoft 365 Group based on its ID",
        SortOrder = 2)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All | MicrosoftGraphApiPermission.Group_Read_All)]
    public class GetDeletedMicrosoft365Group : PnPGraphCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "The Identity of the Microsoft 365 Group")]
        public Microsoft365GroupPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            UnifiedGroupEntity group = null;
            List<UnifiedGroupEntity> groups = null;

            if (Identity != null)
            {
                group = Identity.GetDeletedGroup(AccessToken);
            }
            else
            {
                groups = UnifiedGroupsUtility.ListDeletedUnifiedGroups(AccessToken);
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
#endif