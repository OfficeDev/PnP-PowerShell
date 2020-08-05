#if !ONPREMISES
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Graph;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.IO;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Set, "PnPMicrosoft365Group")]
    [Alias("Set-PnPUnifiedGroup")]
    [CmdletHelp("Sets Microsoft 365 Group properties",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Set-PnPMicrosoft365Group -Identity $group -DisplayName ""My Displayname""",
       Remarks = "Sets the display name of the group where $group is a Group entity",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Set-PnPMicrosoft365Group -Identity $groupId -Descriptions ""My Description"" -DisplayName ""My DisplayName""",
       Remarks = "Sets the display name and description of a group based upon its ID",
       SortOrder = 2)]
    [CmdletExample(
       Code = @"PS:> Set-PnPMicrosoft365Group -Identity $group -GroupLogoPath "".\MyLogo.png""",
       Remarks = "Sets a specific Microsoft 365 Group logo",
       SortOrder = 3)]
    [CmdletExample(
       Code = @"PS:> Set-PnPMicrosoft365Group -Identity $group -IsPrivate:$false",
       Remarks = "Sets a group to be Public if previously Private",
       SortOrder = 4)]
    [CmdletExample(
       Code = @"PS:> Set-PnPMicrosoft365Group -Identity $group -Owners demo@contoso.com",
       Remarks = "Sets demo@contoso.com as owner of the group",
       SortOrder = 5)]
    [CmdletExample(
       Code = @"PS:> Set-PnPMicrosoft365Group -Identity $group -HideFromOutlookClients:$false",
       Remarks = "Ensures the provided group will be shown in Outlook clients",
       SortOrder = 6)]
    [CmdletRelatedLink(Text = "Documentation", Url = "https://docs.microsoft.com/graph/api/group-update")]
    [CmdletMicrosoftGraphApiPermission(MicrosoftGraphApiPermission.Group_ReadWrite_All)]
    public class SetMicrosoft365Group : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The Identity of the Microsoft 365 Group", ValueFromPipeline = true)]
        public Microsoft365GroupPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "The DisplayName of the group to set")]
        public string DisplayName;

        [Parameter(Mandatory = false, HelpMessage = "The Description of the group to set")]
        public string Description;

        [Parameter(Mandatory = false, HelpMessage = "The array UPN values of owners to set to the group. Note: Will replace owners.")]
        public String[] Owners;

        [Parameter(Mandatory = false, HelpMessage = "The array UPN values of members to set to the group. Note: Will replace members.")]
        public String[] Members;

        [Parameter(Mandatory = false, HelpMessage = "Makes the group private when selected")]
        public SwitchParameter IsPrivate;

        [Parameter(Mandatory = false, HelpMessage = "The path to the logo file of to set. Logo must be at least 48 pixels wide and may be at most 4 MB in size. Requires Site.ReadWrite.All permissions.")]
        public string GroupLogoPath;

        [Parameter(Mandatory = false, HelpMessage = "Creates a Microsoft Teams team associated with created group")]
        public SwitchParameter CreateTeam;

        [Parameter(Mandatory = false, HelpMessage = "Hides the group from the Global Address List")]
        public bool? HideFromAddressLists;

        [Parameter(Mandatory = false, HelpMessage = "Hides the group from Outlook Clients")]
        public bool? HideFromOutlookClients;

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
                bool? isPrivateGroup = null;
                if (IsPrivate.IsPresent)
                {
                    isPrivateGroup = IsPrivate.ToBool();
                }
                try
                {
                    UnifiedGroupsUtility.UpdateUnifiedGroup(
                        groupId: group.GroupId,
                        accessToken: AccessToken,
                        displayName: DisplayName,
                        description: Description,
                        owners: Owners,
                        members: Members,
                        groupLogo: groupLogoStream,
                        isPrivate: isPrivateGroup,
                        createTeam: CreateTeam);

                    if (ParameterSpecified(nameof(HideFromAddressLists)) || ParameterSpecified(nameof(HideFromOutlookClients)))
                    {
                        // For this scenario a separate call needs to be made
                        UnifiedGroupsUtility.SetUnifiedGroupVisibility(group.GroupId, AccessToken, HideFromAddressLists, HideFromOutlookClients);
                    }
                }
                catch(Exception e)
                {
                    while (e.InnerException != null) e = e.InnerException;
                    WriteError(new ErrorRecord(e, "GROUPUPDATEFAILED", ErrorCategory.InvalidOperation, this));
                }
            }
            else
            {
                WriteError(new ErrorRecord(new Exception("Group not found"), "GROUPNOTFOUND", ErrorCategory.ObjectNotFound, this));
            }
        }
    }
}
#endif
