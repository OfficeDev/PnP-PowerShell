#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using PnP.PowerShell.CmdletHelpAttributes;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsCommon.New, "PnPTenantSequenceTeamNoGroupSubSite", SupportsShouldProcess = true)]
    [CmdletHelp("Creates a team site subsite with no Microsoft 365 group object",
        Category = CmdletHelpCategory.Provisioning, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> $site = New-PnPTenantSequenceTeamNoGroupSubSite -Url ""MyTeamSubsite"" -Title ""My Team Site"" -TimeZoneId 4",
       Remarks = "Creates a new team site subsite object with the specified variables",
       SortOrder = 1)]
    public class NewTenantSequenceTeamNoGroupSubSite : PSCmdlet
    {

        [Parameter(Mandatory = true)]
        public string Url;

        [Parameter(Mandatory = true)]
        public string Title;

        [Parameter(Mandatory = true)]
        public uint TimeZoneId;

        [Parameter(Mandatory = false)]
        public uint Language;

        [Parameter(Mandatory = false)]
        public string Description;

        [Parameter(Mandatory = false)]
        public string[] TemplateIds;

        [Parameter(Mandatory = false)]
        public SwitchParameter QuickLaunchDisabled;

        [Parameter(Mandatory = false)]
        public SwitchParameter UseDifferentPermissionsFromParentSite;

        protected override void ProcessRecord()
        {
            var site = new TeamNoGroupSubSite()
            {
                Url = Url,
                Language = (int)Language,
                QuickLaunchEnabled = !QuickLaunchDisabled.IsPresent,
                UseSamePermissionsAsParentSite = !UseDifferentPermissionsFromParentSite.IsPresent,
                TimeZoneId = (int)TimeZoneId,
                Description = Description,
                Title = Title
            };
            if (TemplateIds != null)
            {
                site.Templates.AddRange(TemplateIds.ToList());
            }
            WriteObject(site);
        }
    }
}
#endif