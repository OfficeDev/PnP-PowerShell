#if !ONPREMISES
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Graph;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Remove, "PnPMicrosoft365GroupOwner")]
    [Alias("Remove-PnPUnifiedGroupOwner")]
    [CmdletHelp("Removes owners from a particular Microsoft 365 Group",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Remove-PnPMicrosoft365GroupOwner -Identity ""Project Team"" -Users ""john@contoso.onmicrosoft.com"",""jane@contoso.onmicrosoft.com""",
       Remarks = @"Removes the provided two users as owners from the Microsoft 365 Group named ""Project Team""",
       SortOrder = 1)]
    [CmdletRelatedLink(Text = "Documentation", Url = "https://docs.microsoft.com/graph/api/group-delete-owners")]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All | MicrosoftGraphApiPermission.Directory_ReadWrite_All)]
    public class RemoveMicrosoft365GroupOwner : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The Identity of the Microsoft 365 Group to remove owners from")]
        public Microsoft365GroupPipeBind Identity;

        [Parameter(Mandatory = true, HelpMessage = "The UPN(s) of the user(s) to remove as owners from the Microsoft 365 Group")]
        public string[] Users;

        protected override void ExecuteCmdlet()
        {
            UnifiedGroupEntity group = null;

            if (Identity != null)
            {
                group = Identity.GetGroup(AccessToken);
            }

            if (group != null)
            {
                UnifiedGroupsUtility.RemoveUnifiedGroupOwners(group.GroupId, Users, AccessToken);
            }
        }
    }
}
#endif