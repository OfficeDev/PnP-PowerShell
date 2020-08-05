#if !ONPREMISES
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Graph;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Add, "PnPMicrosoft365GroupMember")]
    [Alias("Add-PnPUnifiedGroupMember")]
    [CmdletHelp("Adds members to a particular Microsoft 365 Group",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Add-PnPMicrosoft365GroupMember -Identity ""Project Team"" -Users ""john@contoso.onmicrosoft.com"",""jane@contoso.onmicrosoft.com""",
       Remarks = @"Adds the provided two users as additional members to the Microsoft 365 Group named ""Project Team""",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Add-PnPMicrosoft365GroupMember -Identity ""Project Team"" -Users ""john@contoso.onmicrosoft.com"",""jane@contoso.onmicrosoft.com"" -RemoveExisting",
       Remarks = @"Sets the provided two users as the only members of the Microsoft 365 Group named ""Project Team"" by removing any current existing members first",
       SortOrder = 2)]
    [CmdletRelatedLink(Text = "Documentation", Url = "https://docs.microsoft.com/graph/api/group-post-members")]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.None, MicrosoftGraphApiPermission.User_ReadWrite_All | MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class AddMicrosoft365GroupMember : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The Identity of the Microsoft 365 Group to add members to")]
        public Microsoft365GroupPipeBind Identity;

        [Parameter(Mandatory = true, HelpMessage = "The UPN(s) of the user(s) to add to the Microsoft 365 Group as a member")]
        public string[] Users;

        [Parameter(Mandatory = false, HelpMessage = "If provided, all existing members will be removed and only those provided through Users will become members")]
        public SwitchParameter RemoveExisting;

        protected override void ExecuteCmdlet()
        {
            UnifiedGroupEntity group = null;

            if (Identity != null)
            {
                group = Identity.GetGroup(AccessToken);
            }

            if (group != null)
            {
                UnifiedGroupsUtility.AddUnifiedGroupMembers(group.GroupId, Users, AccessToken, RemoveExisting.ToBool());
            }
        }
    }
}
#endif