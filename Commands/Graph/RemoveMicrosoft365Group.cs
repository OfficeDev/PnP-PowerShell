#if !ONPREMISES
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Graph;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Remove, "PnPMicrosoft365Group")]
    [Alias("Remove-PnPUnifiedGroup")]
    [CmdletHelp("Removes one Microsoft 365 Group",
        Category = CmdletHelpCategory.Graph,
        OutputTypeLink = "https://docs.microsoft.com/graph/api/group-delete",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = "PS:> Remove-PnPMicrosoft365Group -Identity $groupId",
       Remarks = "Removes an Microsoft 365 Group based on its ID",
       SortOrder = 1)]
    [CmdletExample(
       Code = "PS:> Remove-PnPMicrosoft365Group -Identity $group",
       Remarks = "Removes the provided Microsoft 365 Group",
       SortOrder = 2)]
    [CmdletExample(
       Code = "PS:> Get-PnPMicrosoft365Group | ? Visibility -eq \"Public\" | Remove-PnPMicrosoft365Group",
       Remarks = "Removes all the public Microsoft 365 Groups",
       SortOrder = 3)]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class RemoveMicrosoft365Group : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The Identity of the Microsoft 365 Group")]
        public Microsoft365GroupPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            if (Identity != null)
            {
                UnifiedGroupEntity group = Identity.GetGroup(AccessToken);
                
                if (group != null)
                {
                    UnifiedGroupsUtility.DeleteUnifiedGroup(group.GroupId, AccessToken);
                }
            }
        }
    }
}
#endif