#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsCommon.New, "PnPTenantSequenceCommunicationSite", SupportsShouldProcess = true)]
    [CmdletHelp("Creates a communication site object",
        Category = CmdletHelpCategory.Provisioning, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> $site = New-PnPTenantSequenceCommunicationSite -Url ""/sites/mycommunicationsite"" -Title ""My Team Site""",
       Remarks = "Creates a new communication site object with the specified variables",
       SortOrder = 1)]
    public class NewTenantSequenceCommunicationSite : BasePSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Url;

        [Parameter(Mandatory = true)]
        public string Title;

        [Parameter(Mandatory = false)]
        public uint Language;

        [Parameter(Mandatory = false)]
        public string Owner;

        [Parameter(Mandatory = false)]
        public string Description;

        [Parameter(Mandatory = false)]
        public string Classification;

        [Parameter(Mandatory = false)]
        public string SiteDesignId;

        [Parameter(Mandatory = false)]
        public SwitchParameter HubSite;

        [Parameter(Mandatory = false)]
        public SwitchParameter AllowFileSharingForGuestUsers;

        [Parameter(Mandatory = false)]
        public string[] TemplateIds;

        protected override void ProcessRecord()
        {
            var site = new CommunicationSiteCollection
            {
                Url = Url,
                Language = (int)Language,
                Owner = Owner,
                AllowFileSharingForGuestUsers = AllowFileSharingForGuestUsers.IsPresent,
                Classification = Classification,
                Description = Description,
                IsHubSite = HubSite.IsPresent,
                Title = Title,
            };
            if(ParameterSpecified(nameof(SiteDesignId)))
            {
                site.SiteDesign = SiteDesignId;
            }
            if (TemplateIds != null)
            {
                site.Templates.AddRange(TemplateIds.ToList());
            }
            WriteObject(site);
        }
    }
}
#endif